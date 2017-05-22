# sublee

### usage:
`dotnet.exe sublee.dll download "PathToFile" -v`

### example:
`dotnet.exe sublee.dll download "C:\Users\tomas.pavlic\Desktop\Silicon.Valley.S04E05.720p.HDTV.x265.ShAaNiG.mkv" -v`

### output (with argument -v | --verbose):
```
C:\Users\tomas.pavlic\Desktop\sublee\bin\Release\PublishOutput>dotnet.exe sublee.dll download "C:\Users\tomas.pavlic\Desktop\Silicon.Valley.S04E05.720p.HDTV.x265.ShAaNiG.mkv" -v
[2017-05-22 02:35:32][INFO] Logging into OpenSubtitles api.
[2017-05-22 02:35:32][INFO] Searching subtitles for 'C:\Users\tomas.pavlic\Desktop\Silicon.Valley.S04E05.720p.HDTV.x265.ShAaNiG.mkv' using method moviehash.
[2017-05-22 02:35:32][INFO] Subtitles found 'Silicon.Valley.S04E05.720p.HDTV.x265.ShAaNiG.srt'.
[2017-05-22 02:35:32][INFO] Downloading subtitle for 'C:\Users\tomas.pavlic\Desktop\Silicon.Valley.S04E05.720p.HDTV.x265.ShAaNiG.mkv'.
[2017-05-22 02:35:32][INFO] Subtitles successfully downloaded.
```

#### help
use -h argument for more information.

##### example:
dotnet.exe sublee.dll -h

##### output:
```
C:\Users\tomas.pavlic\Desktop\sublee\bin\Release\PublishOutput>dotnet.exe sublee.dll -h

Usage:  [options] [command]

Options:
  -? | -h | --help  Show help information

Commands:
  download
  search

Use " [command] --help" for more information about a command.
```
