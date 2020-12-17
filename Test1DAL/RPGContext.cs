using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Test1DAL.Models;
namespace Test1DAL
{

    public class RPGContext : DbContext
    {
       public RPGContext(DbContextOptions<RPGContext> options) : base(options) { }
       public DbSet<Item> Items { get; set; }
    }
}
