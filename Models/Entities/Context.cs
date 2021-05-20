using CSharpSnackisDB.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Models.Entities
{
    public class Context : IdentityDbContext<User>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<FilteredWords> FilteredWords { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Topic> Topics { get; set; }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            const string adminId = "admin-c0-aa65-4af8-bd17-00bd9344e575";
            const string roleId = "root-0c0-aa65-4af8-bd17-00bd9344e575";
            const string userRoleId = "user-2c0-aa65-4af8-bd17-00bd9344e575";

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = roleId,
                Name = "root",
                NormalizedName = "ROOT"
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = userRoleId,
                Name = "User",
                NormalizedName = "USER"
            });

            var hasher = new PasswordHasher<User>();

            builder.Entity<User>().HasData(new User
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@csharpsnackis.api",
                NormalizedEmail = "ADMIN@csharsnackis.API",
                CreateDate = DateTime.Now,
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "AdminPass2!"),
                SecurityStamp = Guid.NewGuid().ToString(),
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roleId,
                UserId = adminId
            });
        }
    }
}
