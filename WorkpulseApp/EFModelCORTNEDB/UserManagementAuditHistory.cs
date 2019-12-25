using System;

namespace WorkpulseApp.Models
{
    public partial class UserManagementAuditHistory
    {
        public int RowId { get; set; }
        public string UserName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string RoleId { get; set; }
        public string ActionType { get; set; }
        public string LocationCode { get; set; }

    }
}
