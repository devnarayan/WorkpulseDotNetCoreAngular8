using System;
using System.Collections.Generic;

namespace CORTNE.Models
{
    public partial class AppException
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public string ExceptionMessage { get; set; }
        public string Stacktrace { get; set; }
        public string Controller { get; set; }
        public string ExceptionType { get; set; }
        public DateTime? ErrorDate { get; set; }
    }
}
