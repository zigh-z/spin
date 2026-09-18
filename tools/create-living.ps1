$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot -Parent
$assets=Join-Path $root 'telhado-verde-espin/assets'
Add-Type -Path (Join-Path $PSScriptRoot 'LivingRoof.cs') -ReferencedAssemblies System.Drawing
$scenes=Get-Content (Join-Path $PSScriptRoot 'scenes.json') -Raw -Encoding UTF8 | ConvertFrom-Json
$source=Get-Content (Join-Path $assets 'video-chapters.js') -Raw -Encoding UTF8
$chapters=([regex]::Match($source,'window.spinChapters = (.*?);').Groups[1].Value)|ConvertFrom-Json
foreach($i in 1..10){
 $duration=[double]$chapters[$i].end-[double]$chapters[$i].start
 [LivingRoof]::Render((Join-Path $PSScriptRoot 'ffmpeg.exe'),(Join-Path $assets 'escola-spin.png'),(Join-Path $PSScriptRoot ('living-{0:00}.mp4' -f $i)),[int]$scenes[$i].layer,$scenes[$i].title,$scenes[$i].short,$duration,(Join-Path $PSScriptRoot ('living-{0:00}.png' -f $i)))
 Write-Output "Sequência completa $i/10 renderizada."
}
