﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Contexts
{
    public class CatalogIdentityContext: IdentityDbContext<IdentityUser>
    {
        public CatalogIdentityContext(DbContextOptions<CatalogIdentityContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
   
}
