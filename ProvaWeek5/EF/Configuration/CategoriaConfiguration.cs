using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProvaWeek5.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaWeek5.EF.Configuration
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categorie>
    {
        public void Configure(EntityTypeBuilder<Categorie> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Categoria)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .HasMany(s => s.Spese)
                .WithOne(c => c.Categoria);
        }
    }
}
