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
#include "interface.h"
#include "PavpHandler.h"
#include "ProtectedDisplay.h"
#include <iostream>

#include "crypt_data_gen.h"
#include "sigma_crypto_utils.h"

int SigmaCryptoUtils::current_gid;

bool showImage(UINT8* picture,UINT32 pictureSize, HWND targetControl)
{
	return ProtectedDisplay::Session()->showImage(picture,pictureSize,targetControl);
}

char* getError()
{
	return ProtectedDisplay::Session()->getError();
}

bool refresh()
{
	return ProtectedDisplay::Session()->refresh();
}

bool closePavpSession()
{
	return ProtectedDisplay::Session()->closePavpSession();
}

bool close()
{
	return ProtectedDisplay::Session()->close();
}