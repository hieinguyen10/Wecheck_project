using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace Wecheck_project.Models
{
    public partial class Location
    {
        [PrimaryKey]
        [JsonProperty("name")]
        public string location { get; set; }
        public bool isFavorite { get; set; }

    }
}
