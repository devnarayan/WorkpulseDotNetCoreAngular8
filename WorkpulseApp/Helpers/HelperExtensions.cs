using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkpulseApp.Helpers
{
    public static class HelperExtensions
    {
        public static decimal ToDecimalSafe(this double input)
        {
            if (input < (double)decimal.MinValue)
                return decimal.MinValue;
            else if (input > (double)decimal.MaxValue)
                return decimal.MaxValue;
            else
                return (decimal)input;
        }
    }
}
