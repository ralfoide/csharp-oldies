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

#include "stdafx.h"

#include "VideoCaptureNET.h"

namespace Allenwood
{
	namespace VideoCaptureNet
	{

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		///// CSampleGrabberCB
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		STDMETHODIMP CSampleGrabberCB::BufferCB( double dblSampleTime, BYTE * pBuffer, long lBufferSize )
		{
			//	printf("BCB!");
			if (!pBuffer)
				return E_POINTER;

			VCD->OnSample(dblSampleTime, pBuffer, lBufferSize);

			// Copy the bitmap data into our global buffer
			//memcpy(cb.pBuffer, pBuffer, lBufferSize);

			// Post a message to our application, telling it to come back
			// and write the saved data to a bitmap file on the user's disk.
			//PostMessage(g_hwnd, WM_CAPTURE_BITMAP, 0, 0L);
			return 0;

		}


		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		///// VideoCaptureDeviceProperties
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




		VideoCaptureDeviceProperties::VideoCaptureDeviceProperties(VideoCaptureDevice *vcd)
		{
			BrightnessRep = new VideoCaptureDeviceProperty(vcd, VideoProcAmp_Brightness);
			BacklightCompensationRep = new VideoCaptureDeviceProperty(vcd, VideoProcAmp_BacklightCompensation);
			ColorEnableRep = new VideoCaptureDeviceProperty(vcd, VideoProcAmp_ColorEnable);
			ContrastRep = new VideoCaptureDeviceProperty(vcd, VideoProcAmp_Contrast);
			GainRep = new VideoCaptureDeviceProperty(vcd, VideoProcAmp_Gain);
			GammaRep = new VideoCaptureDeviceProperty(vcd, VideoProcAmp_Gamma);
			HueRep = new VideoCaptureDeviceProperty(vcd, VideoProcAmp_Hue);
			SaturationRep = new VideoCaptureDeviceProperty(vcd, VideoProcAmp_Saturation);
			SharpnessRep = new VideoCaptureDeviceProperty(vcd, VideoProcAmp_Sharpness);
			WhiteBalanceRep = new VideoCaptureDeviceProperty(vcd, VideoProcAmp_WhiteBalance);
			ToStringRep = System::String::Concat(vcd->Desc->Name, " Properties");
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		///// VideoCaptureDeviceProperty
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



		VideoCaptureDeviceProperty::VideoCaptureDeviceProperty(VideoCaptureDevice *vcd, VideoProcAmpProperty directShowProperty)
		{
			VCD = vcd;
			MyVideoProcAmpProperty = directShowProperty;
			long propMin, propMax, propStep, propDefaultValue, propFlags;

			bool success = VCD->GetVideoResources()->GetPropertyRange(MyVideoProcAmpProperty, &propMin, &propMax, &propStep, &propDefaultValue, &propFlags);
			AvailableRep = success;
			MinRep = propMin;
			MaxRep = propMax;
			StepRep = propStep;
			DefaultValueRep = propDefaultValue;
			FlagsRep = propFlags;
		}
		long VideoCaptureDeviceProperty::get_Value()
		{
			if (!AvailableRep) return DefaultValueRep;
			long propValue;
			long flags;
			bool success = VCD->GetVideoResources()->GetPropertyValue(MyVideoProcAmpProperty, &propValue, &flags);
			if (success)
				return propValue;
			else
				return DefaultValueRep;
		}
		void VideoCaptureDeviceProperty::set_Value(long newValue)
		{
			if (!AvailableRep)
				return;
			if (newValue < MinRep) newValue = MinRep;
			if (newValue > MaxRep) newValue = MaxRep;
			VCD->GetVideoResources()->SetPropertyValue(MyVideoProcAmpProperty, newValue, FlagsRep);
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		///// UnmanagedVideoResources
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


		void UnmanagedVideoResources::Error(char *errorMessage)
		{
			mCB.VCD->DisposeAndThrowException(errorMessage);
		}






	}
}