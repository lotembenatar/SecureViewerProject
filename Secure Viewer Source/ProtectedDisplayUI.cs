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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;

using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace ProtectedDisplayHost
{
    public partial class Form1 : Form
    {
        private Thread refreshThread;
        byte[] picture;
        bool sessionExists;
		int imageCounter = 0;
		int numOfImages = 0;
		int panelWidthOffset;
		int panelHeightOffset;
		int scrollbarXOffset;
		int scrollbarHeightOffset;

		bool windowMinimized;

		string pathToDocument = "C:\\DALsdk\\Samples\\ProtectedDisplaySample\\ProtectedDisplayHost\\ProtectedDisplayHost\\Encrypted";

		public Form1()
        {
            InitializeComponent();
            //load the pre-encrypted image from the disk
            picture = File.ReadAllBytes(pathToDocument+"\\EncryptedImage0");
            sessionExists = false;

			panel.MouseWheel += new System.Windows.Forms.MouseEventHandler(panel_MouseWheel);

			// Panel offset
			panelWidthOffset = this.Width - panel.Width;
			panelHeightOffset = this.Height - panel.Height;

			// Scrollbar offsets
			scrollbarXOffset = this.Width - vScrollBar1.Location.X;
			scrollbarHeightOffset = this.Height - vScrollBar1.Height;

			// Window state
			windowMinimized = false;

			numOfImages = Directory.GetFiles(pathToDocument).Length - 1;

			vScrollBar1.Minimum = 0;
			vScrollBar1.Maximum = numOfImages;
			vScrollBar1.SmallChange = 1;
			vScrollBar1.LargeChange = 1;

			// Start session
			//call the show function with the encrypted image and the window handle.
			if (ProtectedDisplayWrapper.showImage(picture, panel.Handle))
			{
				sessionExists = true;
				//start a refresh thread that refreshes the image peridically
				refreshThread = new Thread(new ThreadStart(refresh));
				refreshThread.Start();
			}
			else
			{
				MessageBox.Show("Failed to show image. " + ProtectedDisplayWrapper.getError());
			}
		}

		private void panel_MouseWheel(object sender, MouseEventArgs e)
		{
			if (e.Delta < 0)
			{
				// Down
				if (vScrollBar1.Value < numOfImages)
				{
					vScrollBar1.Value++;
				}
			}
			else
			{
				// Up
				if (vScrollBar1.Value > 0)
				{
					vScrollBar1.Value--;
				}
			}
		}

        private void refresh()
        {
            while (true)
            {
                //refresh the image peridically
                ProtectedDisplayWrapper.refresh();
                Thread.Sleep(10);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
            if (refreshThread != null)
                refreshThread.Abort();
            ProtectedDisplayWrapper.Close();
            Application.Exit();
        }

		private void Form1_Resize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
			{
				refreshThread.Abort();
				windowMinimized = true;
				sessionExists = false;
			}
			if (WindowState == FormWindowState.Normal && windowMinimized)
			{
				if (ProtectedDisplayWrapper.showImage(picture, panel.Handle))
				{
					sessionExists = true;
					//start a refresh thread that refreshes the image peridically
					refreshThread = new Thread(new ThreadStart(refresh));
					refreshThread.Start();
				}
				windowMinimized = false;
			}
			panel.Width = this.Width - panelWidthOffset;
			panel.Height = this.Height - panelHeightOffset;
			vScrollBar1.Height = panel.Height;
			vScrollBar1.Location = new Point(this.Width - scrollbarXOffset, vScrollBar1.Location.Y);
		}

		private void vScrollBar1_ValueChanged(object sender, EventArgs e)
		{
			if (sessionExists)
			{
				imageCounter = vScrollBar1.Value;
				picture = File.ReadAllBytes(pathToDocument + "\\EncryptedImage" + imageCounter);
				if (refreshThread != null)
					refreshThread.Abort();
				//ProtectedDisplayWrapper.closePavpSession();
				sessionExists = false;
				//call the show function with the encrypted image and the window handle.
				if (ProtectedDisplayWrapper.showImage(picture, panel.Handle))
				{
					sessionExists = true;
					//start a refresh thread that refreshes the image peridically
					refreshThread = new Thread(new ThreadStart(refresh));
					refreshThread.Start();
				}
				else
				{
					MessageBox.Show("Failed to show image. " + ProtectedDisplayWrapper.getError());
				}
			}
		}
	}
}