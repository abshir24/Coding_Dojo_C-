using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DojoConnections.Models;

namespace DojoConnections.Models
{
    public class DojoConnectionsContext : DbContext
    {
        public DojoConnectionsContext(DbContextOptions<DojoConnectionsContext> options) : base(options) {}
        public DbSet<User> User { get; set; }
        public DbSet<Network> Network { get; set; }
        public DbSet<Invitation> Invitation { get; set; }
    }
}