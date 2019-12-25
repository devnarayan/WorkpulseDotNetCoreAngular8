using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkpulseApp.ViewModel
{
    public class LocationViewModel
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string OrgCode { get; set; }
        public string Flaircode { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
