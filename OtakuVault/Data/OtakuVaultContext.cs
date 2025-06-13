using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OtakuVault.Models;

namespace OtakuVault.Data
{
    public class OtakuVaultContext : DbContext
    {
        public OtakuVaultContext (DbContextOptions<OtakuVaultContext> options)
            : base(options)
        {
        }

        public DbSet<OtakuVault.Models.Movie> Movie { get; set; } = default!;
        public DbSet<OtakuVault.Models.Novel> Novel { get; set; } = default!;
        public DbSet<OtakuVault.Models.MediaItem> MediaItem { get; set; } = default!;
    }
}
