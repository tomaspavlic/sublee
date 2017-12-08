# sublee
[![Build status](https://img.shields.io/travis/tomaspavlic/sublee.svg)](https://github.com/tomaspavlic/sublee)

Console application for downloading and searching subtitles from OpenSubtitles.org

## Install

[![Releases](https://img.shields.io/github/downloads/tomaspavlic/sublee/total.svg)][Releases]

Standalone **executables** for _Linux_, _macOS_ and _Windows_ are provided in
the [Releases] section.
Download an archive for your operating system and unpack the content to a place
accessible from command line. The sublee is ready to go.

## usage:

#### search subtitles using moviehash method
`sublee /path/to/some/movie --method moviehash --language eng`

#### search subtitles using query method
`sublee "Game Of Thrones s01e01" --method query --language eng`

##### example:
sublee --help

##### output:
```
Usage: sublee [arguments] [options]

Arguments:
  search  Search value depends on search method. moviehash <path>, query <text>, imdb <id>, tag <text>

Options:
  -m|--method <method>  Search method moviehash, query, tag or imdb.
  -o|--output <path>    Path for output subtitles filename default is original substitles name.
  -l|--language <lang>  Language of subtitles default is english (eng). ISO 639-2/B
  -1|--first            Download first subtitles without user input.
  -v|--verbose          Be verbose.
  -?|-h|--help          Show help information
```

##### Prompt
If you are not using option `-1|--first` you will be prompted for subtitles selection

![Prompt](../master/prompt.gif)

[Releases]: https://github.com/tomaspavlic/sublee/releases