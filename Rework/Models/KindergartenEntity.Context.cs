﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rework.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class kindergartenEntities : DbContext
    {
        public kindergartenEntities()
            : base("name=kindergartenEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<child> children { get; set; }
        public virtual DbSet<@class> classes { get; set; }
        public virtual DbSet<condition> conditions { get; set; }
        public virtual DbSet<grade> grades { get; set; }
        public virtual DbSet<parent> parents { get; set; }
        public virtual DbSet<regulation> regulations { get; set; }
        public virtual DbSet<report> reports { get; set; }
        public virtual DbSet<teacher> teachers { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
