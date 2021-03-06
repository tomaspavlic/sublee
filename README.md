# sublee
![build](https://github.com/tomaspavlic/sublee/workflows/build/badge.svg)
[![Releases](https://img.shields.io/github/downloads/tomaspavlic/sublee/total.svg)][Releases]

Command line application for sorting media, searching and downloading subtitles from OpenSubtitles.org

## Install

Standalone **executables** for _Linux_, _macOS_ and _Windows_ are provided in
the [Releases] section.
Download an archive for your operating system and unpack the content to a place
accessible from command line. The sublee is ready to go.

### Manual (CLI only)

_dotnet SDK needed_

Runtime catalog for publish for you OS [here](https://docs.microsoft.com/cs-cz/dotnet/core/rid-catalog)

```bash
git clone https://github.com/tomaspavlic/sublee
cd sublee/src
dotnet publish --runtime osx-x64 --framework netcoreapp3.0 --self-contained -o /opt/sublee
ln -s /opt/sublee/sublee /usr/local/bin/sublee
```

## usage:

#### search subtitles using moviehash method
`sublee search /path/to/some/movie --method moviehash --language eng`

#### search subtitles using query method
`sublee search "Game Of Thrones s01e01" --method query --language eng`

#### rename media file
`sublee rename ~/Downloads /Volumes/DATA -v -r`

#### rename media file
`sublee info /path/to/some/movie`

##### example:
sublee --help

##### output:
```
Command line application for sorting media, searching and downloading subtitles from OpenSubtitles.org

Usage: sublee [options] [command]

Options:
  -?|-h|--help  Show help information

Commands:
  rename        Recognize a media file and move it to new location
  search        Search subtitles for given media file
  info          Show info about media file

Run 'sublee [command] --help' for more information about a command.
```

##### Prompt
If you are not using option `-1|--first` you will be prompted for subtitles selection

![Prompt](../master/prompt.gif)

[Releases]: https://github.com/tomaspavlic/sublee/releases


##### Windows Installer
You can find in [Releases] section also windows installer (msi). It installs application into program files and add this into your path environment. You can start using sublee in commnad line immediatelly. It adds an entry in the Windows Explorer context menu 'Download Subtitles'.

![ContextMenu](../master/context_menu.jpg)