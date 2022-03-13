using BlogEngine.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogEngine.Data.EntityConfigurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PostContent)
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnType("varchar");
            builder.Property(p => p.DatePublished)
                .IsRequired();
            builder.Property(p => p.AuthorId)
                .IsRequired()
                .HasColumnName("AuthorId");
        }
    }
}
