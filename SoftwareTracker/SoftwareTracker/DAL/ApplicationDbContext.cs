using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using EvidencijaSoftvera_IlijaDivljan.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EvidencijaSoftvera_IlijaDivljan.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()//: base("ApplicationDbContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Computers> Computers { get; set; }
        public DbSet<Software> Software { get; set; }
        public DbSet<InstalledSoftware> InstalledSoftware { get; set; }
        public DbSet<AdditionalEquipment> AdditionalEquipment { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Computers>()
                .HasMany(c => c.AdditionalEquipment).WithMany(a => a.Computers)
                .Map(t =>t.MapLeftKey("ComputersId")
                            .MapRightKey("AdditionalEquipmentId")
                            .ToTable("ComputersAdditionalEquipment"));


            modelBuilder.Entity<AdditionalEquipment>().MapToStoredProcedures();
        }
    }
}