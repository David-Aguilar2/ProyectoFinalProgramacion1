using System;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    public class IconicFashionDbContext : DbContext
    {
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

        public virtual DbSet<EL.Usuario> Usuarios { get; set; }
        public virtual DbSet<EL.Categoria> Categorias { get; set; }
        public virtual DbSet<EL.Producto> Productos { get; set; }
        public virtual DbSet<EL.RegistroSalida> RegistrosSalida { get; set; }
    }

}