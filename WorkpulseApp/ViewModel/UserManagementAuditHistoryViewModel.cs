using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkpulseApp.ViewModel
{
    public class UserManagementAuditHistoryViewModel
    {
        public int RowId { get; set; }
        public string UserName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string RoleId { get; set; }
        public string ActionType { get; set; }
        public string LocationCode { get; set; }
        public DateTime? ModifiedStartDate { get; set; }
        public DateTime? ModifiedEndDate { get; set; }
        public string ComponentName { get; set; }
        public string CountyName { get; set; }
    }
}
