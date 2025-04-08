using Financial_List.ViewModels;
using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

namespace Financial_List.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<LikeList> LikeList { get; set; }

        public DbSet<UserLikeListViewModel> UserLikeListView { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserLikeListViewModel>().HasNoKey(); // 沒有主鍵的 ViewModel
        }
    }
}
