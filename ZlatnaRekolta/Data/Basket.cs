using System.ComponentModel.DataAnnotations;

namespace ZlatnaRekolta.Data
{
    public class Basket
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User Users { get; set; }
        public int ProductId { get; set; }
        public Product Products { get; set; }

        [DisplayFormat(DataFormatString = "{0:F3}")]
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public DateTime RegisterOn { get; set; }
    }
}
