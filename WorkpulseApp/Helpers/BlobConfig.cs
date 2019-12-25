using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkpulseApp.Helpers
{
    public class BlobConfig
    {
        public string StorageConnection { get; set; }
        public string Container { get; set; }
        public string CashLogContainer { get; set; }
        public string CashReceiptContainer { get; set; }
        public string DepositContainer { get; set; }
        public string TransferContainer { get; set; }
        public string AccountCodeContainer { get; set; }
    }
}
