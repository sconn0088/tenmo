using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient
{
    public class ApiService
    {
        private readonly string API_URL = "";
        private readonly RestClient client = new RestClient();  
        private static ApiUser user = new ApiUser();


        public bool LoggedIn { get { return !string.IsNullOrWhiteSpace(user.Token); } }

        public ApiService(string api_url)
        {
            API_URL = api_url;
        }

        public decimal GetBalance(int userId)
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + userId + "/balance"); //header
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<decimal> response = client.Get<decimal>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }
        }

        public List<Transfer> TransferLookupUserId(int userId)
        {
            // + userId + "/alltransfers"
            RestRequest request = new RestRequest(API_URL + "transfer/" + userId + "/alltransfers");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }
        }

        public Transfer GetTransferById(int transferId)
        {
            RestRequest request = new RestRequest(API_URL + "transfer/" + transferId);
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<Transfer> response = client.Get<Transfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }
        }

        public List<Transfer> PendingTransferRequests(int userId)
        {
            RestRequest request = new RestRequest(API_URL + "transfer/" + userId + "/pendingtransfers");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }
        }

        public List<User> GetAllUsers()
        {
            RestRequest request = new RestRequest(API_URL + "user");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<User>> response = client.Get<List<User>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception("Error occured - unable to reach server", response.ErrorException);
            }
            else
            {
                return response.Data;
            }

        }
        public User GetUser(string username)
        {
            RestRequest request = new RestRequest(API_URL + "user/" + username);
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<User> response = client.Get<User>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception("Error occured - unable to reach server", response.ErrorException);
            }
            else
            {
                return response.Data;
            }
        }
        public Transfer CreateNewTransferObject()
        {
            Transfer transfer = new Transfer();

            Console.WriteLine("Please select a user to send to: ");

            User transferSend = GetUser(Console.ReadLine());
            transfer.AccountTo = transferSend.UserId;

            Console.WriteLine("Please enter the amount to send: ");
            transfer.Amount = decimal.Parse(Console.ReadLine());
            transfer.AccountFrom = UserService.GetUserId();
            transfer.TransferStatusId = 2;
            transfer.TransferTypeId = 2;
            return transfer;
        }

        public Transfer CreateTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest(API_URL + "user");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<Transfer> response = client.Get<Transfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception("Error occured - unable to reach server", response.ErrorException);
            }
            else
            {
                return response.Data;
            }
        }

        public Account GetAccount(int userId)
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + userId);
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<Account> response = client.Get<Account>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }
        }

        public void WriteTransferToDB(Transfer transfer)
        {
            RestRequest request = new RestRequest(API_URL + "transfer/newtransfer");
            request.AddJsonBody(transfer);
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<Transfer> response = client.Post<Transfer>(request);
        }

        public Account UpdateAccount(Account accountToUpdate)
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + accountToUpdate.AccountId);
            request.AddJsonBody(accountToUpdate);
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<Account> response = client.Put<Account>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {
                throw new Exception("Error occurred - received non-success response: " + (int)response.StatusCode);
            }
            else
            {
                return response.Data;
            }
        }
    }
}
