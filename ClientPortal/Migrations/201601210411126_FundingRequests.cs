namespace ClientPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FundingRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FundingRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CrmRecordId = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(),
                        UpdatedOn = c.DateTime(),
                        CreatedBy = c.Guid(nullable: false),
                        Status = c.String(),
                        organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.organization_Id)
                .Index(t => t.organization_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FundingRequests", "organization_Id", "dbo.Organizations");
            DropIndex("dbo.FundingRequests", new[] { "organization_Id" });
            DropTable("dbo.FundingRequests");
        }
    }
}
