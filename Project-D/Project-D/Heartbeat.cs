using SQLite;
using System;

namespace Project_D
{
    [Table("heartbeat")]
    public class Heartbeat
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int UserId { get; set; }

        public string Day { get; set; }

        public int AvgBPM { get; set; }
    }
    public class HeartbeatData
    {
        public string Time { get; set; }
        public int BPM { get; set; }
    }
}
