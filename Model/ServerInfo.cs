using System.Runtime.Serialization;

namespace OpenSubtitles.Model
{
    [DataContract]
    public class ServerInfo
    {
        [DataMember(Name = "xmlrpc_version")]
        public string XmlRpcVersion { get; set; }
    }
}
