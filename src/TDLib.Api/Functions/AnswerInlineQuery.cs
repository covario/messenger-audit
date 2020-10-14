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
        /// Sets the result of an inline query; for bots only 
        /// </summary>
        public class AnswerInlineQuery : Function<Ok>
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "answerInlineQuery";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// Identifier of the inline query 
            /// </summary>
            [JsonConverter(typeof(Converter.Int64))]
            [JsonProperty("inline_query_id")]
            public Int64 InlineQueryId { get; set; }

            /// <summary>
            /// True, if the result of the query can be cached for the specified user
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("is_personal")]
            public bool IsPersonal { get; set; }

            /// <summary>
            /// The results of the query 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("results")]
            public InputInlineQueryResult[] Results { get; set; }

            /// <summary>
            /// Allowed time to cache the results of the query, in seconds 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("cache_time")]
            public int CacheTime { get; set; }

            /// <summary>
            /// Offset for the next inline query; pass an empty string if there are no more results
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("next_offset")]
            public string NextOffset { get; set; }

            /// <summary>
            /// If non-empty, this text should be shown on the button that opens a private chat with the bot and sends a start message to the bot with the parameter switch_pm_parameter 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("switch_pm_text")]
            public string SwitchPmText { get; set; }

            /// <summary>
            /// The parameter for the bot start message
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("switch_pm_parameter")]
            public string SwitchPmParameter { get; set; }
        }


        /// <summary>
        /// Sets the result of an inline query; for bots only 
        /// </summary>
        public static Task<Ok> AnswerInlineQueryAsync(this Client client,
            Int64 inlineQueryId = default(Int64),
            bool isPersonal = default(bool),
            InputInlineQueryResult[] results = default(InputInlineQueryResult[]),
            int cacheTime = default(int),
            string nextOffset = default(string),
            string switchPmText = default(string),
            string switchPmParameter = default(string))
        {
            return client.ExecuteAsync(new AnswerInlineQuery
            {
                InlineQueryId = inlineQueryId,
                IsPersonal = isPersonal,
                Results = results,
                CacheTime = cacheTime,
                NextOffset = nextOffset,
                SwitchPmText = switchPmText,
                SwitchPmParameter = switchPmParameter,
            });
        }
    }
}