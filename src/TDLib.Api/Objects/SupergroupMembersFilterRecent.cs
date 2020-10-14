using System;
using Newtonsoft.Json;

namespace TdLib
{
    /// <summary>
    /// Autogenerated TDLib APIs
    /// </summary>
    public static partial class TdApi
    {
        /// <summary>
        /// Returns recently active users in reverse chronological order
        /// </summary>
        public partial class SupergroupMembersFilter : Object
        {
            /// <summary>
            /// Returns recently active users in reverse chronological order
            /// </summary>
            public class SupergroupMembersFilterRecent : SupergroupMembersFilter
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "supergroupMembersFilterRecent";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }
            }
        }
    }
}