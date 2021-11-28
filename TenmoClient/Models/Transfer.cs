namespace TenmoClient.Models
{
    public class Transfer
    {
        //public Transfer()
        //{
        //    //for type paramater
        //}

        public int TransferId { get; set; }
        public int TransferTypeId { get; set; }
        public string TransferTypeDesc { get; set; }
        public int TransferStatusId { get; set; }
        public string TransferStatusDesc { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }

        public Transfer()
        {

        }

        public Transfer(int transferTypeId, int transferStatusId, int accountFrom, int accountTo, decimal amount)
        {
            TransferTypeId = transferTypeId;
            TransferStatusId = transferStatusId;
            AccountFrom = accountFrom;
            AccountTo = accountTo;
            Amount = amount;
        }
    }
}
