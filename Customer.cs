namespace E_CommerceApplication
{
    public class Customer
    {
        private static int s_customerID = 3001;
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string MobileNumber { get; set; }
        public decimal WalletBalance { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

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

        public Customer(string customerName, int age, string city, string mobileNumber, decimal walletBalance, string emailID, string password, string confirmPassword)
        {
            CustomerID = $"CID{s_customerID++}";
            CustomerName = customerName;
            Age = age;
            City = city;
            MobileNumber = mobileNumber;
            WalletBalance = walletBalance;
            EmailID = emailID;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}