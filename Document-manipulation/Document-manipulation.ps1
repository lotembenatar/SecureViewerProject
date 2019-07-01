param([string]$DocumentPath)

$Width = 960
$Height = 720

$pathImages = "$PWD\Images"
$pathCroppedImages = "$PWD\CroppedImages"

sal PYTHON3 "C:\Program Files (x86)\Microsoft Visual Studio\Shared\Anaconda3_64\python.exe"

if(($DocumentPath -eq "")) {
    Write-Host "Usage: Document-manipulation.ps1 [[-DocumentPath] <string>]"
    return
}

# First load module
Unblock-File "$PWD\Image.psm1" -Confirm:$false
Import-Module "$PWD\Image.psm1"

# Remove files
Remove-Item $pathImages\*
Remove-Item $pathCroppedImages\*

# Split Document to images
PYTHON3 "$PWD\Convert-pdf2image.py" $DocumentPath
$numOfPages = (ls $pathImages).Count - 1

# Split Images to smaller images
$imageCroppedCounter = 0
foreach($imageCounter in (0..$numOfPages)) {
    $imagePath = "$pathImages\$imageCounter.bmp"

    # First Image
    cp $imagePath $pathCroppedImages\$imageCroppedCounter.bmp

    $image = Get-Image $pathCroppedImages\$imageCroppedCounter.bmp
    #$image.Crop(3,0,3,0)
    $cropOffset = $image.Height - [int]($image.Width/4*3)
    $image.Crop(0,0,0,$cropOffset)
    $image.Resize($Width,$Height,$true)

    $imageCroppedCounter++
    # Second Image
    cp $imagePath $pathCroppedImages\$imageCroppedCounter.bmp

    $image = Get-Image $pathCroppedImages\$imageCroppedCounter.bmp
    #$image.Crop(3,0,3,0)
    $cropOffset = $image.Height - [int]($image.Width/4*3)
    $image.Crop(0,[int]($cropOffset/2),0,[int]($cropOffset/2))
    $image.Resize($Width,$Height,$true)

    $imageCroppedCounter++
    # Third Image
    cp $imagePath $pathCroppedImages\$imageCroppedCounter.bmp

    $image = Get-Image $pathCroppedImages\$imageCroppedCounter.bmp
    #$image.Crop(3,0,3,0)
    $cropOffset = $image.Height - [int]($image.Width/4*3)
    $image.Crop(0,$cropOffset,0,0)
    $image.Resize($Width,$Height,$true)

    # For Next first
    $imageCroppedCounter += 2
}

.\Merge.ps1 $pathCroppedImages