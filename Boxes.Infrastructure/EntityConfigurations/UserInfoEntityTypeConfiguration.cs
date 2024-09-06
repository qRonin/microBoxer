using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boxes.Domain.AggregatesModel.UserAggregate;

namespace Boxes.Infrastructure.EntityConfigurations;

internal class UserInfoEntityTypeConfiguration
: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> userConfiguration)
    {
        userConfiguration.ToTable("users");

        userConfiguration.Ignore(b => b.DomainEvents);

        userConfiguration.Property(b => b.Id)
            .HasMaxLength(200);
        userConfiguration.HasIndex("Id")
            .IsUnique(true);

        userConfiguration.HasMany(b => b.UserBoxes)
            .WithOne();
        userConfiguration.HasMany(b => b.UserBoxContents)
            .WithOne();
    }
}