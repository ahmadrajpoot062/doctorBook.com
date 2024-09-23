using doctorBook.com.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace doctorBook.com.Data
{
    public class ApplicationDbContext : IdentityDbContext<myAppUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
