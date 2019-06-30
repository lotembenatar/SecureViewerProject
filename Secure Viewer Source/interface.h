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
#include <Windows.h>
#include <d3d9.h>

#ifdef __cplusplus
extern "C" {
#endif

#define PAVP_EXPORT __declspec(dllexport) bool
#define EXTERN_DLL_EXPORT __declspec(dllexport) char*

EXTERN_DLL_EXPORT getError();

//basic function: first image display - params: encrypted image, encrypted image size, handler to the target window
PAVP_EXPORT showImage(UINT8* picture, UINT32 pictureSize, HWND targetControl);

//Refresh. Must be called peridically.
PAVP_EXPORT refresh();

//de-init PAVP and JHI
PAVP_EXPORT close();

//de-init PAVP
PAVP_EXPORT closePavpSession();

#ifdef __cplusplus
};
#endif
