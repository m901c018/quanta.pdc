﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using cns.Data;

namespace cns.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210302134215_AddForm_Col_FormStatusCode")]
    partial class AddForm_Col_FormStatusCode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("cns.Models.PDC_File", b =>
                {
                    b.Property<long>("FileID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<DateTime>("CreatorDate");

                    b.Property<string>("CreatorName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<int>("FileCategory");

                    b.Property<string>("FileDescription");

                    b.Property<string>("FileExtension")
                        .HasMaxLength(10);

                    b.Property<string>("FileFullName")
                        .HasMaxLength(256);

                    b.Property<string>("FileName")
                        .HasMaxLength(128);

                    b.Property<string>("FileNote");

                    b.Property<string>("FileRemark");

                    b.Property<long>("FileSize");

                    b.Property<int>("FileType");

                    b.Property<string>("FunctionName")
                        .IsRequired();

                    b.Property<long>("SourceID");

                    b.HasKey("FileID");

                    b.ToTable("PDC_File");
                });

            modelBuilder.Entity("cns.Models.PDC_Form", b =>
                {
                    b.Property<long>("FormID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppliedFormNo")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("ApplierID")
                        .IsRequired();

                    b.Property<DateTime>("ApplyDate");

                    b.Property<string>("BUCode")
                        .HasMaxLength(32);

                    b.Property<string>("BoardTypeName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("CompCode")
                        .HasMaxLength(32);

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<DateTime>("CreatorDate");

                    b.Property<string>("CreatorName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("FormStatus")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<int>("FormStatusCode");

                    b.Property<bool>("IsMB");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(128);

                    b.Property<DateTime?>("ModifyerDate");

                    b.Property<string>("ModifyerName")
                        .HasMaxLength(128);

                    b.Property<string>("PCBLayoutStatus")
                        .HasMaxLength(32);

                    b.Property<string>("PCBType");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Result");

                    b.Property<string>("Revision")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("FormID");

                    b.ToTable("PDC_Form");
                });

            modelBuilder.Entity("cns.Models.PDC_Form_StageLog", b =>
                {
                    b.Property<long>("StageLogID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<DateTime>("CreatorDate");

                    b.Property<string>("CreatorName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<long>("FormID");

                    b.Property<string>("PDC_Member");

                    b.Property<string>("Result");

                    b.Property<int>("Stage");

                    b.Property<string>("StageName")
                        .HasMaxLength(128);

                    b.Property<decimal>("WorkHour");

                    b.HasKey("StageLogID");

                    b.ToTable("PDC_Form_StageLog");
                });

            modelBuilder.Entity("cns.Models.PDC_Parameter", b =>
                {
                    b.Property<long>("ParameterID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<DateTime>("CreatorDate");

                    b.Property<string>("CreatorName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<bool>("IsSync");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(128);

                    b.Property<DateTime?>("ModifyerDate");

                    b.Property<string>("ModifyerName")
                        .HasMaxLength(128);

                    b.Property<int>("OrderNo");

                    b.Property<string>("ParameterDesc");

                    b.Property<string>("ParameterGroup");

                    b.Property<string>("ParameterName")
                        .HasMaxLength(256);

                    b.Property<string>("ParameterNote");

                    b.Property<long>("ParameterParentID");

                    b.Property<string>("ParameterText")
                        .HasMaxLength(256);

                    b.Property<string>("ParameterType");

                    b.Property<string>("ParameterValue");

                    b.HasKey("ParameterID");

                    b.ToTable("PDC_Parameter");
                });

            modelBuilder.Entity("cns.Models.PDC_StackupColumn", b =>
                {
                    b.Property<long>("StackupColumnID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColumnCode")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("ColumnName")
                        .HasMaxLength(128);

                    b.Property<string>("ColumnType")
                        .HasMaxLength(64);

                    b.Property<string>("ColumnValue");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<DateTime>("CreatorDate");

                    b.Property<string>("CreatorName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("DataType")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<int>("DecimalPlaces");

                    b.Property<int>("MaxLength");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(128);

                    b.Property<DateTime?>("ModifyerDate");

                    b.Property<string>("ModifyerName")
                        .HasMaxLength(128);

                    b.Property<int>("OrderNo");

                    b.Property<long>("ParentColumnID");

                    b.HasKey("StackupColumnID");

                    b.ToTable("PDC_StackupColumn");
                });

            modelBuilder.Entity("cns.Models.PDC_StackupDetail", b =>
                {
                    b.Property<long>("StackupDetailID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColumnType")
                        .HasMaxLength(64);

                    b.Property<string>("ColumnValue");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<DateTime>("CreatorDate");

                    b.Property<string>("CreatorName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("DataType")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<int>("IndexNo");

                    b.Property<bool>("IsDisabled");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(128);

                    b.Property<DateTime?>("ModifyerDate");

                    b.Property<string>("ModifyerName")
                        .HasMaxLength(128);

                    b.Property<long>("StackupColumnID");

                    b.HasKey("StackupDetailID");

                    b.ToTable("PDC_StackupDetail");
                });

            modelBuilder.Entity("cns.Models.Todo", b =>
                {
                    b.Property<string>("TodoId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsDone");

                    b.Property<string>("TodoItem")
                        .IsRequired();

                    b.HasKey("TodoId");

                    b.ToTable("Todo");
                });

            modelBuilder.Entity("cns.Models.vw_FormQuery", b =>
                {
                    b.Property<long>("FormID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppliedFormNo");

                    b.Property<string>("ApplierID");

                    b.Property<DateTime>("ApplyDate");

                    b.Property<string>("ApplyOrder");

                    b.Property<string>("BUCode");

                    b.Property<string>("BoardTypeName");

                    b.Property<string>("CompCode");

                    b.Property<string>("Creator");

                    b.Property<DateTime>("CreatorDate");

                    b.Property<string>("CreatorName");

                    b.Property<string>("FormStatus");

                    b.Property<bool>("IsMB");

                    b.Property<string>("Modifyer");

                    b.Property<DateTime?>("ModifyerDate");

                    b.Property<string>("ModifyerName");

                    b.Property<string>("PCBLayoutStatus");

                    b.Property<string>("PCBType");

                    b.Property<string>("PDC_Member");

                    b.Property<string>("ProjectName");

                    b.Property<string>("Revision");

                    b.Property<DateTime?>("StageDate");

                    b.Property<string>("StageName");

                    b.HasKey("FormID");

                    b.ToTable("vw_FormQuery");
                });

            modelBuilder.Entity("cns.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<bool>("isSuperAdmin");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
