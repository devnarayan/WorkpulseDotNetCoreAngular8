using System.Collections.Generic;

namespace CORTNE.ViewModel
{
    public class CountyModel
    {
        public string CountyName { get; set; }
        public string CountyId { get; set; }
        public bool Assigned { get; set; }
    }

    public class VMUpdateUserRole
    {
        public string UserName { get; set; }
        public List<CountyModel> UserCounties { get; set; }
        public List<RoleModel> UserRoles { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class VMUpdateDate
    {
        public int Id { get; set; }
        public System.DateTime ProcessedDate { get; set; }
        public string UserName { get; set; }

    }

}
