namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambioEnTablaProductos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "FechaRegistro", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "FechaRegistro");
        }
    }
}
