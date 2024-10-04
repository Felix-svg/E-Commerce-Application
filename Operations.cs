using System;
using System.Collections.Generic;

namespace E_CommerceApplication
{
    public class Operations
    {
        private static List<Customer> customers = new List<Customer>();
        private static List<Product> products = new List<Product>();
        private static List<Order> orders = new List<Order>();
        private static Customer currentLoggedInCustomer;

        public static void MainMenu()
        {
            bool flag = true;
            do
            {
                Console.WriteLine("1. Customer Registration\n2. Login\n3. Exit");
                string customerChoice = Console.ReadLine().Trim();

                switch (customerChoice)
                {
                    case "1":
                        CustomerRegistration();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        flag = false;
                        Console.WriteLine("Thanks for Shopping With us. Goodbye!...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again");
                        break;
                }
            } while (flag);
        }

        public static void CustomerRegistration()
        {
            Console.WriteLine("Enter the details below");
            Console.WriteLine("Name");
            string userName = Console.ReadLine().Trim();
            int age;
            do
            {
                Console.WriteLine("Age");
            } while (!int.TryParse(Console.ReadLine().Trim(), out age) || age < 18);
            Console.WriteLine("City");
            string city = Console.ReadLine().Trim();
            Console.WriteLine("Phone Number");
            string phoneNumber = Console.ReadLine().Trim();
            decimal walletBalance;
            do
            {
                Console.WriteLine("Enter Wallet Balance (from 1)");
            } while (!decimal.TryParse(Console.ReadLine().Trim(), out walletBalance) || walletBalance < 1);
            Console.WriteLine("Email ID");
            string emailID = Console.ReadLine().Trim();
            string password;
            string confirmPassword;
            do
            {
               // Console.WriteLine("Password mismatch. Please try again");
                Console.WriteLine("Password");
                password = Console.ReadLine().Trim();
                Console.WriteLine("Confirm Password");
                confirmPassword = Console.ReadLine().Trim();
                if(password != confirmPassword)
                {
                    Console.WriteLine("Password mismatch. Please try again");
                }
            } while (password != confirmPassword);



            Customer newCustomer = new(userName, age, city, phoneNumber, walletBalance, emailID, password, confirmPassword);
            customers.Add(newCustomer);
            Console.WriteLine($"User registration successful.\nYour ID is {newCustomer.CustomerID}");
        }

        public static void Login()
        {
            bool loginSuccessful = false;
            while (!loginSuccessful)
            {
                Console.WriteLine("Enter ID");
                string customerID = Console.ReadLine().ToUpper().Trim();
                Console.WriteLine("Enter Password");
                string password = Console.ReadLine().Trim();

                bool flag = true;
                foreach (Customer customer in customers)
                {
                    if (customer.CustomerID == customerID && password == customer.Password)
                    {
                        flag = false;
                        currentLoggedInCustomer = customer;
                        loginSuccessful = true;
                        Console.WriteLine($"Welcome {customer.CustomerName}");
                        SubMenu();
                        break;
                    }
                }
                if (flag)
                {
                    Console.WriteLine("Invalid Customer ID or Password. Please try again");
                }
            }
        }

        public static void SubMenu()
        {
            Console.WriteLine("a. Purchase\nb. Order History\nc. Cancel Order\nd. Wallet Balance\ne. Wallet Recharge\nf. Exit");
            string userChoice = Console.ReadLine().ToLower().Trim();

            switch (userChoice)
            {
                case "a":
                    Purchase();
                    break;
                case "b":
                    OrderHistory();
                    break;
                case "c":
                    CancelOrder();
                    break;
                case "d":
                    WalletBalance();
                    break;
                case "e":
                    WalletRecharge();
                    break;
                case "f":
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid Choice. Please try again");
                    SubMenu();
                    break;
            }
        }

        public static void Purchase()
        {
            bool purchaseComplete = false;
            while (!purchaseComplete)
            {
                foreach (Product product in products)
                {
                    Console.Write($"Product ID: {product.ProductID} | Product Name: {product.ProductName} | Price: {product.Price} | Shipping Duration: {product.ShippingDuration} | Stock: {product.Stock}\n");
                }

                Console.WriteLine("Select a Product using its ID");
                string productID = Console.ReadLine().ToUpper().Trim();

                bool flag = true;
                foreach (Product product1 in products)
                {
                    if (product1.ProductID == productID)
                    {
                        flag = false;
                        Console.WriteLine($"How Many {product1.ProductName}(s) do you want to purchase?");
                        int quantity;
                        do
                        {
                            Console.WriteLine("Please enter a valid quantity");
                        } while (!int.TryParse(Console.ReadLine().Trim(), out quantity) || quantity < 1);

                        if (product1.Stock < quantity)
                        {
                            Console.WriteLine($"Required count not available. Current availability is {product1.Stock}");
                        }
                        else
                        {
                            decimal deliveryCharge = 50;
                            decimal totalPrice = (quantity * product1.Price) + deliveryCharge;

                            if (currentLoggedInCustomer.WalletBalance >= totalPrice)
                            {
                                currentLoggedInCustomer.WalletBalance -= totalPrice;
                                product1.Stock -= quantity;

                                Order newOrder = new(currentLoggedInCustomer.CustomerID, product1.ProductID, totalPrice, DateTime.Now, quantity, OrderStatus.Ordered);
                                orders.Add(newOrder);
                                Console.WriteLine($"Order Placed Successfully. Order ID: {product1.ProductID}");
                                DateTime deliveryDate = DateTime.Now.AddDays(product1.ShippingDuration);
                                Console.WriteLine($"Your order will be delivered on {deliveryDate.ToString("dd/MM/yyyy")}");
                                purchaseComplete = true;
                            }
                            else
                            {
                                Console.WriteLine("Insufficient Wallet Balance. Please recharge your wallet and do the purchase again");
                                WalletRecharge();
                                break;
                            }
                        }
                        break;
                    }
                }
                if (flag)
                {
                    Console.WriteLine("Invalid Product ID. Enter a valid one to continue");
                }
            }
            SubMenu();
        }

        public static void OrderHistory()
        {
            bool flag = true;
            foreach (Order order in orders)
            {
                if (order.CustomerID == currentLoggedInCustomer.CustomerID)
                {
                    flag = false;
                    Console.Write($"Order ID: {order.OrderID} | Customer ID: {order.CustomerID} | Product ID: {order.ProductID} | TotalPrice: {order.TotalPrice} | PurchaseDate: {order.PurchaseDate.ToString("dd/MM/yyyy")} | Quantity: {order.Quantity} | Status: {order.OrderStatus}\n");
                }
            }
            if (flag)
            {
                Console.WriteLine($"No Order History found for {currentLoggedInCustomer.CustomerName}");
            }
            SubMenu();
        }

        public static void CancelOrder()
        {
            bool orderCancelled = false;
            while (!orderCancelled)
            {


                foreach (Order order in orders)
                {
                    if (order.CustomerID == currentLoggedInCustomer.CustomerID && order.OrderStatus == OrderStatus.Ordered)
                    {
                        Console.Write($"Order ID: {order.OrderID} | Customer ID: {order.CustomerID} | Product ID: {order.ProductID} | TotalPrice: {order.TotalPrice} | PurchaseDate: {order.PurchaseDate.ToString("dd/MM/yyyy")} | Quantity: {order.Quantity} | Status: {order.OrderStatus}\n");
                    }
                }

                Console.WriteLine("Select an Order to cancel by its ID");
                string orderID = Console.ReadLine().ToUpper().Trim();

                bool flag = true;
                foreach (Order order in orders)
                {
                    if (order.OrderID == orderID && order.CustomerID == currentLoggedInCustomer.CustomerID && order.OrderStatus == OrderStatus.Ordered)
                    {
                        flag = false;
                        bool flag1 = true;
                        foreach (Product product in products)
                        {
                            if (order.ProductID == product.ProductID)
                            {
                                flag1 = false;
                                product.Stock += order.Quantity;
                                currentLoggedInCustomer.WalletBalance += order.TotalPrice;
                                order.OrderStatus = OrderStatus.Cancelled;
                                Console.WriteLine($"Order {order.OrderID} cancelled successfully");
                                orderCancelled = true;
                                break;
                            }
                        }
                        if (flag1)
                        {
                            Console.WriteLine("Failed to cancel order. Please try again");
                        }
                        break;
                    }
                }
                if (flag)
                {
                    Console.WriteLine($"Invalid Order ID. Please enter a valid one to continue");
                }
            }
            SubMenu();
        }

        public static void WalletBalance()
        {
            bool foundUser = false;
            while (!foundUser)
            {
                bool flag = true;
                foreach (Customer customer in customers)
                {
                    if (customer.CustomerID == currentLoggedInCustomer.CustomerID)
                    {
                        flag = false;
                        Console.WriteLine($"Balance: {currentLoggedInCustomer.WalletBalance:C}");
                        foundUser = true;
                        break;
                    }
                }
                if (flag)
                {
                    Console.WriteLine("Failed to retrieve user balance");
                }
            }
            SubMenu();
        }

        public static void WalletRecharge()
        {
            bool flag = true;
            do
            {
                Console.WriteLine("Do you want to recharge your wallet? (yes/no)");
                string userChoice = Console.ReadLine().ToLower().Trim();
                if (userChoice == "yes")
                {
                    //Console.WriteLine("Enter amount to recharge");
                    decimal rechargeAmount;
                    do
                    {
                        // Console.WriteLine("Please enter a valid recharge amount");
                        Console.WriteLine("Enter amount to recharge");
                    } while (!decimal.TryParse(Console.ReadLine().Trim(), out rechargeAmount) || rechargeAmount < 1);
                    currentLoggedInCustomer.WalletRecharge(rechargeAmount);
                    Console.WriteLine($"New Balance: {currentLoggedInCustomer.WalletBalance:C}");
                }
                else if (userChoice == "no")
                {
                    flag = false;
                    SubMenu();
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter 'yes' or 'no'.");
                }
            } while (flag);
        }

        public static void DefaultData()
        {
            customers.Add(new("Default Customer", 30, "Default City", "0123456789", 10000, "defaultuser@app.com", "123", "123"));

            products.Add(new("Mobile (Samsung)", 10, 10000, 3));
            products.Add(new("Tablet (Lenovo)", 5, 15000, 2));
            products.Add(new("Camera (Sony)", 3, 20000, 4));
            products.Add(new("iPhone", 5, 50000, 6));
            products.Add(new("Laptop (Lenovo i3)", 3, 40000, 3));
            products.Add(new("Headphone (Boat)", 5, 1000, 2));
            products.Add(new("Speakers (Boat)", 4, 500, 2));

            orders.Add(new(customers[0].CustomerID, products[3].ProductID, 20000, DateTime.Now, 2, OrderStatus.Ordered));
        }
    }
}