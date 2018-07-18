﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VipcoQualityControl.Models.QualityControls;

namespace VipcoQualityControl.Migrations
{
    [DbContext(typeof(QualityControlContext))]
    [Migration("20180712072633_updateWelder4")]
    partial class updateWelder4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.Branch", b =>
                {
                    b.Property<int>("BranchId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.HasKey("BranchId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Branch");

                    b.HasData(
                        new { BranchId = 1, Address = "-", Name = "Vipco2" },
                        new { BranchId = 2, Address = "-", Name = "Vipco1" },
                        new { BranchId = 3, Address = "-", Name = "Vipco4" },
                        new { BranchId = 4, Address = "-", Name = "Vipco5" },
                        new { BranchId = 5, Address = "-", Name = "Vipco6" },
                        new { BranchId = 6, Address = "-", Name = "Subcontract" }
                    );
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.InspectionPoint", b =>
                {
                    b.Property<int>("InspectionPointId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.HasKey("InspectionPointId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("InspectionPoint");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.LocationQualityControl", b =>
                {
                    b.Property<int>("LocationQualityControlId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.HasKey("LocationQualityControlId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("LocationQualityControl");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.MasterProjectList", b =>
                {
                    b.Property<int>("MasterProjectListId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Box");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<string>("DrawingNo")
                        .HasMaxLength(200);

                    b.Property<string>("GradeMaterial1")
                        .HasMaxLength(200);

                    b.Property<string>("GradeMaterial2")
                        .HasMaxLength(200);

                    b.Property<double?>("Heigth");

                    b.Property<double?>("JointNumber");

                    b.Property<double?>("Length");

                    b.Property<string>("MarkNo")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<int?>("ProjectCodeDetailId");

                    b.Property<double?>("Quantity");

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("Revised");

                    b.Property<double?>("Thickness");

                    b.Property<string>("TypeMaterial1")
                        .HasMaxLength(200);

                    b.Property<string>("TypeMaterial2")
                        .HasMaxLength(200);

                    b.Property<int?>("UnitNo");

                    b.Property<double?>("Weigth");

                    b.Property<double?>("Width");

                    b.HasKey("MasterProjectListId");

                    b.ToTable("MasterProjectList");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.Permission", b =>
                {
                    b.Property<int>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<int>("LevelPermission");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int>("UserId");

                    b.HasKey("PermissionId");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.QualityControlReason", b =>
                {
                    b.Property<int>("QualityControlReasonId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<string>("Remark")
                        .HasMaxLength(250);

                    b.HasKey("QualityControlReasonId");

                    b.ToTable("QualityControlReason");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.QualityControlResult", b =>
                {
                    b.Property<int>("QualityControlResultId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<string>("EmpCode");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("QualityControlResultDate");

                    b.Property<int>("QualityControlStatus");

                    b.Property<string>("Remark")
                        .HasMaxLength(250);

                    b.Property<int?>("RequireQualityControlId");

                    b.HasKey("QualityControlResultId");

                    b.HasIndex("RequireQualityControlId")
                        .IsUnique()
                        .HasFilter("[RequireQualityControlId] IS NOT NULL");

                    b.ToTable("QualityControlResult");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.QualityControlWelding", b =>
                {
                    b.Property<int>("QualityControlWeldingId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<double?>("FailLength");

                    b.Property<int?>("JointNo");

                    b.Property<string>("MarkNo")
                        .HasMaxLength(100);

                    b.Property<string>("MarkNoPreview")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("ParentQcWeldingId");

                    b.Property<string>("ProcessWeld")
                        .HasMaxLength(100);

                    b.Property<int?>("ProjectCodeMasterId");

                    b.Property<int?>("QualityControlReasonId");

                    b.Property<double?>("Reject");

                    b.Property<string>("Remark")
                        .HasMaxLength(250);

                    b.Property<int?>("RequireQualityControlId");

                    b.Property<string>("ResponseBy");

                    b.Property<double?>("TestLength");

                    b.Property<double?>("Thickness");

                    b.Property<string>("WelderNo")
                        .HasMaxLength(100);

                    b.Property<DateTime>("WeldingDate");

                    b.HasKey("QualityControlWeldingId");

                    b.HasIndex("ParentQcWeldingId");

                    b.HasIndex("QualityControlReasonId");

                    b.HasIndex("RequireQualityControlId");

                    b.ToTable("QualityControlWelding");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.RequireHasAttach", b =>
                {
                    b.Property<int>("RequireHasAttachId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AttachFileId");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("RequireQualityControlId");

                    b.HasKey("RequireHasAttachId");

                    b.HasIndex("RequireQualityControlId");

                    b.ToTable("RequireHasAttach");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.RequireHasMasterProject", b =>
                {
                    b.Property<int>("RequireHasMasterProjectId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<int?>("MasterProjectListId");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<double?>("PassQuantity");

                    b.Property<int?>("QualityControlReasonId");

                    b.Property<double?>("Quantity");

                    b.Property<int?>("RequireQualityControlId");

                    b.HasKey("RequireHasMasterProjectId");

                    b.HasIndex("MasterProjectListId");

                    b.HasIndex("QualityControlReasonId");

                    b.HasIndex("RequireQualityControlId");

                    b.ToTable("RequireHasMasterProject");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.RequireHasWelder", b =>
                {
                    b.Property<int>("RequireHasWelderId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<double>("PercentNDE");

                    b.Property<int?>("QcStatus");

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int>("RequireHasMasterProjectId");

                    b.Property<int?>("VTStaus");

                    b.Property<DateTime?>("WelderDate");

                    b.Property<int>("WelderNo1Id");

                    b.Property<int>("WelderNo2Id");

                    b.Property<int?>("WelderProcess");

                    b.HasKey("RequireHasWelderId");

                    b.HasIndex("RequireHasMasterProjectId")
                        .IsUnique();

                    b.ToTable("RequireHasWelder");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.RequireQcMoreWorkActvity", b =>
                {
                    b.Property<int>("RequireQcMoreWorkActvityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("RequireQualityControlId");

                    b.Property<int?>("WorkActivityId");

                    b.HasKey("RequireQcMoreWorkActvityId");

                    b.HasIndex("RequireQualityControlId");

                    b.HasIndex("WorkActivityId");

                    b.ToTable("RequireQcMoreWorkActvity");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.RequireQualityControl", b =>
                {
                    b.Property<int>("RequireQualityControlId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BranchId");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<string>("GroupMIS");

                    b.Property<int?>("InspectionPointId");

                    b.Property<string>("MailReply")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("ParentRequireQcId");

                    b.Property<int?>("ProjectCodeDetailId");

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<DateTime>("RequireDate");

                    b.Property<string>("RequireEmp");

                    b.Property<string>("RequireQualityNo")
                        .HasMaxLength(50);

                    b.Property<int>("RequireStatus");

                    b.Property<DateTime?>("ResponseDate");

                    b.Property<int?>("WorkActivityId");

                    b.Property<int?>("WorkGroupQualityControlId");

                    b.HasKey("RequireQualityControlId");

                    b.HasIndex("BranchId");

                    b.HasIndex("InspectionPointId");

                    b.HasIndex("ParentRequireQcId");

                    b.HasIndex("WorkActivityId");

                    b.HasIndex("WorkGroupQualityControlId");

                    b.ToTable("RequireQualityControl");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.WelderHasProject", b =>
                {
                    b.Property<int>("WelderHasProjectId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("ProjectCodeMasterId");

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("WelderNoId");

                    b.HasKey("WelderHasProjectId");

                    b.HasIndex("WelderNoId");

                    b.ToTable("WelderHasProject");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.WelderNo", b =>
                {
                    b.Property<int>("WelderNoId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<string>("EmpCode")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ExprireDate");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("RegisterDate");

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<string>("WelderNoCode");

                    b.HasKey("WelderNoId");

                    b.ToTable("WelderNo");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.WorkActivity", b =>
                {
                    b.Property<int>("WorkActivityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int>("TypeWorkActivity");

                    b.HasKey("WorkActivityId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("WorkActivity");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.WorkGroupHasWorkShop", b =>
                {
                    b.Property<int>("WorkGroupHasWorkShopId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("GroupMis");

                    b.Property<int?>("LocationQualityControlId");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.HasKey("WorkGroupHasWorkShopId");

                    b.HasIndex("LocationQualityControlId");

                    b.ToTable("WorkGroupHasWorkShop");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.WorkGroupQualityControl", b =>
                {
                    b.Property<int>("WorkGroupQualityControlId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<string>("Email")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<string>("SubEmail")
                        .HasMaxLength(500);

                    b.HasKey("WorkGroupQualityControlId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("WorkGroupQualityControl");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.WorkQualityControl", b =>
                {
                    b.Property<int>("WorkQualityControlId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<DateTime>("QualityControlDate");

                    b.Property<string>("QualityControlEmp");

                    b.Property<string>("Remark")
                        .HasMaxLength(250);

                    b.Property<int?>("RequireQualityControlId");

                    b.Property<int?>("WorkQcStatus");

                    b.HasKey("WorkQualityControlId");

                    b.HasIndex("RequireQualityControlId")
                        .IsUnique()
                        .HasFilter("[RequireQualityControlId] IS NOT NULL");

                    b.ToTable("WorkQualityControl");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.QualityControlResult", b =>
                {
                    b.HasOne("VipcoQualityControl.Models.QualityControls.RequireQualityControl", "RequireQualityControl")
                        .WithOne("QualityControlResult")
                        .HasForeignKey("VipcoQualityControl.Models.QualityControls.QualityControlResult", "RequireQualityControlId");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.QualityControlWelding", b =>
                {
                    b.HasOne("VipcoQualityControl.Models.QualityControls.QualityControlWelding", "ParentQcWelding")
                        .WithMany()
                        .HasForeignKey("ParentQcWeldingId");

                    b.HasOne("VipcoQualityControl.Models.QualityControls.QualityControlReason", "QualityControlReason")
                        .WithMany()
                        .HasForeignKey("QualityControlReasonId");

                    b.HasOne("VipcoQualityControl.Models.QualityControls.RequireQualityControl", "RequireQualityControl")
                        .WithMany()
                        .HasForeignKey("RequireQualityControlId");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.RequireHasAttach", b =>
                {
                    b.HasOne("VipcoQualityControl.Models.QualityControls.RequireQualityControl", "RequireQualityControl")
                        .WithMany("RequireHasAttaches")
                        .HasForeignKey("RequireQualityControlId");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.RequireHasMasterProject", b =>
                {
                    b.HasOne("VipcoQualityControl.Models.QualityControls.MasterProjectList", "MasterProjectList")
                        .WithMany("RequireHasMasterProjects")
                        .HasForeignKey("MasterProjectListId");

                    b.HasOne("VipcoQualityControl.Models.QualityControls.QualityControlReason", "QualityControlReason")
                        .WithMany("RequireHasMasterProjects")
                        .HasForeignKey("QualityControlReasonId");

                    b.HasOne("VipcoQualityControl.Models.QualityControls.RequireQualityControl", "RequireQualityControl")
                        .WithMany("RequireHasMasterProjects")
                        .HasForeignKey("RequireQualityControlId");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.RequireHasWelder", b =>
                {
                    b.HasOne("VipcoQualityControl.Models.QualityControls.RequireHasMasterProject", "RequireHasMasterProject")
                        .WithOne("RequireHasWelder")
                        .HasForeignKey("VipcoQualityControl.Models.QualityControls.RequireHasWelder", "RequireHasMasterProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.RequireQcMoreWorkActvity", b =>
                {
                    b.HasOne("VipcoQualityControl.Models.QualityControls.RequireQualityControl", "RequireQualityControl")
                        .WithMany("RequireQcMoreWorkActvities")
                        .HasForeignKey("RequireQualityControlId");

                    b.HasOne("VipcoQualityControl.Models.QualityControls.WorkActivity", "WorkActivity")
                        .WithMany()
                        .HasForeignKey("WorkActivityId");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.RequireQualityControl", b =>
                {
                    b.HasOne("VipcoQualityControl.Models.QualityControls.Branch", "Branch")
                        .WithMany("RequireQualityControls")
                        .HasForeignKey("BranchId");

                    b.HasOne("VipcoQualityControl.Models.QualityControls.InspectionPoint", "InspectionPoint")
                        .WithMany("RequireQualityControls")
                        .HasForeignKey("InspectionPointId");

                    b.HasOne("VipcoQualityControl.Models.QualityControls.RequireQualityControl", "ParentRequireQc")
                        .WithMany()
                        .HasForeignKey("ParentRequireQcId");

                    b.HasOne("VipcoQualityControl.Models.QualityControls.WorkActivity", "WorkActivity")
                        .WithMany("RequireQualityControls")
                        .HasForeignKey("WorkActivityId");

                    b.HasOne("VipcoQualityControl.Models.QualityControls.WorkGroupQualityControl", "WorkGroupQualityControl")
                        .WithMany("RequireQualityControls")
                        .HasForeignKey("WorkGroupQualityControlId");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.WelderHasProject", b =>
                {
                    b.HasOne("VipcoQualityControl.Models.QualityControls.WelderNo", "WelderNo")
                        .WithMany()
                        .HasForeignKey("WelderNoId");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.WorkGroupHasWorkShop", b =>
                {
                    b.HasOne("VipcoQualityControl.Models.QualityControls.LocationQualityControl", "LocationQualityControl")
                        .WithMany("WorkGroupHasWorkShops")
                        .HasForeignKey("LocationQualityControlId");
                });

            modelBuilder.Entity("VipcoQualityControl.Models.QualityControls.WorkQualityControl", b =>
                {
                    b.HasOne("VipcoQualityControl.Models.QualityControls.RequireQualityControl", "RequireQualityControl")
                        .WithOne("WorkQualityControl")
                        .HasForeignKey("VipcoQualityControl.Models.QualityControls.WorkQualityControl", "RequireQualityControlId");
                });
#pragma warning restore 612, 618
        }
    }
}
