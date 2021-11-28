using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    interface ITransferStatusesDao
    {
        public enum TransferStatus
        {
            Pending = 1,
            Approved = 2,
            Rejected = 3
        }
    }
}
