namespace EasyFlexibilityTool.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailTemplateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailTemplate",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TemplateName = c.String(),
                        SendCondition = c.String(),
                        TemplateContent = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailTemplate");
        }
    }
}
