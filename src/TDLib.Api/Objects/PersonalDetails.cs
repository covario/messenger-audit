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
        /// Contains the user's personal details
        /// </summary>
        public class PersonalDetails : Object
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "personalDetails";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// First name of the user written in English; 1-255 characters 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("first_name")]
            public string FirstName { get; set; }

            /// <summary>
            /// Middle name of the user written in English; 0-255 characters 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("middle_name")]
            public string MiddleName { get; set; }

            /// <summary>
            /// Last name of the user written in English; 1-255 characters
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("last_name")]
            public string LastName { get; set; }

            /// <summary>
            /// Native first name of the user; 1-255 characters 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("native_first_name")]
            public string NativeFirstName { get; set; }

            /// <summary>
            /// Native middle name of the user; 0-255 characters 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("native_middle_name")]
            public string NativeMiddleName { get; set; }

            /// <summary>
            /// Native last name of the user; 1-255 characters
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("native_last_name")]
            public string NativeLastName { get; set; }

            /// <summary>
            /// Birthdate of the user 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("birthdate")]
            public Date Birthdate { get; set; }

            /// <summary>
            /// Gender of the user, "male" or "female" 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("gender")]
            public string Gender { get; set; }

            /// <summary>
            /// A two-letter ISO 3166-1 alpha-2 country code of the user's country 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("country_code")]
            public string CountryCode { get; set; }

            /// <summary>
            /// A two-letter ISO 3166-1 alpha-2 country code of the user's residence country
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("residence_country_code")]
            public string ResidenceCountryCode { get; set; }
        }
    }
}