using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TdLib
{
    /// <summary>
    /// Autogenerated TDLib APIs
    /// </summary>
    public static partial class TdApi
    {
        /// <summary>
        /// Sends a custom request; for bots only 
        /// </summary>
        public class SendCustomRequest : Function<CustomRequestResult>
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "sendCustomRequest";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// The method name 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("method")]
            public string Method { get; set; }

            /// <summary>
            /// JSON-serialized method parameters
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("parameters")]
            public string Parameters { get; set; }
        }


        /// <summary>
        /// Sends a custom request; for bots only 
        /// </summary>
        public static Task<CustomRequestResult> SendCustomRequestAsync(this Client client,
            string method = default(string),
            string parameters = default(string))
        {
            return client.ExecuteAsync(new SendCustomRequest
            {
                Method = method,
                Parameters = parameters,
            });
        }
    }
}