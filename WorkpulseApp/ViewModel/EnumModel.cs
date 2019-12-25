using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkpulseApp.ViewModel
{
    public enum CashReceiptStatus
    {
        All = 0,
        Pending = 1,
        Approved = 2,
        Deposit = 3,
        Transfer = 4
    }
    public enum DepositReceiptStatus
    {
        All = 0,
        Deposited = 1,
        AccountCodeAdded = 2,
        Deleted = 3
    }
    public enum AccountCodeStatus
    {
        All = 0,
        Pending = 1,
        Submitted = 2,
        Deleted = 3
    }

    public enum ContainerEnum
    {
        Container = 0,
        CashLogContainer = 1,
        CashReceiptContainer = 2,
        DepositContainer = 3,
        TransferContainer = 4,
        AccountCodeContainer = 5
    }
}
