#pragma once

//*****************
namespace Allenwood
{
//**********************
namespace VideoCaptureNet
{


//**********************************
public __gc class VideoCaptureDevice  : public IDisposable
{
	//-------------------------------------------
	//----------- Public Methods ----------------
	//-------------------------------------------

public:
	//******************************
	VideoCaptureDevice(VideoCaptureDeviceDesc *vcdd)
	{
		if (vcdd == NULL)
		{
			System::NullReferenceException * ex = new System::NullReferenceException("Null VideoCaptureDeviceDesc parameter passed into VideoCaptureDevice constructor.");
			throw ex;
		}

		DisposedRep = false;
		DescRep = vcdd;
		VideoResources = NULL;
		InitUnmanagedResources();
		PropertiesRep = new VideoCaptureDeviceProperties(this);
		ReadyForNext = true;
	}

	//******************************
	void Dispose()
	{
		DestroyUnmanagedResources();
	}


	//-------------------------------------------
	//----------- Private Methods ---------------
	//-------------------------------------------

private:
	//******************************
	~VideoCaptureDevice()
	{
		DestroyUnmanagedResources();
	}

	//******************************
	void InitUnmanagedResources()
	{
		if (VideoResources != NULL)
			DestroyUnmanagedResources();
		VideoResources = new UnmanagedVideoResources(DescRep->DevicePath, this);
		ResolutionCached = VideoResources->GetResolution();
	}

	//******************************
	void DestroyUnmanagedResources()
	{
		if (Enabled) 
			Enabled = false;
		
		if (VideoResources != NULL)
		{
			VideoResources->Dispose();
			VideoResources = NULL;
			DisposedRep = true;
		}
	}

	//******************************
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

	};
