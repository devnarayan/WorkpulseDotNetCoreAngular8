﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkpulseApp.EFModelCORTNEDB
{
    public partial class USStates
    {
        public int Id { get; set;}
        public string Name { get; set; }
        public string AlphaCode { get; set; }
        public int FIPSCode { get; set; }
    }
}
