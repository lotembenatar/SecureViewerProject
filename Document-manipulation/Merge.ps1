param([string]$Path, [string]$ImageModulePath)

Add-Type -Assembly System.Drawing

if($Path -eq "") {
    Write-Host "Not Path given"
    return
}

# First load module
if($ImageModulePath -eq "") {
    $ImageModulePath = "$PWD\Image.psm1"
}
Unblock-File $ImageModulePath -Confirm:$false
Import-Module $ImageModulePath

$numOfMergePages = [int]((ls $Path).Count/3)
$imageCounter = 3
foreach($i in (2..$numOfMergePages)) {
    
    $playbutton = [System.Drawing.Image]::FromFile("$Path\"+($imageCounter+1)+".bmp");
    $frame = [System.Drawing.Image]::FromFile("$Path\"+($imageCounter-1)+".bmp");
    $bitmap = [System.Drawing.Bitmap]::new($playbutton.Width,$playbutton.Height+$frame.Height+5)
    $canvas = [System.Drawing.Graphics]::FromImage($bitmap)

    $first = [System.Drawing.Rectangle]::new(0,0,$playbutton.Width,$playbutton.Height)
    $second = [System.Drawing.Rectangle]::new(0,0,$frame.Width,$frame.Height)
    $canvas.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $canvas.DrawImage($frame,$first,$second,[System.Drawing.GraphicsUnit]::Pixel)

    $canvas.DrawImage($playbutton,0,($bitmap.Height ) - ($playbutton.Height ))
    $canvas.Save()
    $bitmap.Save("$Path\"+($imageCounter)+".bmp",[System.Drawing.Imaging.ImageFormat]::Bmp)
    
    $image = Get-Image ("$Path\"+($imageCounter)+".bmp")
    $cropOffset = $image.Height - [int]($image.Width/4*3)
    $image.Crop(0,[int]($cropOffset/2),0,[int]($cropOffset/2))
    $image.Resize($Width,$Height,$true)

    $playbutton.Dispose()
    $frame.Dispose()

    $imageCounter += 4
}