using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkpulseApp.ViewModel
{
    public class ApplicationConfigurationLkViewModel
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
        public string Value { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class CreateApplicationConfigurationLkViewModelDto
    {        
        public string Description { get; set; }
        public string Area { get; set; }
        public string Value { get; set; }
        public string UpdatedBy { get; set; }             
    }

    public class DeleteApplicationConfigurationLkViewModelDto
    {
        public int ID { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class GetApplicationConfigurationLkViewModelDto
    {
        public int ID { get; set; }
        public string Description { get; set; }        
        public string Value { get; set; }        
    }

    public class UpdateApplicationConfigurationLkViewModelDto
    {
        public int ID { get; set; }
        public string Description { get; set; }        
        public string Value { get; set; }
        public string UpdatedBy { get; set; }        
    }
}
