using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlinePharmacy.Models;

namespace OnlinePharmacy.DAL
{
    public class DBConn : IdentityDbContext <AppUser>
    {
        public DBConn(DbContextOptions<DBConn> options) : base(options)
        {
        }

        
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<PatientInfo> Patients { get; set; }
       public DbSet<MedicineRequest> Requests {  get; set; }


    }
}
