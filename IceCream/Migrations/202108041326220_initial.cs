namespace IceCream.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 60),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 700),
                        Url = c.String(maxLength: 500),
                        CategorySort = c.Int(nullable: false),
                        CategoryActive = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                        ShowMenu = c.Boolean(nullable: false),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                        Image = c.String(maxLength: 500),
                        TypePost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 150),
                        Description = c.String(nullable: false, maxLength: 500),
                        Body = c.String(),
                        Image = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Sort = c.Int(nullable: false),
                        View = c.Int(nullable: false),
                        ArticleCategoryId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Home = c.Boolean(nullable: false),
                        Url = c.String(maxLength: 300),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArticleCategories", t => t.ArticleCategoryId, cascadeDelete: true)
                .Index(t => t.ArticleCategoryId);
            
            CreateTable(
                "dbo.Banners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BannerName = c.String(nullable: false, maxLength: 100),
                        Slogan = c.String(maxLength: 200),
                        Image = c.String(maxLength: 500),
                        Active = c.Boolean(nullable: false),
                        GroupId = c.Int(nullable: false),
                        Url = c.String(maxLength: 500),
                        Sort = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        CartId = c.String(),
                        ProductId = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Count = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Description = c.String(nullable: false, maxLength: 600),
                        Body = c.String(),
                        ListImage = c.String(),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        SaleOff = c.Decimal(precision: 18, scale: 2),
                        CreateDate = c.DateTime(nullable: false),
                        Sort = c.Int(nullable: false),
                        ProductCategoryId = c.Int(nullable: false),
                        Hot = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Home = c.Boolean(nullable: false),
                        Url = c.String(maxLength: 300),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryId, cascadeDelete: true)
                .Index(t => t.ProductCategoryId);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 80),
                        Url = c.String(maxLength: 500),
                        CategorySort = c.Int(nullable: false),
                        CategoryActive = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                        Description = c.String(maxLength: 500),
                        Image = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Fullname = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 60),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 200),
                        Avatar = c.String(maxLength: 500),
                        CreateDate = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaDonHang = c.String(maxLength: 50),
                        CreateDate = c.DateTime(nullable: false),
                        Payment = c.Boolean(nullable: false),
                        TypePay = c.Int(nullable: false),
                        Transport = c.Int(nullable: false),
                        TransportDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Viewed = c.Boolean(nullable: false),
                        CustomerInfo_Fullname = c.String(nullable: false, maxLength: 50),
                        CustomerInfo_Address = c.String(nullable: false, maxLength: 200),
                        CustomerInfo_Mobile = c.String(nullable: false, maxLength: 11),
                        CustomerInfo_Email = c.String(nullable: false, maxLength: 50),
                        CustomerInfo_Body = c.String(maxLength: 200),
                        ThanhToanTruoc = c.Int(nullable: false),
                        OrderMemberId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.OrderMemberId)
                .Index(t => t.OrderMemberId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.OrderId, t.ProductId })
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ConfigSites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Facebook = c.String(maxLength: 500),
                        Linkedin = c.String(maxLength: 500),
                        Instagram = c.String(maxLength: 500),
                        Youtube = c.String(maxLength: 500),
                        Twitter = c.String(maxLength: 500),
                        LiveChat = c.String(maxLength: 4000),
                        Image = c.String(),
                        GoogleMap = c.String(maxLength: 4000),
                        GoogleAnalytics = c.String(maxLength: 4000),
                        Place = c.String(),
                        Title = c.String(maxLength: 200),
                        Description = c.String(maxLength: 500),
                        Hotline = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        AboutImage = c.String(),
                        AboutText = c.String(),
                        InfoFooter = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fullname = c.String(maxLength: 50),
                        Address = c.String(maxLength: 200),
                        Mobile = c.String(),
                        Email = c.String(nullable: false),
                        Body = c.String(maxLength: 4000),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 700),
                        Image = c.String(),
                        Name = c.String(nullable: false, maxLength: 100),
                        Job = c.String(maxLength: 100),
                        Sort = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProducts",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Product_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.UserProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.UserProducts", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Orders", "OrderMemberId", "dbo.Users");
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.Articles", "ArticleCategoryId", "dbo.ArticleCategories");
            DropIndex("dbo.UserProducts", new[] { "Product_Id" });
            DropIndex("dbo.UserProducts", new[] { "User_Id" });
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "OrderMemberId" });
            DropIndex("dbo.Products", new[] { "ProductCategoryId" });
            DropIndex("dbo.Carts", new[] { "ProductId" });
            DropIndex("dbo.Articles", new[] { "ArticleCategoryId" });
            DropTable("dbo.UserProducts");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Contacts");
            DropTable("dbo.ConfigSites");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
            DropTable("dbo.Users");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.Carts");
            DropTable("dbo.Banners");
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleCategories");
            DropTable("dbo.Admins");
        }
    }
}
