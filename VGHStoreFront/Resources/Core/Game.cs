using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace PriceCheckerVGH
{
    [DataContract]
    public class Game
    {
        [DataMember(Name = "gamestop-price")]
        public string price { get; set; }

        [DataMember(Name = "console-name")]
        public string console { get; set; }

        [DataMember(Name = "product-name")]
        public string title { get; set; }

        [DataMember(Name = "upc")]
        public string upc { get; set; }
    }
}
