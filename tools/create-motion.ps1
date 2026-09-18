$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot -Parent
$assets=Join-Path $root 'telhado-verde-espin/assets'
Add-Type -Path (Join-Path $PSScriptRoot 'RoofMovie.cs') -ReferencedAssemblies System.Drawing
$scenes=Get-Content (Join-Path $PSScriptRoot 'scenes.json') -Raw -Encoding UTF8 | ConvertFrom-Json
foreach($i in 1..10){
 [RoofMovie]::Render((Join-Path $PSScriptRoot 'ffmpeg.exe'),(Join-Path $assets 'escola-spin.png'),(Join-Path $PSScriptRoot ('motion-{0:00}.mp4' -f $i)),[int]$scenes[$i].layer,$scenes[$i].title,$scenes[$i].short,(Join-Path $PSScriptRoot ('motion-{0:00}.png' -f $i)))
 Write-Output "Animação $i/10 renderizada."
}
