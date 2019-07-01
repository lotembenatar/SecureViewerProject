from pdf2image import convert_from_path
import os
import os.path
import sys

pdf_path = sys.argv[1]
save_images_path = os.getcwd() + "\\Images\\"

# We should get this document from the creator
# Document should be parsed into a series of images
documentImages = convert_from_path(pdf_path)

# Save all pages as images (JPEG format)
numOfImage = 0
for page in documentImages:
	page.save((save_images_path+str(numOfImage)+".bmp"),"BMP")
	numOfImage += 1

