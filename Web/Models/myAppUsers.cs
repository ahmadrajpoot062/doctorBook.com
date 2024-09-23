using Microsoft.AspNetCore.Identity;

namespace doctorBook.com.Models
{
    public class myAppUsers:IdentityUser
    {
        public string First_Name {  get; set; }
        public string Last_Name { get; set;}
        public string Role { get; set;}
    }
}
