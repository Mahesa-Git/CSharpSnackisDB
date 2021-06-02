﻿// <auto-generated />
using System;
using CSharpSnackisDB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CSharpSnackisDB.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CSharpSnackisDB.Entities.Category", b =>
                {
                    b.Property<string>("CategoryID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EditDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.FilteredWords", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Words")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("FilteredWords");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.GroupChat", b =>
                {
                    b.Property<string>("GroupChatID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("GroupChatID");

                    b.ToTable("GroupChats");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Post", b =>
                {
                    b.Property<string>("PostID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BodyText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EditDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsReported")
                        .HasColumnType("bit");

                    b.Property<bool>("IsThreadStart")
                        .HasColumnType("bit");

                    b.Property<string>("ThreadID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PostID");

                    b.HasIndex("ThreadID");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.PostReaction", b =>
                {
                    b.Property<string>("PostReactionID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Counter")
                        .HasColumnType("int");

                    b.Property<string>("PostID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ReplyID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TypeID")
                        .HasColumnType("int");

                    b.HasKey("PostReactionID");

                    b.HasIndex("PostID");

                    b.HasIndex("ReplyID");

                    b.ToTable("PostReactions");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Reply", b =>
                {
                    b.Property<string>("ReplyID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BodyText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EditDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("GroupChatID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsReported")
                        .HasColumnType("bit");

                    b.Property<string>("PostID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ReplyID");

                    b.HasIndex("GroupChatID");

                    b.HasIndex("PostID");

                    b.HasIndex("UserId");

                    b.ToTable("Replies");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Thread", b =>
                {
                    b.Property<string>("ThreadID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BodyText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EditDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsReported")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TopicID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ThreadID");

                    b.HasIndex("TopicID");

                    b.HasIndex("UserId");

                    b.ToTable("Threads");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Topic", b =>
                {
                    b.Property<string>("TopicID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EditDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TopicID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MailToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfileText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = "admin-c0-aa65-4af8-bd17-00bd9344e575",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "e5315841-c908-4cb2-a520-a5b5b886bc14",
                            CreateDate = new DateTime(2021, 6, 2, 20, 37, 17, 834, DateTimeKind.Local).AddTicks(334),
                            Email = "admin@csharpsnackis.api",
                            EmailConfirmed = true,
                            IsBanned = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@csharsnackis.API",
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAEAACcQAAAAEPCeGHXUTvdca2NdfetBm3XMJ0kvbsn8gvg0HjKCrkgiyf7pYeqz2Nd9THeJrvmgHQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "0a73ca50-ec98-4cee-9b71-ae0a862e2910",
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("GroupChatUser", b =>
                {
                    b.Property<string>("GroupChatsGroupChatID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UsersId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("GroupChatsGroupChatID", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("GroupChatUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "root-0c0-aa65-4af8-bd17-00bd9344e575",
                            ConcurrencyStamp = "b351f3c6-d1de-4d0d-a420-ab2848e9595f",
                            Name = "root",
                            NormalizedName = "ROOT"
                        },
                        new
                        {
                            Id = "user-2c0-aa65-4af8-bd17-00bd9344e575",
                            ConcurrencyStamp = "f627599e-0499-48bf-b124-09514b98ba19",
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");

                    b.HasData(
                        new
                        {
                            UserId = "admin-c0-aa65-4af8-bd17-00bd9344e575",
                            RoleId = "root-0c0-aa65-4af8-bd17-00bd9344e575"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Post", b =>
                {
                    b.HasOne("CSharpSnackisDB.Entities.Thread", "Thread")
                        .WithMany("Posts")
                        .HasForeignKey("ThreadID");

                    b.HasOne("CSharpSnackisDB.Entities.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId");

                    b.Navigation("Thread");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.PostReaction", b =>
                {
                    b.HasOne("CSharpSnackisDB.Entities.Post", "Post")
                        .WithMany("PostReactions")
                        .HasForeignKey("PostID");

                    b.HasOne("CSharpSnackisDB.Entities.Reply", "Reply")
                        .WithMany()
                        .HasForeignKey("ReplyID");

                    b.Navigation("Post");

                    b.Navigation("Reply");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Reply", b =>
                {
                    b.HasOne("CSharpSnackisDB.Entities.GroupChat", "GroupChat")
                        .WithMany("Replies")
                        .HasForeignKey("GroupChatID");

                    b.HasOne("CSharpSnackisDB.Entities.Post", "Post")
                        .WithMany("Replies")
                        .HasForeignKey("PostID");

                    b.HasOne("CSharpSnackisDB.Entities.User", "User")
                        .WithMany("Replies")
                        .HasForeignKey("UserId");

                    b.Navigation("GroupChat");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Thread", b =>
                {
                    b.HasOne("CSharpSnackisDB.Entities.Topic", "Topic")
                        .WithMany("Threads")
                        .HasForeignKey("TopicID");

                    b.HasOne("CSharpSnackisDB.Entities.User", "User")
                        .WithMany("Threads")
                        .HasForeignKey("UserId");

                    b.Navigation("Topic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Topic", b =>
                {
                    b.HasOne("CSharpSnackisDB.Entities.Category", "Category")
                        .WithMany("Topics")
                        .HasForeignKey("CategoryID");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("GroupChatUser", b =>
                {
                    b.HasOne("CSharpSnackisDB.Entities.GroupChat", null)
                        .WithMany()
                        .HasForeignKey("GroupChatsGroupChatID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CSharpSnackisDB.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CSharpSnackisDB.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CSharpSnackisDB.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CSharpSnackisDB.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CSharpSnackisDB.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Category", b =>
                {
                    b.Navigation("Topics");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.GroupChat", b =>
                {
                    b.Navigation("Replies");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Post", b =>
                {
                    b.Navigation("PostReactions");

                    b.Navigation("Replies");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Thread", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Topic", b =>
                {
                    b.Navigation("Threads");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.User", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("Replies");

                    b.Navigation("Threads");
                });
#pragma warning restore 612, 618
        }
    }
}
