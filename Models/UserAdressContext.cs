using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NRH_UserAdress.Models;

namespace NRH_UserAdress.Models
{
    public class UserAdressContext : DbContext
    {
        public UserAdressContext(DbContextOptions<UserAdressContext> options)
            : base(options)
        {
        }

        public DbSet<Pays> Pays { get; set; } = null!;
        public DbSet<Province> Province { get; set; } = null!;
        public DbSet<Ville> Ville { get; set; } = null!;
        public DbSet<Account> Account { get; set; } = null!;

    }
}
