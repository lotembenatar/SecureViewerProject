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
#pragma once
#include <windows.h>
#include <stdio.h>
#include <iostream>
#include <jhi.h>
#include <DelayImp.h>
#include "PavpHandler.h"

using namespace std;

static int CMD_GENERATE_PAVP_SESSION_KEY	= 1;

class ProtectedDisplay
{
public:
	static ProtectedDisplay* Session();
	bool showImage(UINT8* picture,UINT32 pictureSize, HWND targetControl);
	char* getError();	
	bool refresh();
	bool close();
	bool closePavpSession();
private:
	ProtectedDisplay(void);
private:
	~ProtectedDisplay(void);

	//This is the path to the Intel DAL trusted application that was created in Eclipse.
	string taPath;
	//This is the UUID of this trusted application.
	//This is the same value as the UUID field in the Intel DAL trusted application manifest.
	string taId;

	//flag indicating whther the sample was initialized successfully or not
	bool initialized;

	//error specifies which error was returned from JHI API
	char* errorMessage;
	int errorCode;

	//JHI handlers
	JHI_HANDLE handle;
	JHI_SESSION_HANDLE session;

	//encrypted bitmap we display
	UINT8* encryptedBitmap;

	//Instance of PavpHandler
	PavpHandler handler;

	// ME handle
	UINT ME_handle = 0;

	// A flag that represents if the key has already been created
	bool isKeyGenerated = false;

	// The pavp key got from the DAL applet
	UINT8* key;
};

