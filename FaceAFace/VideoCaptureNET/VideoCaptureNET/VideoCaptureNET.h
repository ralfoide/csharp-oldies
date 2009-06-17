// VideoCaptureNET library
// Copyright 2004, Robert Burke, robert.burke@gmail.com
// http://www.mle.ie/~rob/videocapturenet
//
//    This file is part of VideoCaptureNET.
//
//    VideoCaptureNET is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 2 of the License, or
//    (at your option) any later version.
//
//    VideoCaptureNET is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with VideoCaptureNET; if not, write to the Free Software
//    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA


#pragma once
#include "stdafx.h"
#include "wtypes.h"
#include <dshow.h>
#include "MType.h"
#include <qedit.h>
#include <atlbase.h>
#include <vcclr.h>
#define atlTraceException atlTraceGeneral

#define SAFE_RELEASE(x) { if (x) x->Release(); x = NULL; }
using namespace System;
using namespace System::Drawing;
using namespace System::Collections;
using namespace System::Runtime::InteropServices;

namespace Allenwood
{
	namespace VideoCaptureNet
	{

		public __gc class VideoCaptureDevice;

		// Note: this object is a SEMI-COM object, and can only be created statically.
		// We use this little semi-com object to handle the sample-grab-callback,
		// since the callback must provide a COM interface. We could have had an interface
		// where you provided a function-call callback, but that's really messy, so we
		// did it this way. You can put anything you want into this C++ object, even
		// a pointer to a CDialog. Be aware of multi-thread issues though.
		//
		class CSampleGrabberCB : public ISampleGrabberCB
		{
		public:
			// these will be set by the main thread below. We need to
			// know this in order to write out the bmp
			long lWidth;
			long lHeight;

			CSampleGrabberCB( ) { }

			// fake out any COM ref counting
			STDMETHODIMP_(ULONG) AddRef() { return 2; }
			STDMETHODIMP_(ULONG) Release() { return 1; }

			// fake out any COM QI'ing
			STDMETHODIMP QueryInterface(REFIID riid, void ** ppv)
			{
				if( riid == IID_ISampleGrabberCB || riid == IID_IUnknown )
				{
					*ppv = (void *) static_cast<ISampleGrabberCB*> ( this );
					return NOERROR;
				}
				return E_NOINTERFACE;
			}

			// we don't implement this interface
			STDMETHODIMP SampleCB( double SampleTime, IMediaSample * pSample )
			{
				return 0;
			}


		public:
			gcroot<VideoCaptureDevice *> VCD;

			// This is NOT the main app thread.
			// As a workaround, copy the bitmap data during the callback,
			// post a message to our app, and write the data later.
			STDMETHODIMP BufferCB( double dblSampleTime, BYTE * pBuffer, long lBufferSize ); // Defined in the CPP file.

		};



		class UnmanagedVideoResources
		{
		public:
			UnmanagedVideoResources(String *devicePath, VideoCaptureDevice *vcd)
			{
				const char* str = (const char*)
					(Marshal::StringToHGlobalAnsi(devicePath)).ToPointer();
				DevicePath = str;
				mCB.VCD = vcd;
				m_pProcAmp = 0;
				InitStillGraph();

			}
			~UnmanagedVideoResources()
			{
				ClearGraph();
				Marshal::FreeHGlobal(IntPtr((void*)DevicePath));
			}

		private:
			const char * DevicePath;

			// This semi-COM object will receive sample callbacks for us
			CSampleGrabberCB mCB;

		public:
			void Dispose()
			{
				ClearGraph();
			}
			CSampleGrabberCB* GetSampleGrabberCallbackSemiCOM() { return &mCB; }




		public:

			// the capture still graph
			CComPtr< IGraphBuilder > m_pGraph;

			//// the sample grabber for grabbing stills
			CComPtr< ISampleGrabber > m_pGrabber;

			IBaseFilter ** ppCapBaseFilter;

			void Error(char *errorMessage);

			void GetCapDeviceMatchingDevicePath( IBaseFilter ** ppCap )
			{
				HRESULT hr;

				//ASSERT(ppCap);
				if (!ppCap)
					return;

				*ppCap = NULL;

				// create an enumerator
				//
				CComPtr< ICreateDevEnum > pCreateDevEnum;
				pCreateDevEnum.CoCreateInstance( CLSID_SystemDeviceEnum );

				if( !pCreateDevEnum )
					return;

				// enumerate video capture devices
				//
				CComPtr< IEnumMoniker > pEm;
				pCreateDevEnum->CreateClassEnumerator( CLSID_VideoInputDeviceCategory, &pEm, 0 );

				//ASSERT(pEm);
				if( !pEm )
					return;

				pEm->Reset( );

				while( 1 )
				{
					ULONG ulFetched = 0;
					CComPtr< IMoniker > pM;

					hr = pEm->Next( 1, &pM, &ulFetched );
					if( hr != S_OK )
						break;

					// get the property bag interface from the moniker
					//
					CComPtr< IPropertyBag > pBag;
					hr = pM->BindToStorage( 0, 0, IID_IPropertyBag, (void**) &pBag );
					if( hr != S_OK )
						continue;

					// ask for the device path
					//
					CComVariant var;
					var.vt = VT_BSTR;
					hr = pBag->Read( L"DevicePath", &var, NULL );
					if( hr != S_OK )
						continue;
					String *enumeratedDevicePath = var.bstrVal;
					SysFreeString(var.bstrVal);

					String *desiredDevicePath = this->DevicePath;


					if (!(enumeratedDevicePath->CompareTo(desiredDevicePath) == 0))
						continue;

					// ask for the actual filter
					//
					hr = pM->BindToObject( 0, 0, IID_IBaseFilter, (void**) ppCap );
					if( *ppCap )
						break;
				}

				return;
			}

			CComPtr< IBaseFilter > m_pCap;
			CComPtr <IBaseFilter> m_pRenderer;
			CComPtr<ICaptureGraphBuilder2> m_pCGB2;
			IAMVideoProcAmp *m_pProcAmp;


		private:
			bool InitStillGraph( )
			{
				HRESULT hr;

				// create a filter graph
				//
				hr = m_pGraph.CoCreateInstance( CLSID_FilterGraph );
				if( !m_pGraph )
				{
					Error( "Could not create filter graph" );
					return false;
				}

				// get the capture device that matches this instance's DevicePath.
				//
				GetCapDeviceMatchingDevicePath( &m_pCap );
				if( !m_pCap )
				{
					Error( ("No capture device matching the device path was detected on your system.\r\n"));
					return false;
				}

				// add the capture filter to the graph
				//
				hr = m_pGraph->AddFilter( m_pCap, L"Cap" );
				if( FAILED( hr ) )
				{
					Error( ("Could not put capture device in graph"));
					return false;
				}



				// create a sample grabber
				//
				hr = m_pGrabber.CoCreateInstance( CLSID_SampleGrabber );
				if( !m_pGrabber )
				{
					Error( "Could not create SampleGrabber (is qedit.dll registered?)");
					return false;
				}
				CComQIPtr< IBaseFilter, &IID_IBaseFilter > pGrabBase( m_pGrabber );

				// force it to connect to video, 24 bit
				//
				CMediaType VideoType;
				VideoType.SetType( &MEDIATYPE_Video );
				VideoType.SetSubtype( &MEDIASUBTYPE_RGB24 );


				hr = m_pGrabber->SetMediaType( &VideoType ); // shouldn't fail
				if( FAILED( hr ) )
				{
					Error( ("Could not set media type"));
					return false;
				}

				// add the grabber to the graph
				//
				hr = m_pGraph->AddFilter( pGrabBase, L"Grabber" );
				if( FAILED( hr ) )
				{
					Error( ("Could not put sample grabber in graph"));
					return false;
				}

				// build the graph
				hr = m_pCGB2.CoCreateInstance (CLSID_CaptureGraphBuilder2, NULL, CLSCTX_INPROC);
				if (FAILED( hr ))
				{
					Error(("Can't get a ICaptureGraphBuilder2 reference"));
					return false;
				}

				hr = m_pCGB2->SetFiltergraph( m_pGraph );
				if (FAILED( hr ))
				{
					Error(("SetGraph failed"));
					return false;
				}


				// If there is a VP pin present on the video device, then put the
				// renderer on CLSID_NullRenderer
				CComPtr<IPin> pVPPin;
				hr = m_pCGB2->FindPin(
					m_pCap,
					PINDIR_OUTPUT,
					&PIN_CATEGORY_VIDEOPORT,
					NULL,
					FALSE,
					0,
					&pVPPin);




				//// If there is a VP pin, put the renderer on NULL Renderer
				//if (S_OK == hr)
				//{
				//	hr = pRenderer.CoCreateInstance(CLSID_NullRenderer);
				//	if (S_OK != hr)
				//	{
				//		Error(("Unable to make a NULL renderer"));
				//		return S_OK;
				//	}
				//	hr = m_pGraph->AddFilter(pRenderer, L"NULL renderer");
				//	if (FAILED (hr))
				//	{
				//		Error(("Can't add the filter to graph"));
				//		return false;
				//	}
				//}

				// ROB always add a null renderer.
				hr = m_pRenderer.CoCreateInstance(CLSID_NullRenderer);
				if (S_OK != hr)
				{
					Error(("Unable to make a NULL renderer"));
					return false;
				}
				hr = m_pGraph->AddFilter(m_pRenderer, L"NULL renderer");
				if (FAILED (hr))
				{
					Error(("Can't add the filter to graph"));
					return false;
				}


				/// SECOND VERSION
				//// just try to render capture pin.
				hr = m_pCGB2->RenderStream(
					&PIN_CATEGORY_CAPTURE,
					&MEDIATYPE_Video,
					m_pCap,
					pGrabBase,
					m_pRenderer);


				// ORIGINAL VERSION
				//hr = pCGB2->RenderStream(
				//	&PIN_CATEGORY_PREVIEW,
				//	&MEDIATYPE_Interleaved,
				//	pCap,
				//	pGrabBase,
				//	pRenderer);
				//if (FAILED (hr))
				//{
				//	// try to render preview pin
				//	hr = pCGB2->RenderStream(
				//		&PIN_CATEGORY_PREVIEW,
				//		&MEDIATYPE_Video,
				//		pCap,
				//		pGrabBase,
				//		pRenderer);

				//	// try to render capture pin
				//	if( FAILED( hr ) )
				//	{
				//		hr = pCGB2->RenderStream(
				//			&PIN_CATEGORY_CAPTURE,
				//			&MEDIATYPE_Video,
				//			pCap,
				//			pGrabBase,
				//			pRenderer);
				//	}
				//}

				//if( FAILED( hr ) )
				//{
				//	Error( ("Can't build the graph") );
				//	return false;
				//}

				// ask for the connection media type so we know how big
				// it is, so we can write out bitmaps
				//
				AM_MEDIA_TYPE mt;
				hr = m_pGrabber->GetConnectedMediaType( &mt );
				if ( FAILED( hr) )
				{
					Error( ("Could not read the connected media type"));
					return false;
				}

				VIDEOINFOHEADER * vih = (VIDEOINFOHEADER*) mt.pbFormat;
				mCB.lWidth  = vih->bmiHeader.biWidth;
				mCB.lHeight = vih->bmiHeader.biHeight;
				FreeMediaType( mt );

				// don't buffer the samples as they pass through
				//
				hr = m_pGrabber->SetBufferSamples( FALSE );

				// only grab one at a time, stop stream after
				// grabbing one sample
				//
				hr = m_pGrabber->SetOneShot( FALSE );

				// set the callback, so we can grab the one sample
				//
				hr = m_pGrabber->SetCallback( &mCB, 1 );


				// find the video window and stuff it in our window
				//
				// REMOVED BY ROB
				//CComQIPtr< IVideoWindow, &IID_IVideoWindow > pWindow = m_pGraph;
				//if( !pWindow )
				//{
				//	Error( ("Could not get video window interface"));
				//	return false;
				//}

				// set up the preview window to be in our dialog
				// instead of floating popup
				//
				//HWND hwndPreview = NULL;
				//GetDlgItem( IDC_PREVIEW, &hwndPreview );
				//RECT rc;
				//::GetWindowRect( hwndPreview, &rc );

				//hr = pWindow->put_Owner( (OAHWND) hwndPreview );
				//hr = pWindow->put_Left( 0 );
				//hr = pWindow->put_Top( 0 );
				//hr = pWindow->put_Width( rc.right - rc.left );
				//hr = pWindow->put_Height( rc.bottom - rc.top );
				//hr = pWindow->put_WindowStyle( WS_CHILD | WS_CLIPSIBLINGS );
				//hr = pWindow->put_Visible( OATRUE );

				// Add our graph to the running object table, which will allow
				// the GraphEdit application to "spy" on our graph
				//#ifdef REGISTER_FILTERGRAPH
				//			hr = AddGraphToRot(m_pGraph, &g_dwGraphRegister);
				//			if (FAILED(hr))
				//			{
				//				Error(TEXT("Failed to register filter graph with ROT!"));
				//				g_dwGraphRegister = 0;
				//			}
				//#endif



				hr = m_pCap->QueryInterface(IID_IAMVideoProcAmp, (void**)&m_pProcAmp);
				if (FAILED(hr))
				{
					m_pProcAmp = 0;
				}



				//UpdateStatus(_T("Previewing Live Video"));
				return true;
			}

		public:
			bool SetResolution(System::Drawing::Size desiredResolution)
			{
				HRESULT hr;

				// ROB - I'm gonna try to find the output pin that I can use to get possible resolutions
				CComPtr<IPin> pCapPin;
				hr = m_pCGB2->FindPin(
					m_pCap,
					PINDIR_OUTPUT,
					&PIN_CATEGORY_CAPTURE,
					NULL,
					FALSE,
					0,
					&pCapPin);

				// Now get the actual format of the stream data
				IAMStreamConfig *pCfg=0;
				hr = pCapPin->QueryInterface(IID_IAMStreamConfig, (void **)&pCfg);
				int iCount, iSize;
				VIDEO_STREAM_CONFIG_CAPS scc;
				AM_MEDIA_TYPE *pmt;

				hr = pCfg->GetNumberOfCapabilities(&iCount, &iSize);

				ArrayList * toRet = new ArrayList();

				if (sizeof(scc) != iSize)
				{
					// This is not the structure we were expecting.
					return false;
				}
				// Get the first format.
				for (int iii = 0; iii < iCount; iii++)
				{
					hr = pCfg->GetStreamCaps(iii, &pmt, reinterpret_cast<BYTE*>(&scc));
					if (hr == S_OK)
					{
						// Is it VIDEOINFOHEADER and RGB24?
						if (pmt->formattype == FORMAT_VideoInfo &&
							pmt->subtype == MEDIASUBTYPE_RGB24)
						{
							if (scc.MinOutputSize.cx == desiredResolution.get_Width()
								&& scc.MinOutputSize.cy == desiredResolution.get_Height() )
							{
								// Now set the format.
								hr = pCfg->SetFormat(pmt);
								//							m_pGraph->Reconnect(pCfg);

								if (FAILED(hr))
								{
									DeleteMediaType(pmt);
									return false;
								}

								DeleteMediaType(pmt);
								return true;
							}
						}
						DeleteMediaType(pmt);
					}
				}
				return false;
			}

			System::Drawing::Size GetResolution()
			{
				HRESULT hr;
				AM_MEDIA_TYPE mt;
				hr = m_pGrabber->GetConnectedMediaType( &mt );
				if ( FAILED( hr) )
				{
					Error( ("Could not read the connected media type"));
					return false;
				}

				VIDEOINFOHEADER * vih = (VIDEOINFOHEADER*) mt.pbFormat;
				System::Drawing::Size toRet(vih->bmiHeader.biWidth, mCB.lHeight = vih->bmiHeader.biHeight);
				FreeMediaType( mt );
				return toRet;
			}




			System::Drawing::Size  GetResolutionCaps() []
			{
				HRESULT hr;

				// ROB - I'm gonna try to find the output pin that I can use to get possible resolutions
				CComPtr<IPin> pCapPin;
				hr = m_pCGB2->FindPin(
					m_pCap,
					PINDIR_OUTPUT,
					&PIN_CATEGORY_CAPTURE,
					NULL,
					FALSE,
					0,
					&pCapPin);

				// Now get the actual format of the stream data
				IAMStreamConfig *pCfg=0;
				hr = pCapPin->QueryInterface(IID_IAMStreamConfig, (void **)&pCfg);
				int iCount, iSize;
				VIDEO_STREAM_CONFIG_CAPS scc;
				AM_MEDIA_TYPE *pmt;

				hr = pCfg->GetNumberOfCapabilities(&iCount, &iSize);

				ArrayList * toRet = new ArrayList();

				if (sizeof(scc) != iSize)
				{
					// This is not the structure we were expecting.
					return new System::Drawing::Size [0];
				}
				// Get the first format.
				for (int iii = 0; iii < iCount; iii++)
				{
					hr = pCfg->GetStreamCaps(iii, &pmt, reinterpret_cast<BYTE*>(&scc));
					if (hr == S_OK)
					{
						// Is it VIDEOINFOHEADER and RGB24?
						if (pmt->formattype == FORMAT_VideoInfo &&
							pmt->subtype == MEDIASUBTYPE_RGB24)
						{
							// Find the smallest output size.
							LONG width = scc.MinOutputSize.cx;
							LONG height = scc.MinOutputSize.cy;
							System::Drawing::Size mySize(width, height);// = new System::Drawing::Size(width, height);
							toRet->Add(__box(mySize));
							////LONG cbPixel = 2;  // Bytes per pixel in UYVY

							//// Modify the format block.
							//VIDEOINFOHEADER *pVih =
							//	reinterpret_cast<VIDEOINFOHEADER*>(pmt->pbFormat);
							//pVih->bmiHeader.biWidth = width;
							//pVih->bmiHeader.biHeight = height;

							////// Set the sample size and image size.
							////// (Round the image width up to a DWORD boundary.)
							////pmt->lSampleSize = pVih->bmiHeader.biSizeImage =
							////	((width + 3) & ~3) * height * cbPixel;

							////// Now set the format.
							////hr = pCfg->SetFormat(pmt);
							////if (FAILED(hr))
							////{
							////	MessageBox(NULL, TEXT("SetFormat Failed\n"), NULL, MB_OK);
							////}
						}
						DeleteMediaType(pmt);
					}
				}
				return dynamic_cast<System::Drawing::Size[]> (toRet->ToArray(__typeof(System::Drawing::Size)));

			}

			bool GetPropertyRange(VideoProcAmpProperty videoProcProperty, long *propMin, long *propMax, long *propStep, long *propDefaultValue, long *propFlags)
			{
				HRESULT hr;

				if (!m_pProcAmp) return false;

				// Get the range and default value.
				hr = m_pProcAmp->GetRange(videoProcProperty, propMin, propMax, propStep, propDefaultValue, propFlags);
				if (SUCCEEDED(hr))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			bool GetPropertyValue(VideoProcAmpProperty videoProcProperty, long *propValue, long *propFlags)
			{
				HRESULT hr;
				if (!m_pProcAmp) return false;

				// Get the current value.
				hr = m_pProcAmp->Get(videoProcProperty, propValue, propFlags);

				if (SUCCEEDED(hr))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			bool SetPropertyValue(VideoProcAmpProperty videoProcProperty, long propValue, long propFlags)
			{
				HRESULT hr;
				if (!m_pProcAmp) return false;

				// Get the current value.
				hr = m_pProcAmp->Set(videoProcProperty, propValue, propFlags);

				if (SUCCEEDED(hr))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			//bool ShowConfigDialog()
			//{
			//	return ShowConfigDialog(0);
			//}

			//bool ShowConfigDialog(HWND hwndParent)
			//{
			//	bool success = false;
			//	HRESULT hr;
			//	CComQIPtr< IMediaControl, &IID_IMediaControl > pGph = m_pGraph;
			//	CComQIPtr< IMediaControl, &IID_IMediaControl > pControl = m_pCap;
			//	pGph->Stop(); // Stop the graph.

			//	// Query the capture filter for the IAMVfwCaptureDialogs interface.
			//	IAMVfwCaptureDialogs *pVfw = 0;
			//	hr = m_pCap->QueryInterface(IID_IAMVfwCaptureDialogs, (void**)&pVfw);
			//	if (SUCCEEDED(hr))
			//	{
			//		// Check if the device supports this dialog box.
			//		if (S_OK == pVfw->HasDialog(VfwCaptureDialog_Source))
			//		{
			//			// Show the dialog box.
			//			hr = pVfw->ShowDialog(VfwCaptureDialog_Source, hwndParent);
			//		}
			//		success = (SUCCEEDED(hr));
			//	}
			//	pGph->Run();
			//	return success;

			//}




			bool Enable()
			{
				HRESULT hr;

				// run the graph
				//
				CComQIPtr< IMediaControl, &IID_IMediaControl > pControl = m_pGraph;
				hr = pControl->Run( );
				if( FAILED( hr ) )
				{
					Error( ("Could not run graph"));
					return false;
				}

				OAFilterState pfs;

				pControl->GetState(INFINITE, &pfs);

				if (pfs == State_Running) return true;
				else return false;


				return true;
			}

			bool Disable()
			{
				HRESULT hr;
				// Destroy capture graph
				if( m_pGraph )
				{
					// have to wait for the graphs to stop first
					//
					CComQIPtr< IMediaControl, &IID_IMediaControl > pControl = m_pGraph;
					if( pControl )
						hr = pControl->Stop( );

					if (FAILED (hr) )
					{
						Error( ("Could not stop graph"));
						return false;

					}
					OAFilterState pfs;


					pControl->GetState(INFINITE, &pfs);
					if (pfs == State_Stopped) return true;
					else return false;

				}
				return true;

			}

		private:

			void ClearGraph( )
			{
				if (m_pProcAmp)
					m_pProcAmp->Release();

				// Destroy capture graph
				if( m_pGraph )
				{
					// have to wait for the graphs to stop first
					//
					CComQIPtr< IMediaControl, &IID_IMediaControl > pControl = m_pGraph;
					if( pControl )
						pControl->Stop( );

					//// make the window go away before we release graph
					//// or we'll leak memory/resources
					////
					CComQIPtr< IVideoWindow, &IID_IVideoWindow > pWindow = m_pGraph;
					if( pWindow )
					{
						pWindow->put_Visible( OAFALSE );
						pWindow->put_Owner( NULL );
					}

					//#ifdef REGISTER_FILTERGRAPH
					//				// Remove filter graph from the running object table
					//				if (g_dwGraphRegister)
					//					RemoveGraphFromRot(g_dwGraphRegister);
					//#endif

					m_pGraph.Release( );
				}
				if (m_pGrabber)
				{
					m_pGrabber.Release();
				}
				if (m_pCGB2)
				{
					m_pCGB2.Release();
				}

				// ROB -- trying to nuke anything that needs nuking.
				if (m_pCap)
				{
					m_pCap->Stop();
					m_pCap.Release();
				}
				if (m_pRenderer)
				{
					m_pRenderer->Stop();
					m_pRenderer.Release();
				}

				// Destroy playback graph, if it exists
				//if( m_pPlayGraph )
				//{
				//	CComQIPtr< IMediaControl, &IID_IMediaControl > pControl = m_pPlayGraph;
				//	if( pControl )
				//		pControl->Stop( );

				//	CComQIPtr< IVideoWindow, &IID_IVideoWindow > pWindow = m_pPlayGraph;
				//	if( pWindow )
				//	{
				//		pWindow->put_Visible( OAFALSE );
				//		pWindow->put_Owner( NULL );
				//	}

				//	m_pPlayGraph.Release( );
				//}
			}





		};

	public __gc class VideoCaptureFrameEventArgs : public EventArgs
	{
	public:
		VideoCaptureFrameEventArgs(double dblSampleTime, BYTE * pBuffer, Size resolution, long lBufferSize)
		{
			BytesRep = __gc new System::Byte[lBufferSize];
			Marshal::Copy(pBuffer, BytesRep, 0, lBufferSize);
			SampleTimeRep = dblSampleTime;
			ResolutionRep = resolution;
		}
	private:
		System::Byte BytesRep __gc[];
		System::Double SampleTimeRep;
		System::Drawing::Size ResolutionRep;
	public:

		__property System::Byte get_Bytes() [] { return BytesRep; }
		__property System::Drawing::Size get_Resolution()  { return ResolutionRep; }
		__property System::Double get_SampleTime() { return SampleTimeRep; }

		System::Drawing::Bitmap * GetBitmap()
		{
			System::Drawing::Bitmap * toRet = new System::Drawing::Bitmap( ResolutionRep.Width, ResolutionRep.Height, System::Drawing::Imaging::PixelFormat::Format24bppRgb);
			System::Drawing::Imaging::BitmapData * bmd = toRet->LockBits(System::Drawing::Rectangle(0, 0, toRet->Width, toRet->Height), System::Drawing::Imaging::ImageLockMode::ReadWrite, System::Drawing::Imaging::PixelFormat::Format24bppRgb);
			byte * scan0 = (byte*)bmd->Scan0.ToPointer();
			int numToDo = ResolutionRep.Width * ResolutionRep.Height * 3;
			//Marshal::Copy(BytesRep, scan0, 0, numToDo);
			for (int i = 0; i < numToDo; i++)
			{
				scan0[i] = BytesRep[i];
			}
			toRet->UnlockBits(bmd);

			toRet->RotateFlip(RotateFlipType::Rotate180FlipNone); // why is it rotated?!
			return toRet;

		}


	};


	public __delegate void VideoCaptureFrameEventHandler(Object *sender, VideoCaptureFrameEventArgs *e);

	public __gc class VideoCaptureDeviceDesc
	{
	private public: // That means "internal" in C# speak - first keyword is external viz, second is internal viz.

		VideoCaptureDeviceDesc(int deviceNumber, String *name, String *devPath, System::Drawing::Size availableResolutions[])
		{
			NameRep = name;
			DevPathRep = devPath;
			DeviceNumberRep = deviceNumber;
			AvailableResolutionsRep = availableResolutions;
		}

	public:

		System::String * ToString() { return NameRep; }

		__property System::Drawing::Size get_AvailableResolutions()__gc[]{ return AvailableResolutionsRep; }
		__property int get_DeviceNumber() { return DeviceNumberRep; }
		__property String * get_Name() { return NameRep; }
	public public:
		__property String * get_DevicePath() { return DevPathRep; }
	private:
		int DeviceNumberRep;
		String * NameRep;
		String * DevPathRep;
		System::Drawing::Size  AvailableResolutionsRep[];

	public:
		static VideoCaptureDeviceDesc * GetDefaultDeviceDesc()
		{
			VideoCaptureDeviceDesc * devices []= VideoCaptureDeviceDesc::GetAvailableDeviceDescs();
			if (devices->Count == 0) return 0;
			return devices[0];
		}

		static VideoCaptureDeviceDesc * GetAvailableDeviceDescs() []
		{
			HRESULT hr;
			IEnumMoniker *pEnumCat = NULL;

			const CLSID *clsid = &CLSID_VideoInputDeviceCategory;

			// ROB1 -- I'm gonna create this ICreateDevEnumthing here and destroy it afterwards
			ICreateDevEnum * m_pSysDevEnum;
			hr = CoCreateInstance(CLSID_SystemDeviceEnum, NULL,
				CLSCTX_INPROC, IID_ICreateDevEnum,
				(void **)&m_pSysDevEnum);
			if FAILED(hr)
			{
				CoUninitialize();
				return new VideoCaptureDeviceDesc *[0];
			}


			// Enumerate all filters of the selected category
			hr = m_pSysDevEnum->CreateClassEnumerator(*clsid, &pEnumCat, 0);

			//ASSERT(SUCCEEDED(hr));
			if FAILED(hr)
			{
				SAFE_RELEASE(m_pSysDevEnum); // ROB1
				return new VideoCaptureDeviceDesc *[0];
			}

			// Enumerate all filters
			IMoniker *pMoniker;
			ULONG cFetched;
			VARIANT varName={0};
			VARIANT varDevPath={0};
			int nFilters=0;
			ArrayList *knownFilters = new ArrayList();

			// If there are no video capture filters, return empty array.
			if (!pEnumCat)
			{
				SAFE_RELEASE(m_pSysDevEnum); // ROB1
				return new VideoCaptureDeviceDesc *[0];
			}

			int currentDeviceNumber = 0;

			// Enumerate all items associated with the moniker
			while(pEnumCat->Next(1, &pMoniker, &cFetched) == S_OK)
			{
				IPropertyBag *pPropBag;
				//ASSERT(pMoniker);

				// Associate moniker with a file
				hr = pMoniker->BindToStorage(0, 0, IID_IPropertyBag,
					(void **)&pPropBag);
				//ASSERT(SUCCEEDED(hr));
				//ASSERT(pPropBag);
				if (FAILED(hr))
					continue;

				// Read filter name from property bag
				varName.vt = VT_BSTR;
				hr = pPropBag->Read(L"FriendlyName", &varName, 0);
				if (FAILED(hr))
					continue;

				varDevPath.vt = VT_BSTR;
				hr = pPropBag->Read(L"DevicePath", &varDevPath, 0);
				if (FAILED(hr))
					continue;


				// Get filter name (converting BSTR name to a String)
				String * strName = varName.bstrVal;
				String * strDevPath = varDevPath.bstrVal;
				SysFreeString(varName.bstrVal);
				SysFreeString(varDevPath.bstrVal);
				nFilters++;

				VideoCaptureDeviceDesc* vdd = new VideoCaptureDeviceDesc(currentDeviceNumber, strName, strDevPath, 0);
				knownFilters->Add(vdd);

				// Read filter's CLSID from property bag.  This CLSID string will be
				// converted to a binary CLSID and passed to AddFilter(), which will
				// add the filter's name to the listbox and its CLSID to the listbox
				// item's DataPtr item.  When the user clicks on a filter name in
				// the listbox, we'll read the stored CLSID, convert it to a string,
				// and use it to find the filter's filename in the registry.
				VARIANT varFilterClsid;
				varFilterClsid.vt = VT_BSTR;

				// Read CLSID string from property bag
				//hr = pPropBag->Read(L"CLSID", &varFilterClsid, 0);
				//if(SUCCEEDED(hr))
				//{
				//	CLSID clsidFilter;

				//	// Add filter name and CLSID to listbox
				//	if(CLSIDFromString(varFilterClsid.bstrVal, &clsidFilter) == S_OK)
				//	{
				//		//AddFilter(str, &clsidFilter);
				//	}

				//	SysFreeString(varFilterClsid.bstrVal);
				//}

				// Cleanup interfaces
				SAFE_RELEASE(pPropBag);
				SAFE_RELEASE(pMoniker);
				currentDeviceNumber++;
			}

			SAFE_RELEASE(pEnumCat);
			SAFE_RELEASE(m_pSysDevEnum); // ROB1

			return dynamic_cast<VideoCaptureDeviceDesc*[]> (knownFilters->ToArray(__typeof(VideoCaptureDeviceDesc)));
		}




	};

	public __gc class VideoCaptureDeviceProperty
	{
	public:
		VideoCaptureDeviceProperty(VideoCaptureDevice *vcd, VideoProcAmpProperty directShowProperty); // defined at end of file
	private:
		VideoProcAmpProperty MyVideoProcAmpProperty;
		VideoCaptureDevice *VCD;

		long MaxRep;
		long MinRep;
		long StepRep;
		long DefaultValueRep;
		long FlagsRep;
		bool AvailableRep;

	public:
		__property bool get_Available() { return AvailableRep; }
		__property long get_Max() { return MaxRep; }
		__property long get_Min() { return MinRep; }
		__property long get_Step() { return StepRep; }
		__property long get_Default() { return DefaultValueRep; }

		__property long get_Value();
		__property void set_Value(long newValue);

		//__property bool get_Automatic() { return FlagsRep == VideoProcAmp_Flags_Auto; }
		//__property void set_Automatic(bool isPropertyAutomaticallyControlled)
		//{
		//	FlagsRep == isPropertyAutomaticallyControlled ? VideoProcAmp_Flags_Auto : VideoProcAmp_Flags_Manual;
		//	set_Value(get_Value());
		//}

		System::String * ToString()
		{
			return
				String::Concat(S"{Value=", Value.ToString(), S" Default=", Default.ToString(), S" Min=", Min.ToString(), S" Max=", Max.ToString(), S"}");
		}

	};

	[System::ComponentModel::TypeConverterAttribute (__typeof(System::ComponentModel::ExpandableObjectConverter))]
	public __gc class VideoCaptureDeviceProperties
	{
	public:
		VideoCaptureDeviceProperties(VideoCaptureDevice *vcd); // at end of file. just instantiates the rep stuff below

	private:
		VideoCaptureDeviceProperty * BrightnessRep;
		VideoCaptureDeviceProperty * BacklightCompensationRep;
		VideoCaptureDeviceProperty * ColorEnableRep;
		VideoCaptureDeviceProperty * ContrastRep;
		VideoCaptureDeviceProperty * GainRep;
		VideoCaptureDeviceProperty * GammaRep;
		VideoCaptureDeviceProperty * HueRep;
		VideoCaptureDeviceProperty * SaturationRep;
		VideoCaptureDeviceProperty * SharpnessRep;
		VideoCaptureDeviceProperty * WhiteBalanceRep;
		System::String * ToStringRep;
	public:
		__property VideoCaptureDeviceProperty * get_Brightness() { return BrightnessRep; }
		__property VideoCaptureDeviceProperty * get_BacklightCompensation() { return BacklightCompensationRep; }
		__property VideoCaptureDeviceProperty * get_ColorEnable() { return ColorEnableRep; }
		__property VideoCaptureDeviceProperty * get_Contrast() { return ContrastRep; }
		__property VideoCaptureDeviceProperty * get_Gain() { return GainRep; }
		__property VideoCaptureDeviceProperty * get_Gamma() { return GammaRep; }
		__property VideoCaptureDeviceProperty * get_Hue() { return HueRep; }
		__property VideoCaptureDeviceProperty * get_Saturation() { return SaturationRep; }
		__property VideoCaptureDeviceProperty * get_Sharpness() { return SharpnessRep; }
		__property VideoCaptureDeviceProperty * get_WhiteBalance() { return WhiteBalanceRep; }

		void ResetToDefaultValues()
		{
			if (Brightness->Available) Brightness->Value = Brightness->Default;
			if (BacklightCompensation->Available) BacklightCompensation->Value = BacklightCompensation->Default;
			if (ColorEnable->Available) ColorEnable->Value = ColorEnable->Default;
			if (Contrast->Available) Contrast->Value = Contrast->Default;
			if (Gain->Available) Gain->Value = Gain->Default;
			if (Gamma->Available) Gamma->Value = Gamma->Default;
			if (Hue->Available) Hue->Value = Hue->Default;
			if (Saturation->Available) Saturation->Value = Saturation->Default;
			if (Sharpness->Available) Sharpness->Value = Sharpness->Default;
			if (WhiteBalance->Available) WhiteBalance->Value = WhiteBalance->Default;
		}

		System::String * ToString()
		{
			return  ToStringRep;
		}


	};

	public __gc class VideoCaptureDevice  : public IDisposable
	{
	public:
		VideoCaptureDevice(VideoCaptureDeviceDesc *vcdd)
		{
			if (vcdd == 0)
			{
				System::NullReferenceException * ex = new System::NullReferenceException("Null VideoCaptureDeviceDesc parameter passed into VideoCaptureDevice constructor.");
				throw ex;
			}
			DisposedRep = false;
			DescRep = vcdd;
			VideoResources = 0;
			InitUnmanagedResources();
			PropertiesRep = new VideoCaptureDeviceProperties(this);
			ReadyForNext = true;

		}
	private:
		~VideoCaptureDevice()
		{
			DestroyUnmanagedResources();
		}
	public:
		void Dispose()
		{
			DestroyUnmanagedResources();
		}
	private:
		void InitUnmanagedResources()
		{
			if (VideoResources != 0)
				DestroyUnmanagedResources();
			VideoResources = new UnmanagedVideoResources(DescRep->DevicePath, this);
			ResolutionCached = VideoResources->GetResolution();
		}
		void DestroyUnmanagedResources()
		{
			if (Enabled) Enabled = false;
			if (VideoResources != 0)
			{
				VideoResources->Dispose();
				VideoResources = 0;
				DisposedRep = true;
			}
		}

		//// REP

		VideoCaptureDeviceDesc *DescRep;
		System::Boolean EnabledRep;
		System::Boolean DisposedRep;
		System::Drawing::Size ResolutionCached;
		System::String * ErrorString;
		VideoCaptureDeviceProperties * PropertiesRep;

		UnmanagedVideoResources * VideoResources;

	public private:
		UnmanagedVideoResources * GetVideoResources() { return VideoResources; }
		void DisposeAndThrowException(System::String * exceptionText)
		{
			DisposedRep = true;
			Enabled = false;
			this->Dispose();
			System::InvalidOperationException * ex = new System::InvalidOperationException(exceptionText);
			throw ex;
		}


		void OnSample(double dblSampleTime, BYTE * pBuffer, long lBufferSize )
		{
			if (!ReadyForNext) return;
			Size currentResolution = get_Resolution();
			if (currentResolution.Width * currentResolution.Height * 3 != lBufferSize)
			{
				//printf("bad size");
				return; // we only raise events when the size matches the current resolution, to avoid confusion when the resoltion changes.
			}
			ReadyForNext = false;
			if (FrameCaptured) FrameCaptured(this, new VideoCaptureFrameEventArgs(dblSampleTime, pBuffer, currentResolution, lBufferSize));
			ReadyForNext = true;
		}
	private:
		bool ReadyForNext;


	public:

		__event VideoCaptureFrameEventHandler* FrameCaptured;

		__property VideoCaptureDeviceDesc * get_Desc() { return DescRep; }
		__property System::Boolean get_Enabled() { return EnabledRep; }
		__property System::Boolean get_IsDisposed() { return DisposedRep; }
		__property void set_Enabled(System::Boolean isEnabled)
		{
			if (isEnabled == EnabledRep) return;
			if (isEnabled)
			{
				VideoResources->Enable();
				EnabledRep = true;
			}
			else
			{
				VideoResources->Disable();
				EnabledRep = false;
			}
		}
		__property void set_Resolution(System::Drawing::Size desiredResolution)
		{
			bool shouldReEnable = Enabled;
			if (Enabled)
			{
				Console::WriteLine("disabling");
				set_Enabled(false);
			}
			if (!VideoResources->SetResolution(desiredResolution))
			{
				System::InvalidOperationException * ex = new System::InvalidOperationException("Resolution invalid or can't set resolution.");
				throw ex;
			}
			if (shouldReEnable)
			{
				Console::WriteLine("enabling");
				set_Enabled(true);
			}
			ResolutionCached = VideoResources->GetResolution();
		}
		__property System::Drawing::Size get_Resolution()
		{
			return ResolutionCached;
		}

		__property VideoCaptureDeviceProperties * get_Properties()
		{
			return PropertiesRep;
		}


		System::Drawing::Size  GetResolutionCaps() []
		{
			return VideoResources->GetResolutionCaps();
		}

		//bool ShowConfigDialog()
		//{
		//	return VideoResources->ShowConfigDialog();
		//}
		//bool ShowConfigDialog(IntPtr hWnd)
		//{
		//	return VideoResources->ShowConfigDialog((HWND)hWnd.ToPointer());
		//}

		System::String * ToString()
		{
			return DescRep->get_Name();
		}


		// static methods used to get system devices.

	public:

		static VideoCaptureDevice * GetDefaultDevice()
		{
			VideoCaptureDeviceDesc * devices []= VideoCaptureDeviceDesc::GetAvailableDeviceDescs();
			if (devices->Count == 0) return 0;
			return new VideoCaptureDevice(devices[0]);
		}




	};




	}

}