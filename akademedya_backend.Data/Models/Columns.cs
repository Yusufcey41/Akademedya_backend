using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace akademedya_backend.Data.Models
{
    public class Columns
    {
        public int TableId { get; set; }


        public int ColumnId { get; set; }


        public string ColumnName { get; set; } = string.Empty;

        [JsonIgnore]
        public TableInformations ?TableInformations { get; set; }



    }
}
