using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        decimal GetBalance(int id);
        Account GetAccount(int userId);
        List<Account> GetAccounts();
        Account Update(int id, Account updated);
    }
}
