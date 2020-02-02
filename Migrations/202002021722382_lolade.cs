namespace InterviewTest001.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lolade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDocuments", "UserId", "dbo.UserInformations");
            DropIndex("dbo.UserDocuments", new[] { "UserId" });
            AddColumn("dbo.UserDocuments", "CreatedBy", c => c.String());
            AddColumn("dbo.UserDocuments", "TransactionNo", c => c.String());
            AlterColumn("dbo.UserInformations", "TransactionNo", c => c.String());
            DropColumn("dbo.UserDocuments", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserDocuments", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.UserInformations", "TransactionNo", c => c.Guid(nullable: false));
            DropColumn("dbo.UserDocuments", "TransactionNo");
            DropColumn("dbo.UserDocuments", "CreatedBy");
            CreateIndex("dbo.UserDocuments", "UserId");
            AddForeignKey("dbo.UserDocuments", "UserId", "dbo.UserInformations", "UserId", cascadeDelete: true);
        }
    }
}
