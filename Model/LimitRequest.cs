using System.Runtime.Serialization;

namespace OpenSubtitles.Model
{
    [DataContract]
    public class LimitRequest
    {
        [DataMember(Name = "limit")]
        public int Limit { get; set; }
    }
}
