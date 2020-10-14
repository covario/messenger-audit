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
        /// Returns the received vector of objects containing a number; for testing only. This is an offline method. Can be called before authorization 
        /// </summary>
        public class TestCallVectorIntObject : Function<TestVectorIntObject>
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "testCallVectorIntObject";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// Vector of objects to return
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("x")]
            public TestInt[] X { get; set; }
        }


        /// <summary>
        /// Returns the received vector of objects containing a number; for testing only. This is an offline method. Can be called before authorization 
        /// </summary>
        public static Task<TestVectorIntObject> TestCallVectorIntObjectAsync(this Client client,
            TestInt[] x = default(TestInt[]))
        {
            return client.ExecuteAsync(new TestCallVectorIntObject
            {
                X = x,
            });
        }
    }
}