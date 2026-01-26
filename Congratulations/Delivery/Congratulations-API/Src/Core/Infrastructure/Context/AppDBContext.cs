namespace _4EPlatForm.Infrastructure.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<_4EPlatform.Domain.Models.Congrats> Congrats { get; set; }
        public DbSet<EmployeesGroups> EmployeesGroups { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<GroupsCongrats> GroupsCongrats { get; set; }
        public DbSet<LK_SendTo> LK_SendTo { get; set; }
        public DbSet<OrgsCongrats> OrgsCongrats { get; set; }
        public DbSet<SubCatgeory> SubCatgeories { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<UsersCongrats> UsersCongrats { get; set; }
        public DbSet<TemplateUserException> TemplateUserException { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
            base.OnModelCreating(modelBuilder);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        public bool AllMigrationsApplied()
        {
            var applied = this.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = this.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
        public void Migrate()
        {
            this.Database.Migrate();
        }
    }
}
