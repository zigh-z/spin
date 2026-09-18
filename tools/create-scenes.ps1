param([switch]$Narrate)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$out = Join-Path $root 'telhado-verde-espin/assets'
$scenes = Get-Content (Join-Path $PSScriptRoot 'scenes.json') -Raw -Encoding UTF8 | ConvertFrom-Json
if ($Narrate) {
 Add-Type -AssemblyName System.Speech
 $voice = New-Object System.Speech.Synthesis.SpeechSynthesizer
 $voice.SelectVoice('Microsoft Maria Desktop')
 $voice.Rate = 0
 for ($i=0; $i -lt $scenes.Count; $i++) {
  $voice.SetOutputToWaveFile((Join-Path $out ('voice-{0:00}.wav' -f $i)))
  $voice.Speak($scenes[$i].text)
  $voice.SetOutputToNull()
 }
 $voice.Dispose()
 Write-Output 'Narração gerada: 11 cenas em português.'
 exit
}
Add-Type -AssemblyName System.Drawing
$photo = [Drawing.Image]::FromFile((Join-Path $out 'escola-spin.png'))
$labels = @('VEGETAÇÃO','SUBSTRATO','FILTRO','DRENAGEM','PROTEÇÃO DE RAÍZES','IMPERMEABILIZAÇÃO','ISOLANTE TÉRMICO','CONTRA UMIDADE','LAJE / ESTRUTURA')
$colors = @('#71af3b','#a67545','#d7cdb1','#459eae','#5c776c','#33414f','#e4c865','#86654f','#a4acac')
function Brush($hex) { New-Object Drawing.SolidBrush ([Drawing.ColorTranslator]::FromHtml($hex)) }
function Text($value,$size,$x,$y,$width,$height,$color,$bold=$false) {
 $style = [Drawing.FontStyle]::Regular
 if ($bold) { $style = [Drawing.FontStyle]::Bold }
 $font = New-Object Drawing.Font('Segoe UI',$size,$style,[Drawing.GraphicsUnit]::Pixel)
 $brush = Brush $color
 $rect = New-Object Drawing.RectangleF($x,$y,$width,$height)
 $g.DrawString($value,$font,$brush,$rect)
 $font.Dispose(); $brush.Dispose()
}
for ($s=0; $s -lt $scenes.Count; $s++) {
 $scene=$scenes[$s]
 $bmp=New-Object Drawing.Bitmap(1280,720)
 $g=[Drawing.Graphics]::FromImage($bmp)
 $g.SmoothingMode='AntiAlias'; $g.TextRenderingHint='AntiAliasGridFit'
 $g.DrawImage($photo,0,0,1280,742)
 $shade=New-Object Drawing.SolidBrush ([Drawing.Color]::FromArgb(90,9,35,28))
 $g.FillRectangle($shade,0,0,1280,720);$shade.Dispose()
 if ($scene.layer -ge 0) {
  $panel=New-Object Drawing.SolidBrush ([Drawing.Color]::FromArgb(240,249,249,232))
  $g.FillRectangle($panel,36,100,572,443);$panel.Dispose()
  Text $scene.label 16 60 126 520 40 '#357448' $true
  Text $scene.title 39 60 187 520 108 '#164632' $true
  Text $scene.short 26 60 321 490 120 '#3f5e49'
  Text ('CAMADA {0} DE 9   /   DO TOPO À LAJE' -f ($scene.layer+1)) 14 60 495 500 26 '#567653' $true
  $panel=New-Object Drawing.SolidBrush ([Drawing.Color]::FromArgb(200,12,42,33))
  $g.FillRectangle($panel,649,83,600,490);$panel.Dispose()
  Text 'CORTE ILUSTRATIVO SOBRE A ESCOLA' 14 680 98 540 25 '#eff5d8' $true
  for ($j=8; $j -ge 0; $j--) {
   $x=690; $y=149+$j*43
   if ($j -eq $scene.layer) {$x=668}
   $points=[Drawing.Point[]]@([Drawing.Point]::new($x,$y+8),[Drawing.Point]::new($x+45,$y),[Drawing.Point]::new($x+480,$y),[Drawing.Point]::new($x+453,$y+30),[Drawing.Point]::new($x,$y+30))
   $brush=Brush $colors[$j]; $g.FillPolygon($brush,$points);$brush.Dispose()
   if ($j -eq $scene.layer) { $pen=New-Object Drawing.Pen([Drawing.Color]::White,3);$g.DrawPolygon($pen,$points);$pen.Dispose() }
   $textColor='#102c28';if ($j -eq 4 -or $j -eq 5 -or $j -eq 7) {$textColor='#ffffff'}
   Text ('{0:00}   {1}' -f ($j+1),$labels[$j]) 16 ($x+48) ($y+3) 400 27 $textColor $true
  }
  Text 'SPIN EDUCACIONAL  /  PROPOSTA CONCEITUAL' 13 681 548 550 22 '#d2eac4'
 } else {
  $panel=New-Object Drawing.SolidBrush ([Drawing.Color]::FromArgb(225,15,57,39))
  $g.FillRectangle($panel,35,402,1210,206);$panel.Dispose()
  Text $scene.label 18 66 420 1120 32 '#ceec8f' $true
  Text $scene.title 51 62 464 1150 76 '#ffffff' $true
  Text $scene.short 26 66 546 1110 55 '#e3f3d1'
 }
 $bottom=Brush '#123e2e';$g.FillRectangle($bottom,0,652,1280,68);$bottom.Dispose()
 Text 'TELHADO VERDE  /  ESCOLA SPIN' 17 35 674 680 32 '#ecf5dc' $true
 Text ('{0:00} / 11' -f ($s+1)) 17 1165 674 100 32 '#c6e58c' $true
 $bmp.Save((Join-Path $out ('scene-{0:00}.png' -f $s)),[Drawing.Imaging.ImageFormat]::Png)
 $g.Dispose();$bmp.Dispose()
}
$photo.Dispose()
Write-Output '11 cenas ilustradas geradas.'
