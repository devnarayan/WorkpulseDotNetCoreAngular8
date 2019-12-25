using System;
using System.Collections.Generic;

namespace CORTNE.EFModelCORTNEDB
{
    public partial class TemplateNotice
    {
        public int Id { get; set; }
        public string Mission { get; set; }
        public string Vision { get; set; }
        public string GovernorName { get; set; }
        public string SurgeonName { get; set; }
        public DateTime? Date { get; set; }
        public string AccManagerName { get; set; }
        public string OfficeAddress { get; set; }
        public string Emailid { get; set; }
        public string WrokedBy { get; set; }
        public DateTime? WorkedDate { get; set; }
    }
}
