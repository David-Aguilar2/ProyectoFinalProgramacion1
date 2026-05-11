namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BDFinal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        IdCategoria = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.IdCategoria);
            
            CreateTable(
                "dbo.Productos",
                c => new
                    {
                        IdProducto = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(nullable: false, maxLength: 200),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estado = c.Boolean(nullable: false),
                        IdCategoria = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdProducto)
                .ForeignKey("dbo.Categorias", t => t.IdCategoria, cascadeDelete: true)
                .Index(t => t.IdCategoria);
            
            CreateTable(
                "dbo.RegistroSalidas",
                c => new
                    {
                        IdRegistro = c.Int(nullable: false, identity: true),
                        IdProducto = c.Int(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        Tipo = c.String(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Motivo = c.String(nullable: false),
                        FechaSalida = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.IdRegistro)
                .ForeignKey("dbo.Productos", t => t.IdProducto, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdProducto)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        IdUsuario = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Correo = c.String(maxLength: 120),
                        Username = c.String(nullable: false, maxLength: 50),
                        ClaveAcceso = c.String(nullable: false, maxLength: 100),
                        Telefono = c.String(nullable: false, maxLength: 30),
                        Direccion = c.String(nullable: false, maxLength: 200),
                        Rol = c.Int(nullable: false),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdUsuario);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegistroSalidas", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.RegistroSalidas", "IdProducto", "dbo.Productos");
            DropForeignKey("dbo.Productos", "IdCategoria", "dbo.Categorias");
            DropIndex("dbo.RegistroSalidas", new[] { "IdUsuario" });
            DropIndex("dbo.RegistroSalidas", new[] { "IdProducto" });
            DropIndex("dbo.Productos", new[] { "IdCategoria" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.RegistroSalidas");
            DropTable("dbo.Productos");
            DropTable("dbo.Categorias");
        }
    }
}
