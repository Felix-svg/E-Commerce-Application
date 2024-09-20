using System;

namespace E_CommerceApplication
{
    public enum OrderStatus
    {
        Default,
        Ordered,
        Cancelled
    }

    public class Order
    {
        private static int s_orderID = 1000;
        public string OrderID { get; set; }
        public string CustomerID { get; set; }
        public string ProductID { get; set; }
        public int TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Quantity { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public Order(string customerID, string productID, int totalPrice, DateTime purchaseDate, int quantity, OrderStatus orderStatus)
        {
            OrderID = $"OID{s_orderID++}";
            CustomerID = customerID;
            ProductID = productID;
            TotalPrice = totalPrice;
            PurchaseDate = purchaseDate;
            Quantity = quantity;
            OrderStatus = orderStatus;
        }
    }
}