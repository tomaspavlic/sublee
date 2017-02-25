using System.Runtime.Serialization;

namespace OpenSubtitles.Model
{
    [DataContract]
    public class LogIn
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "seconds")]
        public double Seconds { get; set; }
    }
}
