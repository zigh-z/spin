$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot -Parent
$assets=Join-Path $root 'telhado-verde-espin/assets'
$ff=Join-Path $PSScriptRoot 'ffmpeg.exe'
$chapterScript=Get-Content (Join-Path $assets 'video-chapters.js') -Raw -Encoding UTF8
$chapters=([regex]::Match($chapterScript,'window.spinChapters = (.*?);').Groups[1].Value) | ConvertFrom-Json
$concat=@()
for($i=0;$i -lt 11;$i++){
 $duration=([double]$chapters[$i].end-[double]$chapters[$i].start)
 $d=$duration.ToString('0.00',[Globalization.CultureInfo]::InvariantCulture)
 $fade=($duration-.3).ToString('0.00',[Globalization.CultureInfo]::InvariantCulture)
 $wav=Join-Path $assets ('voice-{0:00}.wav' -f $i)
 $out=Join-Path $PSScriptRoot ('elaborated-{0:00}.mp4' -f $i)
 if($i -eq 0){
  $ref=Join-Path $PSScriptRoot 'reference-spin.mp4'
  $overview=Join-Path $PSScriptRoot 'living-10.mp4'
  $filter='[0:v]trim=0:4.2,setpts=1.5*(PTS-STARTPTS),scale=1280:720,setsar=1,fps=25,trim=duration=6.28,setpts=PTS-STARTPTS,settb=AVTB[a];[1:v]trim=duration=16.64,setpts=PTS-STARTPTS,setsar=1,settb=AVTB[b];[a][b]xfade=transition=fade:duration=0.6:offset=5.68,fade=t=in:st=0:d=0.35,format=yuv420p[v]'
  & $ff -hide_banner -loglevel error -y -i $ref -i $overview -i $wav -filter_complex $filter -map '[v]' -map 2:a -af 'apad,volume=1.3' -t $d -c:v libx264 -preset veryfast -crf 21 -threads 2 -c:a aac -b:a 128k -ar 48000 -movflags +faststart $out
 }else{
  $motion=Join-Path $PSScriptRoot ('living-{0:00}.mp4' -f $i)
  & $ff -hide_banner -loglevel error -y -i $motion -i $wav -vf "fade=t=in:st=0:d=0.25,fade=t=out:st=${fade}:d=0.25" -af 'apad,volume=1.3' -t $d -c:v libx264 -preset veryfast -crf 21 -threads 2 -c:a aac -b:a 128k -ar 48000 -movflags +faststart $out
 }
 if($LASTEXITCODE -ne 0){throw "Falha no capítulo $i"}
 $concat+="file 'elaborated-$('{0:00}' -f $i).mp4'"
 Write-Output "Capítulo $($i+1)/11 com áudio pronto."
}
[IO.File]::WriteAllLines((Join-Path $PSScriptRoot 'elaborated-concat.txt'),$concat,[Text.UTF8Encoding]::new($false))
& $ff -hide_banner -loglevel error -y -f concat -safe 0 -i (Join-Path $PSScriptRoot 'elaborated-concat.txt') -c copy -movflags +faststart (Join-Path $assets 'telhado-verde-spin-vivo.mp4')
if($LASTEXITCODE -ne 0){throw 'Falha ao montar o vídeo'}
& $ff -hide_banner -loglevel error -y -ss 2 -i (Join-Path $PSScriptRoot 'living-01.mp4') -frames:v 1 (Join-Path $assets 'video-vivo-capa.png')
Write-Output 'Vídeo animado finalizado.'
