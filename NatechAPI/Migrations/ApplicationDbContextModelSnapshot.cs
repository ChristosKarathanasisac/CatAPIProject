﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NatechAPI.Data;

#nullable disable

namespace NatechAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NatechAPI.Models.Entities.CatEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CatId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Height")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CatId")
                        .IsUnique();

                    b.ToTable("Cats");
                });

            modelBuilder.Entity("NatechAPI.Models.Entities.CatTag", b =>
                {
                    b.Property<Guid>("CatEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TagEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CatEntityId", "TagEntityId");

                    b.HasIndex("TagEntityId");

                    b.ToTable("CatTags");
                });

            modelBuilder.Entity("NatechAPI.Models.Entities.TagEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("NatechAPI.Models.Entities.CatTag", b =>
                {
                    b.HasOne("NatechAPI.Models.Entities.CatEntity", "CatEntity")
                        .WithMany("CatTags")
                        .HasForeignKey("CatEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NatechAPI.Models.Entities.TagEntity", "TagEntity")
                        .WithMany("CatTags")
                        .HasForeignKey("TagEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CatEntity");

                    b.Navigation("TagEntity");
                });

            modelBuilder.Entity("NatechAPI.Models.Entities.CatEntity", b =>
                {
                    b.Navigation("CatTags");
                });

            modelBuilder.Entity("NatechAPI.Models.Entities.TagEntity", b =>
                {
                    b.Navigation("CatTags");
                });
#pragma warning restore 612, 618
        }
    }
}
