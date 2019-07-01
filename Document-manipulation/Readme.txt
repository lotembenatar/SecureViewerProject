This code will run by the server.

1. The server gets the document in PDF format.
2. The server extract n pages from the document as bitmaps.
3. The server split the images into kn smaller images at 4:3 ratio in 960,720 resulotion

How to use:

Open PowerShell and run:
.\Document-manipulation.ps1 [[-DocumentPath] <string>]

What should be installed to run it?

Python3
Poppler installed and set in the system PATH

