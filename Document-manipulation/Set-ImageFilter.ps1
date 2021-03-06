#requires -version 2.0
function Set-ImageFilter {
    <#
        .Synopsis
            Applies an image filter to one or more images
        .Description
            Applies an Windows Image Acquisition filter to one or more Windows Image Acquisition images
        .Example
            $image = Get-Image .\Try.jpg            
            $image = $image | Set-ImageFilter -filter (Add-RotateFlipFilter -flipHorizontal -passThru) -passThru                    
            $image.SaveFile("$pwd\Try2.jpg")
        .Parameter image
            The image or images the filter will be applied to
        .Parameter passThru
            If set, the image or images will be emitted onto the pipeline       
        .Parameter filter
            One or more Windows Image Acquisition filters to apply to the image
    #>
    param(
    [Parameter(ValueFromPipeline=$true)]
    $image,
    
    [__ComObject[]]
    $filter,
    
    [switch]
    $passThru
    )
    
    process {
        if (-not $image.LoadFile) { return }
        $i = $image
        foreach ($f in $filter) {
            $i = $f.Apply($i.PSObject.BaseObject)
        }       
        if ($passThru) { 
            $i
        }
    }
}