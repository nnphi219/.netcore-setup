using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetLearning.Application.DBContexts
{
    public class LearningDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public LearningDbContext(DbContextOptions<LearningDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
