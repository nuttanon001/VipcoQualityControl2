using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VipcoQualityControl.ViewModels;

namespace VipcoQualityControl.Models.QualityControls
{
    public class QualityControlContext : DbContext
    {
        public QualityControlContext(DbContextOptions<QualityControlContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>().ToTable("Branch")
                .HasIndex(b => b.Name).IsUnique();
            modelBuilder.Entity<Branch>().HasData(
                new { BranchId = 1, Name = "Vipco2", Address = "-" },
                new { BranchId = 2, Name = "Vipco1", Address = "-" },
                new { BranchId = 3, Name = "Vipco4", Address = "-" },
                new { BranchId = 4, Name = "Vipco5", Address = "-" },
                new { BranchId = 5, Name = "Vipco6", Address = "-" },
                new { BranchId = 6, Name = "Subcontract", Address = "-" });
            modelBuilder.Entity<DashBoardDaily>().ToTable("DashBoardDaily");
            modelBuilder.Entity<InspectionPoint>().ToTable("InspectionPoint")
                .HasIndex(i => i.Name).IsUnique();
            modelBuilder.Entity<LocationQualityControl>().ToTable("LocationQualityControl")
                .HasIndex(l => l.Name).IsUnique();
            modelBuilder.Entity<MasterProjectList>().ToTable("MasterProjectList");
            modelBuilder.Entity<Permission>().ToTable("Permission");
            modelBuilder.Entity<QualityControlResult>().ToTable("QualityControlResult");
            modelBuilder.Entity<QualityControlWelding>().ToTable("QualityControlWelding");
            modelBuilder.Entity<RequireHasAttach>().ToTable("RequireHasAttach");
            modelBuilder.Entity<RequireHasMasterProject>().ToTable("RequireHasMasterProject");
            modelBuilder.Entity<RequireHasWelder>().ToTable("RequireHasWelder");
            modelBuilder.Entity<RequireQcMoreWorkActvity>().ToTable("RequireQcMoreWorkActvity");
            modelBuilder.Entity<RequireQualityControl>().ToTable("RequireQualityControl");
            modelBuilder.Entity<WelderHasProject>().ToTable("WelderHasProject");
            modelBuilder.Entity<WelderNo>().ToTable("WelderNo");
            modelBuilder.Entity<WorkActivity>().ToTable("WorkActivity")
                .HasIndex(w => w.Name).IsUnique();
            modelBuilder.Entity<WorkGroupHasWorkShop>().ToTable("WorkGroupHasWorkShop");
            modelBuilder.Entity<WorkGroupQualityControl>().ToTable("WorkGroupQualityControl")
                .HasIndex(w => w.Name).IsUnique();
            modelBuilder.Entity<WorkQualityControl>().ToTable("WorkQualityControl");
            //View
            modelBuilder.Query<WelderNoViewModel>().ToView("View_WelderNo_Employee");
            modelBuilder.Query<WelderHasProjectViewModel>().ToView("View_WelderNo_Project_Employee");

        }

        #region Entities
        // Dbset
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<DashBoardDaily> DashBoardDailies { get; set; }
        public DbSet<InspectionPoint> InspectionPoints { get; set; }
        public DbSet<LocationQualityControl> LocationQualityControls { get; set; }
        public DbSet<MasterProjectList> MasterProjectLists { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<QualityControlResult> QualityControlResults { get; set; }
        public DbSet<QualityControlWelding> QualityControlWeldings { get; set; }
        public DbSet<RequireHasAttach> RequireHasAttachs { get; set; }
        public DbSet<RequireHasMasterProject> RequireHasMasterProjects { get; set; }
        public DbSet<RequireHasWelder> RequireHasWelders { get; set; }
        public DbSet<RequireQcMoreWorkActvity> RequireQcMoreWorkActvities { get; set; }
        public DbSet<RequireQualityControl> RequireQualityControls { get; set; }
        public DbSet<WelderHasProject> WelderHasProjects { get; set; }
        public DbSet<WelderNo> WelderNos { get; set; }
        public DbSet<WorkActivity> WorkActivities { get; set; }
        public DbSet<WorkGroupHasWorkShop> WorkGroupHasWorkShop { get; set; }
        public DbSet<WorkGroupQualityControl> WorkGroupQualityControls { get; set; }
        public DbSet<WorkQualityControl> WorkQualityControls { get; set; }
        #endregion

        #region QueryType
        public DbQuery<WelderNoViewModel> WelderNoViewModes { get; set; }
        public DbQuery<WelderHasProjectViewModel> WelderHasProjectViewModel { get; set; }

        #endregion
    }
}
