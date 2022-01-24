using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.Models;

namespace TaskPlanner.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // Employee == Model, Employees == databasetable
        public DbSet<Employee> Employees { get; set; }

        public DbSet<WorkTask> Tasks { get; set; }

        public DbSet<Shift> Shifts { get; set; }
    }
}
