using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;

namespace Test_1
{
    public struct ConfigJson
    {
        [JsonProperty ("token")]
        public string Token { get; private set; } // get the token set it to this
        [JsonProperty("prefix")]
        public string Prefix { get; private set; } // get the prefix set it to this
    }
}
