using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Domain.Enums
{
    public enum ContractStatus
    {
        Pending = 0,
        Signed = 1,
        Rejected = 2,
        Active = 3,
        WaitingTransaction= 4,
        Expired = 5,
        CusComplained=6,
        Others=7
    }
}

