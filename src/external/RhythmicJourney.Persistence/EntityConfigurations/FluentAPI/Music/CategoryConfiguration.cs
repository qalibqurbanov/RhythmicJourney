using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Core.Entities.Music;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RhythmicJourney.Persistence.EntityConfigurations.FluentAPI.Music;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasKey(category => category.Id);

        builder
            .HasMany(category => category.Songs)
            .WithOne(song => song.Category)
            .HasForeignKey(song => song.CategoryId);
    }
}