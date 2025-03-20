using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class GreetingAppContext : DbContext
    {
        public GreetingAppContext(DbContextOptions<GreetingAppContext> options) : base(options) 
        {
        
        }
        public virtual DbSet<MessageEntity> GreetingMessage { get; set; }

        public virtual DbSet<UserEntity> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageEntity>()
                .HasOne(a => a.User)
                .WithMany(u => u.MessageEntries)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
