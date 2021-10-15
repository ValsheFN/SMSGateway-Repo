using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSGateway.Server.Models
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<ContactGroup> ContactGroups { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<SmsTemplate> SmsTemplates { get; set; }
        public DbSet<TopUp> TopUps { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasMany(p => p.CreatedContact)
                .WithOne(p => p.CreatedByUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasMany(p => p.UpdatedContact)
                .WithOne(p => p.UpdatedByUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasMany(p => p.CreatedContactGroup)
                .WithOne(p => p.CreatedByUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasMany(p => p.UpdatedContactGroup)
                .WithOne(p => p.UpdatedByUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasMany(p => p.CreatedGroup)
                .WithOne(p => p.CreatedByUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasMany(p => p.UpdatedGroup)
                .WithOne(p => p.UpdatedByUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasMany(p => p.CreatedSmsTemplate)
                .WithOne(p => p.CreatedByUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasMany(p => p.UpdatedSmsTemplate)
                .WithOne(p => p.UpdatedByUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasMany(p => p.CreatedTopUp)
                .WithOne(p => p.CreatedByUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasMany(p => p.UpdatedTopUp)
                .WithOne(p => p.UpdatedByUser)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }

    }

    
}
