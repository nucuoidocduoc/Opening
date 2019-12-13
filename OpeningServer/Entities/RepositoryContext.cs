using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Drawing> Drawings { get; set; }
        //public DbSet<Element> Elements { get; set; }
        //public DbSet<ElementManagement> ElementManagements { get; set; }
        //public DbSet<GeometryVersion> GeometryVersions { get; set; }
        //public DbSet<Revision> Revisions { get; set; }
        //public DbSet<CheckoutVersion> CheckoutVersions { get; set; }
    }
}