namespace E_CommerceApplication
{
    public class Customer
    {
        private static int s_customerID = 3000;
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string City { get; set; }
        public string MobileNumber { get; set; }
        public decimal WalletBalance { get; set; }
        public string EmailID { get; set; }

        public decimal WalletRecharge(decimal amount)
        {
            WalletBalance += amount;
            return WalletBalance;
        }

        public decimal DeductBalance(decimal amount)
        {
            WalletBalance -= amount;
            return WalletBalance;
        }

        public Customer(string customerName, string city, string mobileNumber, decimal walletBalance, string emailID)
        {
            CustomerID = $"CID{s_customerID++}";
            CustomerName = customerName;
            City = city;
            MobileNumber = mobileNumber;
            WalletBalance = walletBalance;
            EmailID = emailID;
        }
    }
}