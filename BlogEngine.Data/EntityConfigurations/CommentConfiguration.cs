using System;
using System.Collections.Generic;
using System.Text;
using BlogEngine.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogEngine.Data.EntityConfigurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CommentContent)
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnType("varchar");
            builder.Property(p => p.DateCommented)
                .IsRequired();
            builder.Property(p => p.AuthorId)
                .IsRequired()
                .HasColumnName("AuthorId");
        }
    }
}
