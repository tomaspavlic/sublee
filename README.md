# sublee
Console application for downloading and searching subtitles from OpenSubtitles.org


### usage:

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
