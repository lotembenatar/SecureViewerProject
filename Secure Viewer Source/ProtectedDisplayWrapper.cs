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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ProtectedDisplayHost
{
    public class ProtectedDisplayWrapper
    {
        [DllImport("protecteddisplaylibrary", EntryPoint = "refresh", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool refresh();

        [DllImport("protecteddisplaylibrary", EntryPoint = "close", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Close();

        [DllImport("protecteddisplaylibrary", EntryPoint = "closePavpSession", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool closePavpSession();

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("protecteddisplaylibrary", EntryPoint = "showImage", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool showImage(IntPtr picture, Int32 pictureSize, IntPtr targetControl);

        [DllImport("protecteddisplaylibrary", EntryPoint = "getError", CallingConvention = CallingConvention.Cdecl)]
        public static extern string getError();

        public static bool showImage(byte[] encryptedImage, IntPtr targetControl)
        {
            IntPtr bitstrPtr = Marshal.AllocHGlobal(encryptedImage.Length);
            Marshal.Copy(encryptedImage, 0, bitstrPtr, encryptedImage.Length);

            bool ret = showImage(bitstrPtr, encryptedImage.Length, targetControl);

            Marshal.FreeHGlobal(bitstrPtr);

            return ret;
        }
    }
}

