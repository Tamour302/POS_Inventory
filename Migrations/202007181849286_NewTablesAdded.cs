namespace POS_Inventory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTablesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Customer_Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Phone_Number = c.String(),
                        Shop_Number = c.String(),
                        Is_Active = c.Boolean(nullable: false),
                        Is_Deleted = c.Boolean(nullable: false),
                        Remaining_Amount = c.Double(nullable: false),
                        Created_Date = c.DateTime(nullable: false),
                        Modified_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Sale_Id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        Amount_Pay = c.Double(nullable: false),
                        Grand_Total = c.Double(nullable: false),
                        Left_Amount = c.Double(nullable: false),
                        Remaining_Amount = c.Double(nullable: false),
                        Is_Paid = c.Boolean(nullable: false),
                        Is_Active = c.Boolean(nullable: false),
                        Is_Deleted = c.Boolean(nullable: false),
                        Created_Date = c.DateTime(nullable: false),
                        Modified_Date = c.DateTime(nullable: false),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Sale_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Sale_FoodItemRelation",
                c => new
                    {
                        Sale_FoodItem_Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Double(nullable: false),
                        Rate = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        Created_Date = c.DateTime(nullable: false),
                        Modified_Date = c.DateTime(nullable: false),
                        Sale_Id = c.Int(),
                        Food_Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Sale_FoodItem_Id)
                .ForeignKey("dbo.FoodItems", t => t.Food_Item_Id)
                .ForeignKey("dbo.Sales", t => t.Sale_Id)
                .Index(t => t.Sale_Id)
                .Index(t => t.Food_Item_Id);
            
            CreateTable(
                "dbo.FoodItems",
                c => new
                    {
                        FoodItem_Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Is_Active = c.Boolean(nullable: false),
                        Is_Deleted = c.Boolean(nullable: false),
                        Created_Date = c.DateTime(nullable: false),
                        Modified_Date = c.DateTime(nullable: false),
                        FoodCategory = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FoodItem_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Sale_FoodItemRelation", "Sale_Id", "dbo.Sales");
            DropForeignKey("dbo.Sale_FoodItemRelation", "Food_Item_Id", "dbo.FoodItems");
            DropForeignKey("dbo.Sales", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Sale_FoodItemRelation", new[] { "Food_Item_Id" });
            DropIndex("dbo.Sale_FoodItemRelation", new[] { "Sale_Id" });
            DropIndex("dbo.Sales", new[] { "Customer_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.FoodItems");
            DropTable("dbo.Sale_FoodItemRelation");
            DropTable("dbo.Sales");
            DropTable("dbo.Customers");
        }
    }
}
