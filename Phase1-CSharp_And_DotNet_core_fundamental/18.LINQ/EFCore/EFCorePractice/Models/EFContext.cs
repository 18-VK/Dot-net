using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCorePractice.Models
{
    public class EFContext : DbContext
    {
        //  "Server=192.168.1.100;Database=MyDatabase;Trusted_Connection=True;";
        string ConnectionString = "Server=LAPTOP-0DCLBA10\\SQLEXPRESS;Database=SysDBPractice;Trusted_Connection=True;Encrypt=False;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
        public DbSet<Product> Products { get; set; }
    }
}
