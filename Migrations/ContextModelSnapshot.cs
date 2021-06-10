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

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsReported")
                        .HasColumnType("bit");

                    b.Property<bool>("IsThreadStart")
                        .HasColumnType("bit");

                    b.Property<string>("PostReactionID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ThreadID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PostID");

                    b.HasIndex("PostReactionID");

                    b.HasIndex("ThreadID");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.PostReaction", b =>
                {
                    b.Property<string>("PostReactionID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("AddOrRemove")
                        .HasColumnType("bit");

                    b.Property<int>("LikeCounter")
                        .HasColumnType("int");

                    b.HasKey("PostReactionID");

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

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsReported")
                        .HasColumnType("bit");

                    b.Property<string>("PostID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PostReactionID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ReplyID");

                    b.HasIndex("GroupChatID");

                    b.HasIndex("PostID");

                    b.HasIndex("PostReactionID");

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

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<bool>("IsReported")
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
                            ConcurrencyStamp = "7ae56a15-5f72-46e7-9499-073a8294f7be",
                            CreateDate = new DateTime(2021, 6, 10, 13, 31, 13, 70, DateTimeKind.Local).AddTicks(4073),
                            Email = "admin@csharpsnackis.api",
                            EmailConfirmed = true,
                            IsBanned = false,
                            IsReported = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@csharsnackis.API",
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAEAACcQAAAAEAlVceK0fRc10QRjVe2lfqxxsF81u9e5OMTJ+kcBFZzkv+TdYdSfBzIql3FXt+EPXw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "8f4ae376-83e5-482b-9465-8995b581bc4a",
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
                            ConcurrencyStamp = "d80ce274-aa5e-46b5-b684-a7da1666ae66",
                            Name = "root",
                            NormalizedName = "ROOT"
                        },
                        new
                        {
                            Id = "user-2c0-aa65-4af8-bd17-00bd9344e575",
                            ConcurrencyStamp = "656f545e-f3e6-434b-9eb1-5963dfed7934",
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

            modelBuilder.Entity("PostReactionUser", b =>
                {
                    b.Property<string>("PostReactionsPostReactionID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UsersId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PostReactionsPostReactionID", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("PostReactionUser");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Post", b =>
                {
                    b.HasOne("CSharpSnackisDB.Entities.PostReaction", "PostReaction")
                        .WithMany()
                        .HasForeignKey("PostReactionID");

                    b.HasOne("CSharpSnackisDB.Entities.Thread", "Thread")
                        .WithMany("Posts")
                        .HasForeignKey("ThreadID");

                    b.HasOne("CSharpSnackisDB.Entities.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId");

                    b.Navigation("PostReaction");

                    b.Navigation("Thread");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CSharpSnackisDB.Entities.Reply", b =>
                {
                    b.HasOne("CSharpSnackisDB.Entities.GroupChat", "GroupChat")
                        .WithMany("Replies")
                        .HasForeignKey("GroupChatID");

                    b.HasOne("CSharpSnackisDB.Entities.Post", "Post")
                        .WithMany("Replies")
                        .HasForeignKey("PostID");

                    b.HasOne("CSharpSnackisDB.Entities.PostReaction", "PostReaction")
                        .WithMany()
                        .HasForeignKey("PostReactionID");

                    b.HasOne("CSharpSnackisDB.Entities.User", "User")
                        .WithMany("Replies")
                        .HasForeignKey("UserId");

                    b.Navigation("GroupChat");

                    b.Navigation("Post");

                    b.Navigation("PostReaction");

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

            modelBuilder.Entity("PostReactionUser", b =>
                {
                    b.HasOne("CSharpSnackisDB.Entities.PostReaction", null)
                        .WithMany()
                        .HasForeignKey("PostReactionsPostReactionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CSharpSnackisDB.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
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
