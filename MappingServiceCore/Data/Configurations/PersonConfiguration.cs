using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MappingServiceCore.Models.Entities;

namespace MappingServiceCore.Data.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                   .HasMaxLength(30);

            builder.Property(p => p.LastName)
                   .HasMaxLength(50);

            builder.Property(p => p.PhoneNumber)
                   .HasMaxLength(11)
                   .IsRequired();

            builder.Property(p => p.CreateDate)
                   .HasDefaultValueSql("GETDATE()")
                   .ValueGeneratedOnAdd();
        }
    }
}