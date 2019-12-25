using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkpulseApp.EFModelCORTNEDB
{
    public partial class ApplicationConfigurationLk
    {
        public int ID { get; set; }
        public string InCode_Lookup { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
        public string Value { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
