﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.API.Models;

namespace User.API.Data
{
    public class UserContext:DbContext
    {
        public  UserContext(DbContextOptions<UserContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AppUser>()
                .ToTable("Users")
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserProperty>().Property(u => u.Key).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>().Property(u => u.Value).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>()
                .ToTable("UserPropertes")
                .HasKey(u => new {u.Key,u.Value,u.AppUserId });

            modelBuilder.Entity<UserTag>().Property(u=>u.Tag).HasMaxLength(100);
            modelBuilder.Entity<UserTag>()
                .ToTable("UserTags")
                .HasKey(u => new { u.UserId,u.Tag});

            modelBuilder.Entity<BPFile>()
                .ToTable("BPFiles")
                .HasKey(u => u.Id);


            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>().Property(u => u.Id).ValueGeneratedOnAdd();

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserProperty> UserProperties { get; set; }
        public DbSet<UserTag> UserTags { get; set; }
        //public DbSet<AppUser> Users { get; set; }
    }
}
