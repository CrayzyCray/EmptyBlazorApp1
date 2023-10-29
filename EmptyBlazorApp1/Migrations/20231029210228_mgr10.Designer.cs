﻿// <auto-generated />
using System;
using EmptyBlazorApp1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmptyBlazorApp1.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231029210228_mgr10")]
    partial class mgr10
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.13");

            modelBuilder.Entity("CommunityUser", b =>
                {
                    b.Property<int>("CommunitiesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MembersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CommunitiesId", "MembersId");

                    b.HasIndex("MembersId");

                    b.ToTable("CommunityUser");
                });

            modelBuilder.Entity("EmptyBlazorApp1.Entities.Community", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CreatorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SocialNetworkLink")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Communities");
                });

            modelBuilder.Entity("EmptyBlazorApp1.Entities.CommunityHashTags", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CommunityId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CommunityId");

                    b.ToTable("HashTags");
                });

            modelBuilder.Entity("EmptyBlazorApp1.Entities.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("SessionId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("EmptyBlazorApp1.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanCreateCommunity")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EmptyBlazorApp1.Entities.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("TEXT");

                    b.Property<string>("Course")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SocialNetworkLink")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("University")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserProfile");
                });

            modelBuilder.Entity("CommunityUser", b =>
                {
                    b.HasOne("EmptyBlazorApp1.Entities.Community", null)
                        .WithMany()
                        .HasForeignKey("CommunitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EmptyBlazorApp1.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EmptyBlazorApp1.Entities.Community", b =>
                {
                    b.HasOne("EmptyBlazorApp1.Entities.User", "Creator")
                        .WithMany("CreatedCommunities")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("EmptyBlazorApp1.Entities.CommunityHashTags", b =>
                {
                    b.HasOne("EmptyBlazorApp1.Entities.Community", null)
                        .WithMany("HashTags")
                        .HasForeignKey("CommunityId");
                });

            modelBuilder.Entity("EmptyBlazorApp1.Entities.Session", b =>
                {
                    b.HasOne("EmptyBlazorApp1.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EmptyBlazorApp1.Entities.UserProfile", b =>
                {
                    b.HasOne("EmptyBlazorApp1.Entities.User", "User")
                        .WithOne("UserProfile")
                        .HasForeignKey("EmptyBlazorApp1.Entities.UserProfile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EmptyBlazorApp1.Entities.Community", b =>
                {
                    b.Navigation("HashTags");
                });

            modelBuilder.Entity("EmptyBlazorApp1.Entities.User", b =>
                {
                    b.Navigation("CreatedCommunities");

                    b.Navigation("UserProfile");
                });
#pragma warning restore 612, 618
        }
    }
}
