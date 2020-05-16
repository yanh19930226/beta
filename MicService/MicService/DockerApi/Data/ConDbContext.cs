using DockerApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockerApi.Data
{
    public class ConDbContext:DbContext
    {
        public ConDbContext(DbContextOptions<ConDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasKey(u => u.Id);
        }

        public DbSet<User> Users { get; set; }
    }
}
