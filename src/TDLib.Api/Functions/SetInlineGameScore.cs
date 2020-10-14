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
        /// Updates the game score of the specified user in a game; for bots only 
        /// </summary>
        public class SetInlineGameScore : Function<Ok>
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "setInlineGameScore";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// Inline message identifier 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("inline_message_id")]
            public string InlineMessageId { get; set; }

            /// <summary>
            /// True, if the message should be edited 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("edit_message")]
            public bool EditMessage { get; set; }

            /// <summary>
            /// User identifier 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("user_id")]
            public int UserId { get; set; }

            /// <summary>
            /// The new score
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("score")]
            public int Score { get; set; }

            /// <summary>
            /// Pass true to update the score even if it decreases. If the score is 0, the user will be deleted from the high score table
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("force")]
            public bool Force { get; set; }
        }


        /// <summary>
        /// Updates the game score of the specified user in a game; for bots only 
        /// </summary>
        public static Task<Ok> SetInlineGameScoreAsync(this Client client,
            string inlineMessageId = default(string),
            bool editMessage = default(bool),
            int userId = default(int),
            int score = default(int),
            bool force = default(bool))
        {
            return client.ExecuteAsync(new SetInlineGameScore
            {
                InlineMessageId = inlineMessageId,
                EditMessage = editMessage,
                UserId = userId,
                Score = score,
                Force = force,
            });
        }
    }
}