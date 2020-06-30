﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ResetV7.Models;

namespace ResetV7.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ResetV7.Models.LogType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LogType");
                });

            modelBuilder.Entity("ResetV7.Models.ResetLog", b =>
                {
                    b.Property<Guid>("ResetID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Ip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LogTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("bizUser")
                        .HasColumnType("bit");

                    b.Property<int>("countForgot")
                        .HasColumnType("int");

                    b.Property<int>("countOTP")
                        .HasColumnType("int");

                    b.Property<int>("countReset")
                        .HasColumnType("int");

                    b.Property<bool>("eduUser")
                        .HasColumnType("bit");

                    b.Property<DateTime>("logTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("sessionToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sessionTokenCheck")
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ResetID");

                    b.HasIndex("LogTypeId");

                    b.ToTable("ResetLog");
                });

            modelBuilder.Entity("ResetV7.Models.ResetLog", b =>
                {
                    b.HasOne("ResetV7.Models.LogType", "LogType")
                        .WithMany()
                        .HasForeignKey("LogTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
