using System;

namespace WorkpulseApp.Models
{
    public partial class TimeLog
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public TimeSpan? TimeIn1 { get; set; }
        public TimeSpan? TimeOut1 { get; set; }
        public TimeSpan? TimeIn2 { get; set; }
        public TimeSpan? TimeOut2 { get; set; }
        public TimeSpan? TimeIn3 { get; set; }
        public TimeSpan? TimeOut3 { get; set; }
        public TimeSpan? TimeIn4 { get; set; }
        public TimeSpan? TimeOut4 { get; set; }
        public string UserName { get; set; }
    }
}
