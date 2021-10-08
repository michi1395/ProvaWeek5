using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProvaWeek5.EF.Configuration;
using ProvaWeek5.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaWeek5.EF
{
    public class SpesaContext:DbContext
    {
        public SpesaContext() : base() { }

        public SpesaContext(DbContextOptions<SpesaContext> options)
            : base(options) { }

        public DbSet<Spesa> Spese { get; set; }
        public DbSet<Categorie> Categorie { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionStringSQL = config.GetConnectionString("AcademyG");
                optionsBuilder.UseSqlServer(connectionStringSQL);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SpesaConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
        }
    }
}
