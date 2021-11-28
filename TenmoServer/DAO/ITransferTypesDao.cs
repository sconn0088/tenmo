using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    interface ITransferTypesDao
    {
        public enum TransferType
        {
            Request = 1,
            Send = 2
        }
    }
}
