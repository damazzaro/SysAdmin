using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SysAdmin.Models
{
    public partial class SocialFilesContext : DbContext
    {
        public virtual DbSet<Files> Files { get; set; }

        public SocialFilesContext(DbContextOptions<SocialFilesContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Files>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.Property(e => e.FileId).HasColumnName("file_id");

                entity.Property(e => e.FileDescription)
                    .HasColumnName("file_description")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("file_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FileVerified).HasColumnName("file_verified");

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasColumnName("user_email")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
