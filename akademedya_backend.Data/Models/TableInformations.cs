using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace akademedya_backend.Data.Models
{
    public class TableInformations
    {
        public int TableId{ get; set; }

        public string TableName { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        [JsonIgnore]
        public UserTables ?UserTables { get; set; }

        [JsonIgnore]
        public ICollection<Columns> ?Columns { get; set; }

        [JsonIgnore]
        public ICollection<TablesValues> ?TablesValue { get; set; }
    }
}
