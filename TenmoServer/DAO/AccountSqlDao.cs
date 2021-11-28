using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountDao
    {
        private readonly string connectionString;
        public AccountSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Account> GetAccounts()
        {
            List<Account> account = new List<Account>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * from ACCOUNTS AS a " +
                        "JOIN users AS u on a.user_id = u.user_id;", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        account.Add(GetAccountFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return account;
        }
        public Account GetAccount(int userId)
        {
            Account account = new Account();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance FROM accounts WHERE user_id = @user_id;", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        account = GetAccountFromReader(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return account;
        }
        public decimal GetBalance(int id)
        {
            decimal balance = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT accounts.balance FROM accounts " +
                                                    "JOIN users ON users.user_id = accounts.user_id " +
                                                    "WHERE users.user_id = @users.user_id;", conn);
                    cmd.Parameters.AddWithValue("@users.user_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Account account = GetAccountFromReader(reader);
                        balance = account.Balance;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return balance;
        }

        public Account Update(int id, Account updated)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE accounts SET balance = @balance WHERE account_id = @account_id;", conn);
                    cmd.Parameters.AddWithValue("@account_id", id);
                    cmd.Parameters.AddWithValue("@balance", updated.Balance);
                    cmd.ExecuteNonQuery();

                }
            }
            catch (SqlException)
            {

                throw;
            }
            return updated;
        }

        private Account GetAccountFromReader(SqlDataReader reader)
        {
            Account a = new Account()
            {
                AccountId = Convert.ToInt32(reader["account_id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                Balance = Convert.ToDecimal(reader["balance"]),
            };
            return a;
        }
    }
}
