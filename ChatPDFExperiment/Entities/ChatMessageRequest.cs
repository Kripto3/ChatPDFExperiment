using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatPDFExperiment.Entities
{
    [Serializable]
    public class ChatMessageRequest
    {
        [JsonProperty("sourceId")]
        public string SourceId { get; set; }
        [JsonProperty("messages")]
        public List<ChatMessage> Messages { get; set; }
    }
}
