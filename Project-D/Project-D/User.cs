using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_D
{
    [SQLite.Table("user")]

    public class User
    {

        [PrimaryKey]
        [AutoIncrement]
        [SQLite.Column("id")]
        public int Id { get; set; }

        [SQLite.Column("fullname")]
        public string Fullname { get; set; }

        [SQLite.Column("email")]
        public string Email { get; set; }

        [SQLite.Column("password")]
        public string Password { get; set; }
        
        [SQLite.Column("quote")]
        public string Quote { get; set; }

        [SQLite.Column("image")]
        public string Image { get; set; }

        [SQLite.Column("sound")]
        public string Sound { get; set; }
    }

    public class UserData
    {
        public int UserId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public List<HeartbeatData> HeartbeatData { get; set; }
    }
}
