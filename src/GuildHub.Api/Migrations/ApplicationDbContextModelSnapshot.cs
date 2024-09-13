﻿// <auto-generated />
using System;
using GuildHub.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GuildHub.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GuildHub.Api.PostReplies.PostReply", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text")
                        .HasColumnName("ImagePath");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Message");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("PostReplies", (string)null);
                });

            modelBuilder.Entity("GuildHub.Api.Posts.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text")
                        .HasColumnName("Content");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text")
                        .HasColumnName("ImagePath");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Title");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("Posts", (string)null);
                });

            modelBuilder.Entity("GuildHub.Api.PostReplies.PostReply", b =>
                {
                    b.HasOne("GuildHub.Api.Posts.Post", "Post")
                        .WithMany("PostReplies")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("GuildHub.Api.Posts.Post", b =>
                {
                    b.Navigation("PostReplies");
                });
#pragma warning restore 612, 618
        }
    }
}
