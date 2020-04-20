using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_Framework.Models;

namespace Test_Framework
{
    public class TestContext : DbContext
    {

        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {
        }

        public DbSet<ApiModel> Apis { get; set; }
        public DbSet<TestModel> Tests { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}

    }
}
