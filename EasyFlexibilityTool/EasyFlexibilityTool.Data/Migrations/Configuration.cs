namespace EasyFlexibilityTool.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Model;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "EasyFlexibilityTool.Data.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.AngleMeasurementCategories.AddOrUpdate(c => c.Id,
                new AngleMeasurementCategory {Id = Guid.Parse("42E48AC1-A1E9-4825-B226-697BAC882E1E"), Name = "Side Split"},
                new AngleMeasurementCategory {Id = Guid.Parse("42E48AC1-A1E9-4825-B226-697BAC882E2E"), Name = "Right Open"},
                new AngleMeasurementCategory {Id = Guid.Parse("42E48AC1-A1E9-4825-B226-697BAC882E3E"), Name = "Left Open"},
                new AngleMeasurementCategory {Id = Guid.Parse("42E48AC1-A1E9-4825-B226-697BAC882E4E"), Name = "Right True"},
                new AngleMeasurementCategory {Id = Guid.Parse("42E48AC1-A1E9-4825-B226-697BAC882E5E"), Name = "Left True" },
                new AngleMeasurementCategory {Id = Guid.Parse("42E48AC1-A1E9-4825-B226-697BAC882E6E"), Name = "Right Other"},
                new AngleMeasurementCategory {Id = Guid.Parse("42E48AC1-A1E9-4825-B226-697BAC882E7E"), Name = "Left Other"});

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
