using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using RSS.CompleteApp.ViewModels;

namespace RSS.CompleteApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RSS.CompleteApp.ViewModels.ProductViewModel> ProductViewModel { get; set; }
        public DbSet<RSS.CompleteApp.ViewModels.SupplierViewModel> SupplierViewModel { get; set; }
    }
}
