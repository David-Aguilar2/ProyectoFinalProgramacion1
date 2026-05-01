using System;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    public class IconicFashionDbContext : DbContext
    {
        // El contexto se ha configurado para usar una cadena de conexión 'DbContext' del archivo 
        // de configuración de la aplicación (App.config o Web.config). De forma predeterminada, 
        // esta cadena de conexión tiene como destino la base de datos 'DAL.DbContext' de la instancia LocalDb. 
        // 
        // Si desea tener como destino una base de datos y/o un proveedor de base de datos diferente, 
        // modifique la cadena de conexión 'DbContext'  en el archivo de configuración de la aplicación.
        public IconicFashionDbContext() : base("name=IconicFashionDbContext")
        {
            // 1. Esto anula CUALQUIER sistema de migración (borra el error de "AutomaticMigrationsDisabled")
            Database.SetInitializer<IconicFashionDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 2. Esto evita que EF intente buscar la tabla __MigrationHistory
            modelBuilder.Conventions.Remove<System.Data.Entity.Infrastructure.IncludeMetadataConvention>();
            base.OnModelCreating(modelBuilder);
        }

        // Agregue un DbSet para cada tipo de entidad que desee incluir en el modelo. Para obtener más información 
        // sobre cómo configurar y usar un modelo Code First, vea http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<EL.Usuario> Usuarios { get; set; }
        public virtual DbSet<EL.Categoria> Categorias { get; set; }
        public virtual DbSet<EL.Producto> Productos { get; set; }
        public virtual DbSet<EL.RegistroSalida> RegistrosSalida { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}