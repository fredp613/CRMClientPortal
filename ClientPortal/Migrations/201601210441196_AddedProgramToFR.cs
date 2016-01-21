namespace ClientPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProgramToFR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FundingRequests", "CrmProgramId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FundingRequests", "CrmProgramId");
        }
    }
}
