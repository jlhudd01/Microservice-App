namespace OrderWebAPI.Models
{
    public class OrderItem : Entity
    {
        public int Id {get; set;}
        public int ProductId {get;  set;}
        public string Name {get; set;}
        public decimal Price {get; set;}

        public OrderItem() {}

        public OrderItem(int productid, string name, decimal price)
        {
            ProductId = productid;
            Name = name;
            Price = price;
        }

        public void UpdateOrderItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }
}