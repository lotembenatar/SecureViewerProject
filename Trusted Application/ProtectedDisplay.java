/**
***
*** Copyright  (C) 1985-2014 Intel Corporation. All rights reserved.
***
*** The information and source code contained herein is the exclusive
*** property of Intel Corporation. and may not be disclosed, examined
*** or reproduced in whole or in part without explicit written authorization
*** from the company.
***
*** ----------------------------------------------------------------------------
**/
package com.intel.samples.display;

import com.intel.langutil.TypeConverter;
import com.intel.ui.ProtectedOutput;
import com.intel.util.DebugPrint;
import com.intel.util.IntelApplet;

//
// Implementation of DAL Trusted Application: ProtectedOutputApplet 
//
// *************************************************
// NOTE:  This Trusted Application is intended for API Level 2 and above
// *************************************************

public class ProtectedDisplay extends IntelApplet {
		
	private static final int CMD_GENERATE_PAVP_SESSION_KEY	= 1; 
	
	/*
	 * This method will be called by the VM when a new session is opened to the Trusted Application 
	 * and this Trusted Application instance is being created to handle the new session.
	 * This method cannot provide response data and therefore calling
	 * setResponse or setResponseCode methods from it will throw a NullPointerException.
	 * 
	 * @param	request	the input data sent to the Trusted Application during session creation
	 * 
	 * @return	APPLET_SUCCESS if the operation was processed successfully, 
	 * any other error status code otherwise (note that all error codes will be
	 * treated similarly by the VM by sending "cancel" error code to the SW application).
	 */
	public int onInit(byte[] request) {
		DebugPrint.printString("Protected Display TA: onInit");
		return APPLET_SUCCESS;
	}
	/*
	 * This method will be called by the VM to handle a command sent to this
	 * Trusted Application instance.
	 * 
	 * @param	commandId	the command ID (Trusted Application specific) 
	 * @param	commandData		the input data for this command 
	 * @return	the return value should not be used by the applet
	 */
	public int invokeCommand(int commandId, byte[] commandData) {
		DebugPrint.printString("Protected TA: invokeCommand");
		int res;
		switch (commandId)
		{		
			case CMD_GENERATE_PAVP_SESSION_KEY:
				res=generatePavpKey(commandData);
				break;
							
		   	default:
		   		DebugPrint.printString("Invalid command received!");
		   		res= IntelApplet.APPLET_ERROR_BAD_PARAMETERS;
				break;
	   }	
	   setResponseCode(res);
	   
	   /*
	   * The return value of the invokeCommand method is not guaranteed to be
	   * delivered to the SW application, and therefore should not be used for
	   * this purpose. Trusted Application is expected to return APPLET_SUCCESS code
	   * from this method and use the setResposeCode method instead.
	   */
	   return APPLET_SUCCESS;
	}		
	
	private int generatePavpKey(byte[] commandData)
	{
		int status = IntelApplet.APPLET_ERROR_GENERIC;
		DebugPrint.printString("Generating PAVP key...");
		
		try
		{
			// get the Intel ME Handle
			int slotHandle = TypeConverter.bytesToInt(commandData, 0);
			
			//hard coded symmetric key that the image was encrypted with
			byte[] symetricKey={ (byte) 0xa8,(byte) 0xda,(byte) 0xc6,0x2f,0x1c,0x25,(byte) 0x81,(byte) 0xe2,(byte) 0xdf,0x45,(byte) 0x8e,0x21,0x67,0x3e,0x55,0x30};
			
			// Create the session key
			ProtectedOutput pOutput = ProtectedOutput.getInstance(slotHandle, symetricKey, (short)0, (short)16);

			byte response [] = new byte[16];
			
			//retrieve encrypted key record to be provided to GFX driver for key injection
			pOutput.getEncryptedKeyRecord(response, (short)0);
			
			//return the PAVP key to the host
			setResponse(response, 0, response.length);
			
			DebugPrint.printString("Generating PAVP Session key succeeded!");
			status = IntelApplet.APPLET_SUCCESS;
		}
		catch (Exception ex)
		{
			DebugPrint.printString("Error: failed to generate PAVP Session key\n"+ex.getMessage());
		}
		return status;
	}
	
	/*
	 * This method will be called by the VM when the session being handled by
	 * this Trusted Application instance is being closed 
	 * and this Trusted Application instance is about to be removed.
	 * This method cannot provide response data and therefore
	 * calling setResponse or setResponseCode methods from it will throw a NullPointerException.
	 * 
	 * @return APPLET_SUCCESS code (the status code is not used by the VM).
	 */
	public int onClose() {
		DebugPrint.printString("Protected TA: onClose");
		return APPLET_SUCCESS;
	}
}
