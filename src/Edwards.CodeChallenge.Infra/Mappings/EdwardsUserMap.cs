using Edwards.CodeChallenge.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edwards.CodeChallenge.Infra.Mappings;

    public class EdwardsUserMap : IEntityTypeConfiguration<EdwardsUser>
    {
        public void Configure(EntityTypeBuilder<EdwardsUser> builder)
        {
            builder.ToTable("User");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                 .HasColumnType("TEXT")
                 .HasMaxLength(36)
                 .IsRequired();

            builder.Property(x => x.FirstName)
                .HasColumnType("TEXT")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasColumnType("TEXT")
                .HasMaxLength(30);


            builder.Property(x => x.Email)
                .HasColumnType("TEXT")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasColumnType("TEXT")
                .HasMaxLength(100);


            builder.Property(x => x.DateCreated)
                .HasColumnType("datetime")
                .IsRequired();


        }
    }

