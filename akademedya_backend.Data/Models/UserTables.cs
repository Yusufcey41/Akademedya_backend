using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace akademedya_backend.Data.Models
{
    public class UserTables
    {
        public int UserId { get; set; }

        public int TableId { get; set; }

        [JsonIgnore]
        public UsersInformation ?UserInformation { get; set; }

        [JsonIgnore]
        public TableInformations ?TableInformations { get; set; }
    }
}
