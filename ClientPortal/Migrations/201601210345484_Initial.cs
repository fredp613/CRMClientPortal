namespace ClientPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CrmRecordId = c.Guid(nullable: false),
                        AddressType = c.String(),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        AddressLine3 = c.String(),
                        AddressLine4 = c.String(),
                        City = c.String(),
                        SubDivision = c.String(),
                        Country = c.String(),
                        PostalCode = c.String(),
                        CreatedOn = c.DateTime(),
                        UpdatedOn = c.DateTime(),
                        CreatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CrmRecordId = c.Guid(nullable: false),
                        Name = c.String(),
                        OrganizationType = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(),
                        UpdatedOn = c.DateTime(),
                        CreatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
           
            
            CreateTable(
                "dbo.OrganizationAddresses",
                c => new
                    {
                        Organization_Id = c.Int(nullable: false),
                        Address_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organization_Id, t.Address_id })
                .ForeignKey("dbo.Organizations", t => t.Organization_Id, cascadeDelete: true)
                .ForeignKey("dbo.Addresses", t => t.Address_id, cascadeDelete: true)
                .Index(t => t.Organization_Id)
                .Index(t => t.Address_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OrganizationAddresses", "Address_id", "dbo.Addresses");
            DropForeignKey("dbo.OrganizationAddresses", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.OrganizationAddresses", new[] { "Address_id" });
            DropIndex("dbo.OrganizationAddresses", new[] { "Organization_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.OrganizationAddresses");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Organizations");
            DropTable("dbo.Addresses");
        }
    }
}
