namespace EasyFlexibilityTool.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", false)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<AngleMeasurement> AngleMeasurements { get; set; }
        public IDbSet<AnonymAngleMeasurement> AnonymAngleMeasurements { get; set; }
        public IDbSet<AngleMeasurementCategory> AngleMeasurementCategories { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<EmailTemplate> EmailTemplates { get; set; }
    }
}
