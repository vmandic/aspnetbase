﻿// <auto-generated />
using System;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AspNetBase.Infrastructure.DbMigrations.SqliteMigrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190106170729_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.Property<Guid>("Uid");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("AppRole");
                });

            modelBuilder.Entity("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.Property<Guid>("Uid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("AppRoleClaim");
                });

            modelBuilder.Entity("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

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

                    b.Property<Guid>("Uid");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("AppUser");
                });

            modelBuilder.Entity("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("Uid");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("AppUserClaim");
                });

            modelBuilder.Entity("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUserLogin", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("Uid");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("AppUserLogin");
                });

            modelBuilder.Entity("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Uid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("AppUserRole");
                });

            modelBuilder.Entity("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUserToken", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Uid");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("AppUserToken");
                });

            modelBuilder.Entity("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppRoleClaim", b =>
                {
                    b.HasOne("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUserClaim", b =>
                {
                    b.HasOne("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUserLogin", b =>
                {
                    b.HasOne("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUserRole", b =>
                {
                    b.HasOne("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUserToken", b =>
                {
                    b.HasOne("AspNetBase.Infrastructure.DataAccess.Entities.Identity.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
