using Microsoft.EntityFrameworkCore;
using UFAR.DM.API.Data.Entities;

namespace UFAR.DM.API.Data.DAO {
    public class MainDbContext : DbContext {
        public MainDbContext(DbContextOptions options) : base(options) { }
        public DbSet<WordEntity> Words { get; set; }
        public DbSet<ExpressionEntity> Expressions { get; set; }
        public DbSet<SectionEntity> Sections { get; set; }
        public DbSet<QuestionEntity> Questions { get; set; }
    }
}