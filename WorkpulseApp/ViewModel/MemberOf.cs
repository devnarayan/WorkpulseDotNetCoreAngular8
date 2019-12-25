﻿using Microsoft.Graph;
using System.Collections.Generic;

namespace TestGraphApi.Models
{
    public class MemberOf
    {
        public List<DirectoryObject> Value { get; set; }
    }

    public class ADUserModel
    {
        public List<string> businessPhones { get; set; }
        public string displayName { get; set; }
        public string givenName { get; set; }
        public string jobTitle { get; set; }
        public string mail { get; set; }
        public object mobilePhone { get; set; }
        public string officeLocation { get; set; }
        public object preferredLanguage { get; set; }
        public string surname { get; set; }
        public string userPrincipalName { get; set; }
        public string id { get; set; }
    }
}
