using JNUE_ADAPI.Models;
using System.Data.Entity;

namespace JNUE_ADAPI.DAL
{
    public class HaksaContext : DbContext
    {
        public HaksaContext() : base("HaksaContext") { }

        public DbSet<UniversityMember> HaksaMembers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}