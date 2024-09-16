using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace akademedya_backend.Data.Models
{
    public class UsersInformation
    {
        public int UserId { get; set; }

        public string Firstname { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool isActive { get; set; }

        [JsonIgnore] // swaggerda foreign key tabloları gözükmesin diye yazıyoruz
        public ICollection<UserTables>? UserTables { get; set; }


    }
}
