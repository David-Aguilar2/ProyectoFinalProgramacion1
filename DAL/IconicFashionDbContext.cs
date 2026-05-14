using System;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    // Clase principal que gestiona la conexión y el mapeo de la base de datos mediante Entity Framework
    public class IconicFashionDbContext : DbContext
    {
        // Constructor que utiliza la cadena de conexión definida en el archivo App.config
        public IconicFashionDbContext() : base("name=IconicFashionDbContext")
        {
            // 1. Deshabilita la creación o modificación automática de la base de datos al iniciar
            // Esto evita errores de compatibilidad si el esquema ya existe o se maneja manualmente
            Database.SetInitializer<IconicFashionDbContext>(null);
        }

        // Configuración avanzada del modelo durante su creación
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 2. Elimina la convención que rastrea el historial de cambios (metadatos)
            // Esto impide que el sistema busque la tabla técnica "__MigrationHistory" en SQL Server
            modelBuilder.Conventions.Remove<System.Data.Entity.Infrastructure.IncludeMetadataConvention>();
            base.OnModelCreating(modelBuilder);
        }

        // Representación de la tabla Usuarios en el código
        public virtual DbSet<EL.Usuario> Usuarios { get; set; }

        // Representación de la tabla Categorías en el código
        public virtual DbSet<EL.Categoria> Categorias { get; set; }

        // Representación de la tabla Productos en el código
        public virtual DbSet<EL.Producto> Productos { get; set; }

        // Representación de la tabla de movimientos (Entradas/Salidas) en el código
        public virtual DbSet<EL.RegistroSalida> RegistrosSalida { get; set; }
    }
}