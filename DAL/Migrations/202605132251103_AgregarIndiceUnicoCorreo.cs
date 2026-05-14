namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarIndiceUnicoCorreo : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Usuarios", "Correo", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Usuarios", new[] { "Correo" });
        }
    }
}
