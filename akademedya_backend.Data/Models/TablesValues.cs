using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace akademedya_backend.Data.Models
{
    public class TablesValues
    {
        public int TableId { get; set; }

        public int InputAreaId { get; set; }

        public string Value { get; set; } = string.Empty;

        [JsonIgnore]
        public TableInformations ?TableInformations { get; set; }


    }
}
