using System;
using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        void WriteTransferToDB(Transfer transfer);
        //void UpdateBalanceForTransaction(Transfer transfer);
        Transfer GetTransferByID(int transferId);
        List<Transfer> TransferLookupUserId(int userId);
        List<Transfer> GetAllTransfers();
        List<Transfer> PendingTransferRequests(int userId);
    }
}
