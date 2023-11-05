using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAdoNet
{
    public class TeacherContext : DbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-RG5G9U4;Initial Catalog=SMS;Integrated Security=True; TrustServerCertificate=True");
        }
        /*public TeacherContext(DbContextOptions<TeacherContext> options)
            : base(options)
        {
            
        }*/
    }
}
