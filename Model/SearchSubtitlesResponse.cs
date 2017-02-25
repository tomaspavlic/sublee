﻿using System.Runtime.Serialization;

namespace OpenSubtitles.Model
{
    public class SearchSubtitlesResponse
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "data")]
        public SearchSubtitles[] Data { get; set; }
    }
}
