namespace EvidencijaSoftvera_IlijaDivljan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdditionalEquipment",
                c => new
                    {
                        AdditionalEquipmentId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 60),
                        Description = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.AdditionalEquipmentId);
            
            CreateTable(
                "dbo.Computers",
                c => new
                    {
                        ComputersId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 40),
                        ComputerType = c.Int(nullable: false),
                        Manufacturer = c.String(nullable: false, maxLength: 40),
                        Model = c.String(nullable: false, maxLength: 60),
                        SerialNumber = c.String(nullable: false, maxLength: 20),
                        Cpu = c.String(maxLength: 40),
                        Ram = c.Single(nullable: false),
                        VideoCard = c.String(),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ComputersId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.SerialNumber, unique: true)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.InstalledSoftware",
                c => new
                    {
                        InstalledSoftwareId = c.Int(nullable: false, identity: true),
                        RecordDate = c.DateTime(nullable: false),
                        ComputersId = c.Int(nullable: false),
                        SoftwareId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.InstalledSoftwareId)
                .ForeignKey("dbo.Computers", t => t.ComputersId, cascadeDelete: true)
                .ForeignKey("dbo.Software", t => t.SoftwareId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ComputersId)
                .Index(t => t.SoftwareId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Software",
                c => new
                    {
                        SoftwareId = c.Int(nullable: false, identity: true),
                        Category = c.Int(nullable: false),
                        Manufacturer = c.String(nullable: false, maxLength: 40),
                        Name = c.String(nullable: false, maxLength: 80),
                        Version = c.String(nullable: false, maxLength: 30),
                        License = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SoftwareId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
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
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
                "dbo.ComputersAdditionalEquipment",
                c => new
                    {
                        ComputersId = c.Int(nullable: false),
                        AdditionalEquipmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ComputersId, t.AdditionalEquipmentId })
                .ForeignKey("dbo.Computers", t => t.ComputersId, cascadeDelete: true)
                .ForeignKey("dbo.AdditionalEquipment", t => t.AdditionalEquipmentId, cascadeDelete: true)
                .Index(t => t.ComputersId)
                .Index(t => t.AdditionalEquipmentId);
            
            CreateStoredProcedure(
                "dbo.AdditionalEquipment_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 60),
                        Description = p.String(maxLength: 150),
                    },
                body:
                    @"INSERT [dbo].[AdditionalEquipment]([Name], [Description])
                      VALUES (@Name, @Description)
                      
                      DECLARE @AdditionalEquipmentId int
                      SELECT @AdditionalEquipmentId = [AdditionalEquipmentId]
                      FROM [dbo].[AdditionalEquipment]
                      WHERE @@ROWCOUNT > 0 AND [AdditionalEquipmentId] = scope_identity()
                      
                      SELECT t0.[AdditionalEquipmentId]
                      FROM [dbo].[AdditionalEquipment] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[AdditionalEquipmentId] = @AdditionalEquipmentId"
            );
            
            CreateStoredProcedure(
                "dbo.AdditionalEquipment_Update",
                p => new
                    {
                        AdditionalEquipmentId = p.Int(),
                        Name = p.String(maxLength: 60),
                        Description = p.String(maxLength: 150),
                    },
                body:
                    @"UPDATE [dbo].[AdditionalEquipment]
                      SET [Name] = @Name, [Description] = @Description
                      WHERE ([AdditionalEquipmentId] = @AdditionalEquipmentId)"
            );
            
            CreateStoredProcedure(
                "dbo.AdditionalEquipment_Delete",
                p => new
                    {
                        AdditionalEquipmentId = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[AdditionalEquipment]
                      WHERE ([AdditionalEquipmentId] = @AdditionalEquipmentId)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.AdditionalEquipment_Delete");
            DropStoredProcedure("dbo.AdditionalEquipment_Update");
            DropStoredProcedure("dbo.AdditionalEquipment_Insert");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InstalledSoftware", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Computers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InstalledSoftware", "SoftwareId", "dbo.Software");
            DropForeignKey("dbo.InstalledSoftware", "ComputersId", "dbo.Computers");
            DropForeignKey("dbo.ComputersAdditionalEquipment", "AdditionalEquipmentId", "dbo.AdditionalEquipment");
            DropForeignKey("dbo.ComputersAdditionalEquipment", "ComputersId", "dbo.Computers");
            DropIndex("dbo.ComputersAdditionalEquipment", new[] { "AdditionalEquipmentId" });
            DropIndex("dbo.ComputersAdditionalEquipment", new[] { "ComputersId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.InstalledSoftware", new[] { "ApplicationUserId" });
            DropIndex("dbo.InstalledSoftware", new[] { "SoftwareId" });
            DropIndex("dbo.InstalledSoftware", new[] { "ComputersId" });
            DropIndex("dbo.Computers", new[] { "ApplicationUserId" });
            DropIndex("dbo.Computers", new[] { "SerialNumber" });
            DropTable("dbo.ComputersAdditionalEquipment");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Software");
            DropTable("dbo.InstalledSoftware");
            DropTable("dbo.Computers");
            DropTable("dbo.AdditionalEquipment");
        }
    }
}
