namespace EasyFlexibilityTool.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAngleMeasurementCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AngleMeasurementCategory",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            //Insert a default "Side Split" AngleMeasurementCategory
            Sql("INSERT INTO dbo.AngleMeasurementCategory (Id, Name) VALUES ('42E48AC1-A1E9-4825-B226-697BAC882E1E', 'Side Split')");

            AddColumn("dbo.AngleMeasurement", "AngleMeasurementCategoryId", c => c.Guid());
            AddColumn("dbo.AnonymAngleMeasurement", "AngleMeasurementCategoryId", c => c.Guid());

            //Set all NULL AngleMeasurementCategoryId to the default
            Sql("UPDATE dbo.AngleMeasurement SET AngleMeasurementCategoryId = '42E48AC1-A1E9-4825-B226-697BAC882E1E' WHERE AngleMeasurementCategoryId IS NULL");
            Sql("UPDATE dbo.AnonymAngleMeasurement SET AngleMeasurementCategoryId = '42E48AC1-A1E9-4825-B226-697BAC882E1E' WHERE AngleMeasurementCategoryId IS NULL");

            AlterColumn("dbo.AngleMeasurement", "AngleMeasurementCategoryId", c => c.Guid(nullable: false));
            AlterColumn("dbo.AnonymAngleMeasurement", "AngleMeasurementCategoryId", c => c.Guid(nullable: false));

            CreateIndex("dbo.AngleMeasurement", "AngleMeasurementCategoryId");
            CreateIndex("dbo.AnonymAngleMeasurement", "AngleMeasurementCategoryId");
            AddForeignKey("dbo.AngleMeasurement", "AngleMeasurementCategoryId", "dbo.AngleMeasurementCategory", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AnonymAngleMeasurement", "AngleMeasurementCategoryId", "dbo.AngleMeasurementCategory", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnonymAngleMeasurement", "AngleMeasurementCategoryId", "dbo.AngleMeasurementCategory");
            DropForeignKey("dbo.AngleMeasurement", "AngleMeasurementCategoryId", "dbo.AngleMeasurementCategory");
            DropIndex("dbo.AnonymAngleMeasurement", new[] { "AngleMeasurementCategoryId" });
            DropIndex("dbo.AngleMeasurement", new[] { "AngleMeasurementCategoryId" });
            DropColumn("dbo.AnonymAngleMeasurement", "AngleMeasurementCategoryId");
            DropColumn("dbo.AngleMeasurement", "AngleMeasurementCategoryId");
            DropTable("dbo.AngleMeasurementCategory");
        }
    }
}
