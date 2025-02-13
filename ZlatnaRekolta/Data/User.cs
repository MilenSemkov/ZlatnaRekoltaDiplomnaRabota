using Microsoft.AspNetCore.Identity;

namespace ZlatnaRekolta.Data
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Basket> Baskets { get; set; }
    }
}
