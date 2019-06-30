/**
***
*** Copyright  (C) 1985-2015 Intel Corporation. All rights reserved.
***
*** The information and source code contained herein is the exclusive
*** property of Intel Corporation. and may not be disclosed, examined
*** or reproduced in whole or in part without explicit written authorization
*** from the company.
***
*** ----------------------------------------------------------------------------
**/

/*
Prior to Visual Studio 2015 Update 3, the delay load function hooks were non - const.
They were made const to improve security.
For backwards compatibility with previous Visual Studio versions, we define
the macro DELAYIMP_INSECURE_WRITABLE_HOOKS prior to including DelayImp header
(See details in the DelayImp header file before __pfnDliNotifyHook2 declaration).
*/
#define DELAYIMP_INSECURE_WRITABLE_HOOKS

#include "ProtectedDisplay.h"
#include <fstream>

//Prototype for the delay load function
FARPROC WINAPI delayHook(unsigned dliNotify, PDelayLoadInfo pdli);
PfnDliHook __pfnDliNotifyHook2 = delayHook;

ProtectedDisplay::ProtectedDisplay(void)
{
	try
	{
		errorMessage="";
		errorCode=-1;

		initialized=true;

		//This is the path to the Intel DAL trusted application that was created in Eclipse.
		taPath=getenv("DALSDK");
		taPath.append("\\Samples\\ProtectedDisplaySample\\ProtectedDisplayTA\\bin\\ProtectedDisplaySample_PID_WW25_Prod.dalp");
		//This is the UUID of this trusted application.
		//This is the same value as the UUID field in the Intel DAL trusted application manifest.
		taId = "f43b8192e90940e5b61a8ab26978ddb2";

		encryptedBitmap=NULL;

		//Initialize the JHI
		//Note: this is the first call to JHI function.
		//JHI.dll will now be delayed -load
		//The function delayHook will be called and will perform the load from a trusted path with signature verification
		JHI_RET ret = JHI_Initialize(&handle, NULL, 0);
		if(ret != JHI_SUCCESS)
		{
			errorMessage = "JHI could not be initialized.";
			errorCode = ret;
			initialized=false;
			return;
		}

		wstring ws(taPath.begin(),taPath.end());
		//Install the Intel DAL trusted application
		ret=JHI_Install2(handle,taId.c_str(),ws.c_str());	
		if(ret != JHI_SUCCESS)
		{
			errorMessage = "Failed to install applet.";
			errorCode = ret;
			initialized=false;
			return;
		}
		ret= JHI_CreateSession(handle, taId.c_str(), 0, NULL, &session);
		if(ret != JHI_SUCCESS)
		{	
			errorMessage = "Failed to create session.";
			errorCode = ret;
			initialized=false;
			return;
		}
	}
	catch(std::exception& e)
	{
		errorMessage = strdup(e.what());
		errorCode = -2;
		initialized=false;
		return;
	}
}

ProtectedDisplay* ProtectedDisplay::Session()
{
	static ProtectedDisplay instance;
	return &instance;
}

bool ProtectedDisplay::refresh()
{
	return handler.DisplayVideo()==0;
}

bool ProtectedDisplay::showImage(UINT8* picture,UINT32 pictureSize, HWND targetControl)
{
	if(!initialized)
		return false;
	if (!isKeyGenerated) {
		UINT32 width = 960;
		UINT32 height = 720;

		handler.SetControlSize(width,height);
		//initialize pavp with the window handle
		handler.Initialize(targetControl);

		//get ME handle from pavp,
		handler.GetMEhandle(&ME_handle);

		INT32 responseCode;
		JVM_COMM_BUFFER commBuf;
		UINT8 PAVPKeyRequest[4];

		//copy the ME handle to the buffer
		PAVPKeyRequest[0] = (byte)(ME_handle >> 3 * 8);
		PAVPKeyRequest[1] = (byte)((ME_handle << 1 * 8) >> 3 * 8);
		PAVPKeyRequest[2] = (byte)((ME_handle << 2 * 8) >> 3 * 8);
		PAVPKeyRequest[3] = (byte)((ME_handle << 3 * 8) >> 3 * 8);

		//init the JHI call parameters
		//perform call to the Trusted Application to get PAVP key
		byte pavpKey[16];
		commBuf.TxBuf->buffer = PAVPKeyRequest;
		commBuf.TxBuf->length = 4;
		commBuf.RxBuf->buffer = pavpKey;
		commBuf.RxBuf->length = 16;
		JHI_RET ret = JHI_SendAndRecv2(handle, session, CMD_GENERATE_PAVP_SESSION_KEY, &commBuf, &responseCode);
		if (ret != JHI_SUCCESS || responseCode != 0)
		{
			errorMessage = "Failed to perform send and receive operation.";
			errorCode = ret;
			return false;
		}

		//set the new key we got from the Trusted Application
		handler.SetNewKey(pavpKey);
		key = pavpKey;
		isKeyGenerated = true;
	}

	//allocate buffer for the encrypted bitmap
	encryptedBitmap=new UINT8[pictureSize];
	//copy the encrypted bitmap buffer from the server data
	memcpy(encryptedBitmap,picture,pictureSize);

	//call the show function to display the video
	return handler.ShowImage(encryptedBitmap,pictureSize,targetControl);
}

char* ProtectedDisplay::getError()
{
	char* message=new char[strlen(errorMessage)*2];
	if(strcmp(errorMessage,"")==0)
		return "";
	sprintf(message,"%s Error code: 0x%x",errorMessage,errorCode);
	return message;
}

bool ProtectedDisplay::closePavpSession()
{
	bool res=true;
	//close PAVP session
	if(!handler.ClosePAVPSession())
		res= false;
	return res;
}

bool ProtectedDisplay::close()
{
	bool res=true;
	if(initialized && handle!=NULL)
	{
		//close PAVP session
		if(!handler.ClosePAVPSession())
			res= false;
		//Close Trusted Application session
		if(session!=NULL)
		{
			JHI_RET ret = JHI_CloseSession(handle, &session);
			if(ret != JHI_SUCCESS)
			{
				errorMessage = "Failed to close session.";
				errorCode = ret;
				res= false;
			}
			else
			{
				//Uninstall the Trusted Application
				ret = JHI_Uninstall(handle,const_cast<char*>(taId.c_str()));
				if(ret != JHI_SUCCESS)
				{
					errorMessage = "Failed to uninstall JHI.";
					errorCode = ret;
					res= false;
				}
				else
				{
					//Deinit the JHI handle
					ret=JHI_Deinit(handle);
					if(ret != JHI_SUCCESS)
					{
						errorMessage = "JHI could not be deinitialized.";
						errorCode = ret;
						res= false;
					}
				}
			}
		}
		if(encryptedBitmap!=NULL)
			delete[] encryptedBitmap;

		initialized=false;
	}

	return res;
}

ProtectedDisplay::~ProtectedDisplay(void)
{
	
}
