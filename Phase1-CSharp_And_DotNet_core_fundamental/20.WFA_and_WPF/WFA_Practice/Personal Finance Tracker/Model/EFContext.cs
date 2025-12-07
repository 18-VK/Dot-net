using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Personal_Finance_Tracker.Model
{
    internal class EFContext : DbContext
    {
        string ConnectionString = "Server=LAPTOP-0DCLBA10\\SQLEXPRESS;Database=SysDBPractice;Trusted_Connection=True;Encrypt=False;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

    }
}
