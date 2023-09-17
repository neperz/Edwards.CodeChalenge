using Edwards.CodeChallenge.Domain.Interfaces;
using Edwards.CodeChallenge.Domain.Models;
using Edwards.CodeChallenge.Infra.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Edwards.CodeChallenge.Infra.Context;

    public class EntityContext : DbContext
    {
        private readonly IFileService _fileService;
        public EntityContext(DbContextOptions<EntityContext> options, IFileService fileService)
             : base(options) {
            _fileService=fileService;
        }

        public DbSet<EdwardsUser> EdwardsUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EdwardsUserMap());
        }

        public override int SaveChanges()
        {
            int saveResult = 0;
            foreach (var entry in ChangeTracker.Entries().Where(entity => entity.Entity.GetType().GetProperty("DateCreated") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DateCreated").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DateCreated").IsModified = false;
                }
            }
            saveResult= base.SaveChanges();
            // TODO: When users are updated/deleted etc the collection should be stored to disk
            var dataToSave = EdwardsUsers.ToList();
            _fileService.DumpDataToDisk(dataToSave);

            return saveResult;

            
        }
    }

