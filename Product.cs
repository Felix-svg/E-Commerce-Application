namespace E_CommerceApplication
{
    public class Product
    {
        private static int s_productID = 2000;
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public int ShippingDuration { get; set; }

        public Product(string productName, int stock, int price, int shippingDuration)
        {
            ProductID = $"PID{s_productID++}";
            ProductName = productName;
            Stock = stock;
            Price = price;
            ShippingDuration = shippingDuration;
        }
    }
}