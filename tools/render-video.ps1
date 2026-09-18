$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot -Parent
$out=Join-Path $root 'telhado-verde-espin/assets'
$ff=Join-Path $PSScriptRoot 'ffmpeg.exe'
$probe=Join-Path $PSScriptRoot 'ffprobe.exe'
$scenes=Get-Content (Join-Path $PSScriptRoot 'scenes.json') -Raw -Encoding UTF8 | ConvertFrom-Json
$chapters=@();$concat=@();$vtt="WEBVTT`n`n";$elapsed=0.0
function Stamp([double]$seconds) { [TimeSpan]::FromSeconds($seconds).ToString('hh\:mm\:ss\.fff') }
for($i=0;$i -lt $scenes.Count;$i++) {
 $wav=Join-Path $out ('voice-{0:00}.wav' -f $i)
 $png=Join-Path $out ('scene-{0:00}.png' -f $i)
 $mp4=Join-Path $PSScriptRoot ('clip-{0:00}.mp4' -f $i)
 $raw=& $probe -v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 $wav
 $duration=[Math]::Ceiling(([double]::Parse($raw,[Globalization.CultureInfo]::InvariantCulture)+0.7)*25)/25
 $durationString=$duration.ToString('0.00',[Globalization.CultureInfo]::InvariantCulture)
 $fadeOut=($duration-.45).ToString('0.00',[Globalization.CultureInfo]::InvariantCulture)
 $filter="zoompan=z='min(zoom+0.000035,1.025)':x='iw/2-iw/zoom/2':y='ih/2-ih/zoom/2':d=1:s=1280x720:fps=25,fade=t=in:st=0:d=0.35,fade=t=out:st=${fadeOut}:d=0.4,format=yuv420p"
 & $ff -hide_banner -loglevel error -y -loop 1 -framerate 25 -i $png -i $wav -vf $filter -af 'apad,volume=1.3' -t $durationString -c:v libx264 -preset ultrafast -crf 24 -threads 2 -c:a aac -b:a 128k -ar 48000 -movflags +faststart $mp4
 if($LASTEXITCODE -ne 0){throw "Falha na cena $i"}
 $chapters+=@{title=$scenes[$i].title;text=$scenes[$i].text;start=[Math]::Round($elapsed,2);end=[Math]::Round($elapsed+$duration,2)}
 $sentences=[regex]::Split($scenes[$i].text,'(?<=[.!?])\s+')
 $offset=$elapsed
 $length=($sentences | ForEach-Object {$_.Length} | Measure-Object -Sum).Sum
 foreach($sentence in $sentences){
  $end=$offset+($duration-.3)*$sentence.Length/$length
  $vtt+="$(Stamp $offset) --> $(Stamp $end)`n$sentence`n`n"
  $offset=$end
 }
 $elapsed+=$duration
 $concat+="file 'clip-$('{0:00}' -f $i).mp4'"
 Write-Output "Cena $($i+1)/11 pronta ($durationString s)."
}
[IO.File]::WriteAllLines((Join-Path $PSScriptRoot 'concat.txt'),$concat,[Text.UTF8Encoding]::new($false))
[IO.File]::WriteAllText((Join-Path $out 'telhado-verde-spin.vtt'),$vtt,[Text.UTF8Encoding]::new($false))
[IO.File]::WriteAllText((Join-Path $out 'video-chapters.js'),('window.spinChapters = '+(ConvertTo-Json -InputObject $chapters -Depth 4 -Compress)+'; window.spinCaptions = '+(ConvertTo-Json -InputObject $vtt -Compress)+';'),[Text.UTF8Encoding]::new($false))
& $ff -hide_banner -loglevel error -y -f concat -safe 0 -i (Join-Path $PSScriptRoot 'concat.txt') -c copy -movflags +faststart (Join-Path $out 'telhado-verde-spin.mp4')
if($LASTEXITCODE -ne 0){throw 'Falha na montagem final'}
Write-Output "Vídeo final: $elapsed segundos."
