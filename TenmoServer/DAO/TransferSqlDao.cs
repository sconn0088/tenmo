using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using System.Data.SqlClient;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Transfer> GetAllTransfers()
        {
            List<Transfer> transfers = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT t.transfer_id, t.transfer_type_id, tt.transfer_type_desc, t.transfer_status_id, ts.transfer_status_desc, t.account_from, t.account_to, t.amount FROM transfers t JOIN transfer_types tt ON t.transfer_type_id = t.transfer_type_id JOIN transfer_statuses ts ON ts.transfer_status_id = t.transfer_status_id;", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        transfers.Add(GetTransferFromReader(reader));
                    }
                }
            }
            catch (Exception wtf)
            {

                throw wtf;
            }
            return transfers;
        }

        public Transfer GetTransferByID(int transferId)
        {
            Transfer transfer = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT t.transfer_id, t.transfer_type_id, tt.transfer_type_desc, t.transfer_status_id, ts.transfer_status_desc, " +
                                                    "t.account_from, t.account_to, t.amount FROM transfers t " +
                                                    "JOIN transfer_types tt ON tt.transfer_type_id = t.transfer_type_id " +
                                                    "JOIN transfer_statuses ts ON ts.transfer_status_id = t.transfer_status_id " +
                                                    "JOIN accounts a ON a.account_id = t.account_from or a.account_id = t.account_to " +
                                                    "WHERE t.transfer_id = @transfer_id;", conn);
                    cmd.Parameters.AddWithValue("@transfer_id", transferId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        transfer = GetTransferFromReader(reader);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return transfer;
        }

        public List<Transfer> TransferLookupUserId(int userID)
        {
            List<Transfer> transfers = new List<Transfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT t.transfer_id, t.transfer_type_id, tt.transfer_type_desc, t.transfer_status_id, ts.transfer_status_desc, t.account_from, t.account_to, t.amount " +
                                                    "FROM transfers t " +
                                                    "JOIN transfer_types tt ON tt.transfer_type_id = t.transfer_type_id " +
                                                    "JOIN transfer_statuses ts ON ts.transfer_status_id = t.transfer_status_id " +
                                                    "JOIN accounts a ON a.account_id = t.account_from or a.account_id = t.account_to " +
                                                    "JOIN users u ON u.user_id = a.user_id " +
                                                    "WHERE u.user_id = @user_id;", conn);

                    //string testSQLstrings = "select t.transfer_id, tt.transfer_type_desc, ts.transfer_status_desc, (select u.username from users u join accounts a on a.user_id = u.user_id where a.account_id = t.account_from), (select u.username from users u join accounts a on a.user_id = u.user_id where a.account_id = t.account_to), t.amount from transfers t join transfer_types tt on tt.transfer_type_id = t.transfer_type_id join transfer_statuses ts on ts.transfer_status_id = t.transfer_status_id join accounts a on a.account_id = t.account_from or a.account_id = t.account_to join users u on u.user_id = a.user_id where u.user_id = @user_id;";
                    //SqlCommand cmd = new SqlCommand(testSQLstrings, conn);

                    cmd.Parameters.AddWithValue("@user_id", userID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Transfer transfer = GetTransferFromReader(reader);
                        transfers.Add(transfer);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return transfers;
        }

        public List<Transfer> PendingTransferRequests(int userID)
        {
            List<Transfer> transfers = new List<Transfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT t.transfer_id, t.transfer_type_id, tt.transfer_type_desc, t.transfer_status_id, ts.transfer_status_desc, " +
                                                    "t.account_from, t.account_to, t.amount FROM transfers t " +
                                                    "JOIN transfer_types tt ON tt.transfer_type_id = t.transfer_type_id " +
                                                    "JOIN transfer_statuses ts ON ts.transfer_status_id = t.transfer_status_id " +
                                                    "JOIN accounts a ON a.account_id = t.account_from or a.account_id = t.account_to " +
                                                    "JOIN users u ON u.user_id = a.user_id " +
                                                    "WHERE u.user_id = @user_id AND ts.transfer_status_id = 1;", conn);
                    cmd.Parameters.AddWithValue("@user_id", userID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Transfer transfer = GetTransferFromReader(reader);
                        transfers.Add(transfer);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return transfers;
        }


        public void WriteTransferToDB(Transfer transfer)
        {
            Transfer dcTransfer = new Transfer();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount);", conn);
                    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception wtDB) //naming conventions based on method to debug easier
            {
                throw wtDB;
            }
        }

        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer t = new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]),
                TransferTypeDesc = Convert.ToString(reader["transfer_type_desc"]),
                TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]),
                TransferStatusDesc = Convert.ToString(reader["transfer_status_desc"]),
                AccountFrom = Convert.ToInt32(reader["account_from"]),
                AccountTo = Convert.ToInt32(reader["account_to"]),
                Amount = Convert.ToDecimal(reader["amount"]),
            };
            return t;
        }
    }
}
