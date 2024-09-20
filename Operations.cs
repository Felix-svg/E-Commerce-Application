using System;
using System.Collections.Generic;

namespace E_CommerceApplication
{
    public class Operations
    {
        private static List<Customer> customers = [];
        private static List<Product> products = [];
        private static List<Order> orders = [];
        private static Customer currentLoggedInUser;

        public static void MainMenu()
        {
            bool flag = true;

            do
            {
                Console.WriteLine("Choose an option");
                Console.WriteLine("1. Customer Registration\n2. Login\n3. Exit");

                string userChoice = Console.ReadLine().Trim();
                switch (userChoice)
                {
                    case "1":
                        CustomerRegistration();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        flag = false;
                        Console.WriteLine("Goodbye");
                        break;
                }
            } while (flag);
        }

        public static void CustomerRegistration()
        {
            Console.WriteLine("Enter your name");
            string name = Console.ReadLine();
            Console.WriteLine("Enter your city");
            string city = Console.ReadLine();
            Console.WriteLine("Enter your mobile number");
            string mobileNumber = Console.ReadLine();
            Console.WriteLine("Enter your wallet balance");
            decimal walletBalance = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter your email");
            string email = Console.ReadLine();

            Customer customer = new(name, city, mobileNumber, walletBalance, email);
            Console.WriteLine($"Customer ID: {customer.CustomerID}");
            customers.Add(customer);
        }

        public static void Login()
        {
            Console.WriteLine("Enter Customer ID");
            string customerID = Console.ReadLine().ToUpper().Trim();

            bool flag = true;
            foreach (Customer customer in customers)
            {
                if (customerID == customer.CustomerID)
                {
                    flag = false;
                    currentLoggedInUser = customer;
                    SubMenu();
                    break;
                }
            }
            if (flag)
            {
                Console.WriteLine("Invalid Customer ID");
                Login();
            }
        }

        public static void SubMenu()
        {
            Console.WriteLine("Choose an option to continue");
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
            }

        }

        public static void Purchase()
        {
            foreach (Product product in products)
            {
                Console.Write($"Product ID: {product.ProductID} | Product Name: {product.ProductName} | Price: {product.Price} | Stock: {product.Stock} | Shipping Duration: {product.ShippingDuration}\n");
            }

            Console.WriteLine("Enter product ID");
            string productID = Console.ReadLine().ToUpper().Trim();

            bool flag = true;
            foreach (Product product in products)
            {
                if (productID == product.ProductID)
                {
                    flag = false;

                    Console.WriteLine("How much of the product do you want to purchase?");
                    int requiredCount = int.Parse(Console.ReadLine());
                    if (requiredCount > product.Stock)
                    {
                        Console.WriteLine($"Required count not available. Current availability is {product.Stock}");
                    }
                    else
                    {
                        decimal totalAmount = (requiredCount * product.Price) + 50;

                        bool flag1 = true;
                        foreach (Customer customer in customers)
                        {
                            if (customer.CustomerID == currentLoggedInUser.CustomerID)
                                flag1 = false;
                            {
                                if (customer.WalletBalance >= totalAmount)
                                {
                                    customer.WalletBalance -= totalAmount;
                                    product.Stock -= requiredCount;

                                    Order order = new(customer.CustomerID, product.ProductID, (int)totalAmount, DateTime.Now, requiredCount, OrderStatus.Ordered);
                                    Console.WriteLine($"Order placed successfully. Order ID: {order.OrderID}");
                                    orders.Add(order);
                                    Console.WriteLine($"Order placed successfully. Your order will be delivered on {order.PurchaseDate.AddDays(product.ShippingDuration)}");

                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Insufficient Wallet Balance. Please recharge your wallet and do the purchase again");
                                    WalletRecharge();
                                    Purchase();
                                }
                            }
                            break;
                        }
                        if (flag1)
                        {
                            Console.WriteLine("Order creation failed");
                        }
                    }
                }
            }
            if (flag)
            {
                Console.WriteLine("Invalid Product ID");
            }
            SubMenu();
        }

        public static void OrderHistory()
        {
            bool flag = true;
            foreach (Customer customer in customers)
            {
                if (customer.CustomerID == currentLoggedInUser.CustomerID)
                {
                    flag = false;
                    foreach (Order order in orders)
                    {
                        if (order.CustomerID == customer.CustomerID)
                        {
                            Console.Write($"Order ID: {order.OrderID} | Customer ID: {order.CustomerID} | Product ID: {order.ProductID} | Total Price: {string.Format("{0:C}", order.TotalPrice)} | Purchase date: {order.PurchaseDate} | Quantity: {order.Quantity} | Order Status: {order.OrderStatus}\n");
                        }
                    }
                }
            }
            if (flag)
            {
                Console.WriteLine("No Order History found");
            }
            SubMenu();
        }

        public static void CancelOrder()
        {
            bool flag = true;
            foreach (Customer customer in customers)
            {
                if (customer.CustomerID == currentLoggedInUser.CustomerID)
                {
                    flag = false;
                    foreach (Order order in orders)
                    {
                        if (order.CustomerID == customer.CustomerID && order.OrderStatus == OrderStatus.Ordered)
                        {
                            Console.Write($"Order ID: {order.OrderID} | Customer ID: {order.CustomerID} | Product ID: {order.ProductID} | Total Price: {string.Format("{0:C}", order.TotalPrice)} | Purchase Date: {order.PurchaseDate} | Quantity: {order.Quantity} | Order Status: {order.OrderStatus}\n");

                            Console.WriteLine("Enter the Order ID you want to cancel.");
                            string orderID = Console.ReadLine().ToUpper().Trim();
                            if (order.OrderID == orderID)
                            {
                                foreach (Product product in products)
                                {
                                    if (product.ProductID == order.ProductID)
                                    {
                                        product.Stock += order.Quantity;
                                        customer.WalletBalance += order.TotalPrice;
                                        order.OrderStatus = OrderStatus.Cancelled;
                                        Console.WriteLine($"Order:{order.OrderID} cancelled successfully");
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid Order ID");
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            if (flag)
            {
                Console.WriteLine("Order not found");
            }
            SubMenu();
        }

        public static void WalletBalance()
        {
            bool flag = true;
            foreach (Customer customer in customers)
            {
                if (customer.CustomerID == currentLoggedInUser.CustomerID)
                {
                    flag = false;
                    Console.WriteLine($"Current Balance: {string.Format("{0:C}", customer.WalletBalance)}");
                    break;
                }
            }
            if (flag)
            {
                Console.WriteLine("Invalid Customer ID");
            }
            SubMenu();
        }

        public static void WalletRecharge()
        {
            bool flag = true;
            foreach (Customer customer in customers)
            {
                if (customer.CustomerID == currentLoggedInUser.CustomerID)
                {
                    flag = false;
                    Console.WriteLine("Do you want to recharge your wallet? (yes/no)");
                    string userChoice = Console.ReadLine().ToLower().Trim();

                    if (userChoice == "yes")
                    {
                        Console.WriteLine("How much do you wish to recharge?");
                        decimal amount = decimal.Parse(Console.ReadLine().Trim());
                        customer.WalletRecharge(amount);
                        Console.WriteLine($"Recharge successful\nCurrent Balance: {string.Format("{0:C}", customer.WalletBalance)}");
                    }
                    else if (userChoice == "no")
                    {
                        SubMenu();
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please enter yes or no");
                        WalletRecharge();
                    }
                }
            }
            if (flag)
            {
                Console.WriteLine("Invalid Customer ID");
                Login();
            }
            SubMenu();
        }

        public static void DefaultData()
        {
            Customer customer1 = new("The Collector", "Knowhere", "9885858588", 50000, "collector@mail.com");
            Customer customer2 = new("Gamora", "Vomir", "9888475757", 60000, "gamora@mail.com");
            customers.Add(customer1);
            customers.Add(customer2);

            Product product1 = new("Mobile (Samsung)", 10, 10000, 3);
            Product product2 = new("Tablet (Lenovo)", 5, 15000, 2);
            Product product3 = new("Camera (Sony)", 3, 20000, 4);
            Product product4 = new("iPhone", 5, 50000, 6);
            Product product5 = new("Laptop (Lenovo I3)", 3, 40000, 3);
            Product product6 = new("Headphone (Boat)", 5, 1000, 2);
            Product product7 = new("Speakers (Boat)", 4, 500, 2);
            products.Add(product1);
            products.Add(product2);
            products.Add(product3);
            products.Add(product4);
            products.Add(product5);
            products.Add(product6);
            products.Add(product7);

            Order order1 = new(customer1.CustomerID, product1.ProductID, 20000, DateTime.Now, 2, OrderStatus.Ordered);
            Order order2 = new(customer2.CustomerID, product3.ProductID, 40000, DateTime.Now, 2, OrderStatus.Ordered);
            orders.Add(order1);
            orders.Add(order2);
        }
    }
}