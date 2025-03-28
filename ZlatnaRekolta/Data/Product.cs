using System.ComponentModel.DataAnnotations;

namespace ZlatnaRekolta.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public Category Categories { get; set; }

        public int OriginID {  get; set; }
        public Origin Origins { get; set; }

        public int DistributorID { get; set; }
        public Distributor Distributors { get; set; }
        public string Description { get; set; }
        public string URLimage {  get; set; }
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:F3}")]
        public decimal Quantity {  get; set; }
       
        public UnitOfMeasure UnitOfMe { get; set; }


        public DateTime DateRegister { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Basket> Baskets { get; set; }
        
    }
}
