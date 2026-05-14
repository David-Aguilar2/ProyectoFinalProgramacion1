# Documentación Técnica - IconicFashion

## Arquitectura del Proyecto

IconicFashion está diseñada con un patrón de capas clásico: `GUI`, `BLL`, `DAL` y `EL`.

- `EL` (Entidades): define el modelo de datos y las reglas de esquema para Entity Framework.
- `DAL` (Data Access Layer): encapsula el acceso a la base de datos mediante `IconicFashionDbContext` y clases CRUD.
- `BLL` (Business Logic Layer): contiene reglas de negocio, validaciones y transacciones.
- `GUI` (Interfaz Gráfica): expone los formularios WinForms y consume los servicios de BLL.

La interacción entre capas es unidireccional:

- `GUI` usa `BLL`.
- `BLL` usa `DAL` y `EL`.
- `DAL` usa `EL` y `IconicFashionDbContext`.

Este patrón separa responsabilidades, hace más fácil testear la lógica de negocio y evita que la UI acceda directamente a la base de datos.

---

## Capa EL (Entidades)

### Usuario

La clase `EL.Usuario` representa al actor del sistema con validación de esquema.

```csharp
public class Usuario
{
    public const int ROL_SUPERADMIN = 1;
    public const int ROL_ADMIN = 2;
    public const int ROL_TRABAJADOR = 3;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdUsuario { get; set; }

    [Required, StringLength(100)]
    public string Nombre { get; set; }

    [StringLength(120)]
    [EmailAddress]
    [Index(IsUnique = true)]
    public string Correo { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; }

    [Required, StringLength(100)]
    public string ClaveAcceso { get; set; }

    [Required, StringLength(30)]
    public string Telefono { get; set; }

    [Required, StringLength(200)]
    public string Direccion { get; set; }

    [Required]
    public int Rol { get; set; }

    [Required]
    public bool Estado { get; set; }
}
```

- `Key`: marca `IdUsuario` como clave primaria.
- `DatabaseGenerated(DatabaseGeneratedOption.Identity)`: indica que la base de datos genera el valor.
- `Required`: obliga que la propiedad tenga valor.
- `StringLength(...)`: limita la longitud del texto.
- `EmailAddress`: valida el formato de correo.
- `Index(IsUnique = true)`: asegura unicidad en la columna `Correo`.

> Advertencia técnica: `Index(IsUnique = true)` es un atributo de Entity Framework 6 y crea un índice único en el esquema, garantizando que no existan dos usuarios con el mismo correo.

### Producto

```csharp
[Table("Productos")]
public class Producto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdProducto { get; set; }

    [Required, StringLength(100)]
    public string Nombre { get; set; }

    [Required, StringLength(200)]
    public string Descripcion { get; set; }

    [Required]
    [Column(TypeName = "decimal")]
    public decimal Precio { get; set; }

    [Required]
    public bool Estado { get; set; }

    [Required]
    public int IdCategoria { get; set; }

    [ForeignKey("IdCategoria")]
    public virtual Categoria Categoria { get; set; }

    [Required]
    public int Cantidad { get; set; }

    [Required]
    public DateTime FechaRegistro { get; set; }

    public bool StockBajo()
    {
        return Cantidad <= 4;
    }
}
```

- `ForeignKey("IdCategoria")`: relaciona `Producto` con `Categoria`.
- `StockBajo()`: helper para detectar inventario crítico.

### Categoria

```csharp
public class Categoria
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCategoria { get; set; }

    [Required, StringLength(100)]
    public string Nombre { get; set; }

    [Required, StringLength(200)]
    public string Descripcion { get; set; }
}
```

### RegistroSalida

```csharp
public class RegistroSalida
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdRegistro { get; set; }

    [Required]
    public int IdProducto { get; set; }

    [ForeignKey("IdProducto")]
    public virtual Producto Producto { get; set; }

    [Required]
    public int IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    public virtual Usuario Usuario { get; set; }

    [Required]
    public string Tipo { get; set; }

    [Required]
    public int Cantidad { get; set; }

    [Required]
    public string Motivo { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime FechaSalida { get; set; }
}
```

- `Tipo`: distingue entre `ENTRADA` y `SALIDA`.
- `FechaSalida`: se mapea como `datetime2` para mayor precisión.

---

## Capa DAL (Acceso a Datos)

### IconicFashionDbContext

El contexto `DAL.IconicFashionDbContext` es el punto central de Entity Framework:

```csharp
public class IconicFashionDbContext : DbContext
{
    public IconicFashionDbContext() : base("name=IconicFashionDbContext")
    {
        Database.SetInitializer<IconicFashionDbContext>(null);
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Conventions.Remove<System.Data.Entity.Infrastructure.IncludeMetadataConvention>();
        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<EL.Usuario> Usuarios { get; set; }
    public virtual DbSet<EL.Categoria> Categorias { get; set; }
    public virtual DbSet<EL.Producto> Productos { get; set; }
    public virtual DbSet<EL.RegistroSalida> RegistrosSalida { get; set; }
}
```

- `Database.SetInitializer<IconicFashionDbContext>(null)`: deshabilita los inicializadores automáticos de EF.
  - Esto evita que Entity Framework intente crear o modificar el esquema automáticamente.
  - Es una decisión útil en entornos donde las migraciones se aplican manualmente o la base de datos ya existe.
- `modelBuilder.Conventions.Remove<IncludeMetadataConvention>()`: elimina la convención que busca la tabla `__MigrationHistory`.
  - Se usa para impedir que EF intente validar metadatos de migraciones cuando la base de datos puede no incluir dicha tabla, o cuando se usa un control manual de esquema.

### Patrón CRUD en DAL

Las clases DAL siguen un patrón CRUD sencillo:

- `BuscarPorId` / `ObtenerProducto` / `ObtenerUsuarios` / `ObtenerProductos`
- `Guardar` con `EntityState.Modified` para actualizar o `Add` para insertar.
- `Eliminar` usa `Find` y `Remove`.

Ejemplo de guardado genérico en `DAL.UsuarioDAL`:

```csharp
public int Guardar(Usuario usuario)
{
    _db = new IconicFashionDbContext();

    if (usuario.IdUsuario > 0)
    {
        _db.Entry(usuario).State = EntityState.Modified;
    }
    else
    {
        _db.Usuarios.Add(usuario);
    }

    _db.SaveChanges();
    return usuario.IdUsuario;
}
```

### Método `ObtenerRegistrosSalida` y carga ansiosa

El método más relevante en `DAL.RegistroSalidaDAL` es:

```csharp
public List<RegistroSalida> ObtenerRegistrosSalida()
{
    using (_db = new IconicFashionDbContext())
    {
        return _db.RegistrosSalida
                  .Include(r => r.Producto)
                  .Include(r => r.Usuario)
                  .ToList();
    }
}
```

- `.Include(r => r.Producto)` y `.Include(r => r.Usuario)` realizan carga ansiosa (eager loading).
- Esto permite que los datos relacionados de producto y usuario estén disponibles inmediatamente.
- Evita `LazyLoading` y múltiples consultas posteriores cuando la UI necesita mostrar datos de las entidades relacionadas.

---

## Capa BLL (Lógica de Negocio)

### Validaciones de seguridad en `UsuarioBLL`

`UsuarioBLL` implementa validaciones robustas antes de delegar operaciones a DAL.

#### Hashing SHA256

```csharp
public static string EncriptarClave(string clave)
{
    using (SHA256 sha256Hash = SHA256.Create())
    {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(clave));
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}
```

- Se usa SHA256 para hashear la contraseña antes de compararla con el valor almacenado.
- Esto evita el almacenamiento de contraseñas en texto plano.

#### Validación con Regex

```csharp
if (!string.IsNullOrWhiteSpace(usuario.Correo))
{
    string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    if (!Regex.IsMatch(usuario.Correo, patron))
    {
        return "El formato del correo electrónico no es válido";
    }
}
```

- El patrón asegura un formato básico `nombre@dominio.ext`.
- También se valida la existencia del correo antes de insertar o actualizar.

#### Otras validaciones de `UsuarioBLL`

- `Username` obligatorio y mínimo 4 caracteres.
- `ClaveAcceso` obligatorio y mínimo 5 caracteres.
- `Telefono` obligatorio con longitud mínima para formato `0000-0000`.
- Exclusión de duplicados por `Username` y `Correo`.
- Restricción adicional para el usuario con `IdUsuario == 1`: no puede ser degradado ni eliminado.

### Lógica transaccional de `RegistroSalidaBLL`

`RegistroSalidaBLL.InsertarMovimiento` actualiza stock y registra el movimiento en la misma unidad de trabajo:

```csharp
using (var db = new IconicFashionDbContext())
{
    var productoBD = db.Productos.Find(registro.IdProducto);

    if (registro.Tipo == "SALIDA")
    {
        if (productoBD.Cantidad < registro.Cantidad)
            return $"Stock insuficiente. Solo hay {productoBD.Cantidad} unidades.";

        productoBD.Cantidad -= registro.Cantidad;
    }
    else
    {
        productoBD.Cantidad += registro.Cantidad;
    }

    db.RegistrosSalida.Add(registro);
    db.SaveChanges();
}
```

- Se consulta el producto existente en la misma instancia de contexto.
- Si el movimiento es `SALIDA`, valida que la cantidad disponible sea suficiente.
- Si el movimiento es `ENTRADA`, incrementa el stock.
- `db.SaveChanges()` persiste ambas modificaciones en un solo commit.
- Esto garantiza consistencia entre el inventario y el historial de movimientos.

### Auditoría automática en `ActualizarProducto`

`ProductoBLL.ActualizarProducto` registra un movimiento automático cuando cambia la cantidad de un producto:

```csharp
var productoOriginal = db.Productos.Find(productoEditado.IdProducto);
if (productoOriginal.Cantidad != productoEditado.Cantidad)
{
    int diferencia = productoEditado.Cantidad - productoOriginal.Cantidad;

    RegistroSalida movimientoAuto = new RegistroSalida
    {
        IdProducto = productoEditado.IdProducto,
        IdUsuario = idUsuario,
        FechaSalida = DateTime.Now,
        Motivo = "Stock actualizado por administrador",
        Tipo = diferencia > 0 ? "ENTRADA" : "SALIDA",
        Cantidad = Math.Abs(diferencia)
    };

    db.RegistrosSalida.Add(movimientoAuto);
}

db.Entry(productoOriginal).CurrentValues.SetValues(productoEditado);
db.SaveChanges();
```

- Si la cantidad cambia, se crea automáticamente un registro de salida/entrada.
- Esto proporciona trazabilidad de modificaciones manuales de stock.
- La auditoría se ejecuta antes de aplicar los cambios finales.

---

## Capa GUI (Interfaz)

### Flujo de inicio de la aplicación

El arranque está definido en `GUI\Program.cs`:

```csharp
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);

    while (true)
    {
        using (var login = new Login())
        {
            if (login.ShowDialog() != DialogResult.OK)
            {
                return;
            }
        }

        BuscarProductos menu = new BuscarProductos();
        Application.Run(menu);

        if (menu.DialogResult != DialogResult.OK)
        {
            break;
        }
    }
}
```

- Se muestra primero el formulario de `Login`.
- Si el login es exitoso, se ejecuta `BuscarProductos` como formulario principal.
- Al cerrar sesión, el mismo ciclo permite volver a `Login`.

### Login

`GUI.Autenticacion.Login` gestiona credenciales y establece el usuario autenticado con una propiedad global estática:

```csharp
string passHasheada = UsuarioBLL.EncriptarClave(pass);
var usuarioLogueado = usuarioBLL.Login(user, passHasheada);
```

- Valida que usuario y contraseña no estén vacíos.
- Hashea la contraseña con `UsuarioBLL.EncriptarClave` antes de comparar.
- Rechaza usuarios inactivos (`usuarioLogueado.Estado == false`).
- Tras el éxito, asigna `Login.UsuarioAutenticado` y cierra el formulario.

### BuscarProductos (Menú principal)

Este formulario es el hub principal de la aplicación. Sus responsabilidades son:

- mostrar productos con filtros de búsqueda y stock bajo,
- actualizar indicadores de inventario,
- abrir formularios secundarios,
- controlar permisos según el rol del usuario logueado,
- confirmar la salida de la aplicación.

#### Permisos basados en rol

```csharp
if (userLogueado.Rol == Usuario.ROL_TRABAJADOR)
{
    btnUsuarios.Visible = false;
}

if (userLogueado.Rol == Usuario.ROL_ADMIN)
{
    btnUsuarios.Visible = true;
    btnAlmacen.Visible = true;
}
```

- Un trabajador no puede acceder a la gestión de usuarios.
- Un administrador puede ver los botones de usuarios y almacén.

#### Carga de datos y filtros

```csharp
var listaFiltrada = productos
    .Where(p => p.Estado == true &&
               (p.Nombre.ToLower().Contains(filtro.ToLower()) ||
                p.Descripcion.ToLower().Contains(filtro.ToLower())))
    .Where(p => !filtrarStockBajo || p.Cantidad < 5)
    .Select(p => new
    {
        p.IdProducto,
        p.Nombre,
        Categoria = categorias.FirstOrDefault(c => c.IdCategoria == p.IdCategoria)?.Nombre ?? "N/A",
        p.Cantidad,
        p.Precio
    }).ToList();
```

- Filtra por nombre o descripción.
- Permite activar un filtro de stock bajo.
- Resuelve la categoría por su relación con `IdCategoria`.

#### Control único de formulario hijo

```csharp
private void AbrirFormularioUnico<T>() where T : Form, new()
{
    Form formularioExistente = Application.OpenForms.OfType<T>().FirstOrDefault();
    if (formularioExistente != null) formularioExistente.Close();

    T nuevoFormulario = new T();
    nuevoFormulario.FormClosed += (s, args) => {
        this.Show();
        ActualizarDashboard();
        CargarDatos("");
    };

    this.Hide();
    nuevoFormulario.Show();
}
```

- Evita múltiples instancias del mismo formulario.
- Vuelve a mostrar el menú principal y refresca los datos cuando se cierra el hijo.

#### Confirmación de cierre global

El menú principal muestra un cuadro de diálogo al cerrar, lo cual impide cierres accidentales.

```csharp
private void BuscarProductos_FormClosing(object sender, FormClosingEventArgs e)
{
    DialogResult resultado = MessageBox.Show(
        "¿Estás seguro de querer salir? Se cerrará la sesión actual",
        "Confirmar Salida",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

    if (resultado == DialogResult.No)
    {
        e.Cancel = true;
    }
    else
    {
        Application.ExitThread();
    }
}
```

### ListaUsuario

`GUI.Formularios.ListaUsuario` expone un grid con todas las cuentas existentes y provee botones de edición y eliminación.

- Configura dinámicamente columnas de datos y botones.
- Permite búsqueda por ID.
- Abre `FrmUsuarios` en modo edición o creación.
- Confirma eliminación con un `MessageBox`.
- Tiene la misma lógica de cierre global.

Fragmento clave:

```csharp
private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
{
    if (e.RowIndex < 0) return;
    int idUsuario = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells["Id"].Value);
    if (dgvUsuarios.Columns[e.ColumnIndex].Name == "Editar")
    {
        Usuario usuario = usuarioBLL.ObtenerUsuarioPorId(idUsuario);
        FrmUsuarios frmUsuarios = new FrmUsuarios(usuario);
        frmUsuarios.FormClosed += (s, args) => CargarDatos(txtBuscarId.Text);
        frmUsuarios.ShowDialog();
    }
    else if (dgvUsuarios.Columns[e.ColumnIndex].Name == "Eliminar")
    {
        var confirmResult = MessageBox.Show("¿Está seguro de eliminar este usuario?", "Confirmar eliminación", MessageBoxButtons.YesNo);
        if (confirmResult == DialogResult.Yes)
        {
            usuarioBLL.EliminarUsuario(idUsuario);
            CargarDatos(txtBuscarId.Text);
        }
    }
}
```

### FrmUsuarios

Este formulario maneja creación y edición de usuarios.

- Si recibe un `Usuario`, carga sus datos en los controles.
- Si no recibe usuario, opera en modo creación.
- Si el usuario es absoluto (`IdUsuario == 1`), bloquea la edición del rol.
- En actualización solo rehasea la contraseña si ha cambiado.

Código relevante de guardado:

```csharp
if (_usuarioEdicion != null)
{
    u.IdUsuario = _usuarioEdicion.IdUsuario;
    if (u.IdUsuario == 1) u.Rol = Usuario.ROL_SUPERADMIN;
    if (txtClaveAcceso.Text != _usuarioEdicion.ClaveAcceso)
    {
        u.ClaveAcceso = UsuarioBLL.EncriptarClave(txtClaveAcceso.Text);
    }
    else
    {
        u.ClaveAcceso = _usuarioEdicion.ClaveAcceso;
    }
    resultado = usuarioBLL.ActualizarUsuario(u);
}
else
{
    u.ClaveAcceso = UsuarioBLL.EncriptarClave(txtClaveAcceso.Text);
    resultado = usuarioBLL.InsertarUsuario(u);
}
```

- La separación entre creación y edición garantiza que no se reemplace la contraseña con valor vacío.
- El formulario delega en `UsuarioBLL` todas las reglas de negocio.

### ListaCategoria

`GUI.Formularios.ListaCategoria` es un listado similar a `ListaUsuario` con búsqueda por ID, edición y eliminación.

- Carga categorías en un grid.
- Usa botones de acción generados dinámicamente.
- Al editar abre `FrmCategoria`.
- Al eliminar confirma la acción y actualiza el grid.
- También implementa cierre global con confirmación.

### FrmCategoria

Formulario de alta y edición de categorías.

- Si existe `_categoriaEdicion`, carga campos para actualización.
- En creación inicializa el formulario en modo nuevo.
- Valida que el nombre no esté vacío.

Fragmento de guardado:

```csharp
Categoria c = new Categoria
{
    Nombre = txtNombre.Text,
    Descripcion = Descripcion.Text
};

if (_categoriaEdicion != null)
{
    c.IdCategoria = _categoriaEdicion.IdCategoria;
    resultado = categoriaBLL.ActualizarCategoria(c);
}
else
{
    resultado = categoriaBLL.InsertarCategoria(c);
}
```

### ListaAlmacen

`GUI.Formularios.ListaAlmacen` presenta el inventario de productos y aplica permisos de edición:

- Los `Trabajadores` no pueden ver botones de agregar, editar ni eliminar.
- Los `Administradores` sí pueden administrar productos.
- Muestra nombre de categoría y estado del producto.
- Permite filtrar por ID de producto.

#### Control de permisos

```csharp
if (userLogueado.Rol != Usuario.ROL_TRABAJADOR)
{
    DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn();
    // ...
    dgvAlmacen.Columns.Add(btnEditar);
    dgvAlmacen.Columns.Add(btnEliminar);
}

if (userLogueado.Rol == Usuario.ROL_TRABAJADOR)
{
    agregar.Visible = false;
    gCategorias.Visible = false;
}
```

### FrmAlmacen

Formulario de alta y edición de productos.

- Carga categorías en un combo con opción por defecto.
- Carga estado (`Activo`/`Inactivo`) y datos de producto en modo edición.
- Si se actualiza, pasa `idUsuarioActual` para auditar cambios de stock.

Fragmento clave:

```csharp
if (_productoEdicion != null)
{
    p.IdProducto = _productoEdicion.IdProducto;
    p.FechaRegistro = _productoEdicion.FechaRegistro;
    resultado = productoBLL.ActualizarProducto(p, idUsuarioActual);
}
else
{
    resultado = productoBLL.InsertarProducto(p);
}
```

- El formulario no implementa lógica de negocio compleja; delega en `ProductoBLL`.
- Esto mantiene la interfaz desacoplada del control de stock y auditoría.

### FrmRegistroSalida

Este formulario gestiona movimientos de inventario:

- Carga productos y usuarios en combo boxes.
- Selecciona al usuario autenticado por defecto y no permite cambiarlo.
- Presenta opciones `ENTRADA` y `SALIDA`.
- Valida producto seleccionado, cantidad > 0 y motivo.

Fragmento de validación y registro:

```csharp
RegistroSalida movimiento = new RegistroSalida
{
    IdProducto = Convert.ToInt32(cmbProducto.SelectedValue),
    IdUsuario = Login.UsuarioAutenticado.IdUsuario,
    Tipo = cmbTipo.Text,
    Cantidad = (int)txtCantidad.Value,
    Motivo = txtMotivo.Text,
    FechaSalida = DateTime.Now
};

string resultado = registroBLL.InsertarMovimiento(movimiento);
```

- Si el registro es exitoso, muestra mensaje de confirmación.
- En caso de error, muestra el mensaje devuelto por la BLL.

### ListaRegistroSalida

`GUI.Formularios.ListaRegistroSalida` muestra el historial de movimientos con filtros de fecha.

- Usa `registroBLL.ObtenerRegistrosSalida()`.
- Muestra columnas de fecha, producto, tipo, cantidad, motivo y usuario.
- Filtra entre `dtpDesde` y `dtpHasta`.
- Ordena el resultado de forma descendente por fecha.

Fragmento de carga:

```csharp
var listaFiltrada = movimientos
    .Where(m => m.FechaSalida.Date >= fechaDesde &&
                m.FechaSalida.Date <= fechaHasta)
    .OrderByDescending(m => m.FechaSalida)
    .ToList();
```

- Al usar `Include` en DAL, los valores de `m.Producto?.Nombre` y `m.Usuario?.Nombre` están disponibles sin consultas adicionales.

### Comportamiento de cierre común

Varias formas de lista (`ListaUsuario`, `ListaCategoria`, `ListaAlmacen`, `ListaRegistroSalida`) implementan un patrón de confirmación idéntico:

- se pregunta al usuario si desea salir,
- se cancela el cierre con `e.Cancel = true` si la respuesta es `No`,
- se cierra la aplicación con `Application.ExitThread()` si la respuesta es `Sí`.

---

## Guía de Instalación

Para aplicar las migraciones de Entity Framework en este proyecto, use la Consola de Administrador de Paquetes de Visual Studio con los siguientes comandos:

```powershell
Add-Migration NombreDeLaMigracion
Update-Database
```

Ejemplo:

```powershell
Add-Migration AgregarIndiceUnicoCorreo
Update-Database
```

- `Add-Migration`: crea una nueva migración basada en los cambios del modelo.
- `Update-Database`: aplica los cambios al esquema de la base de datos.

> Nota: el proyecto ya deshabilita el inicializador automático con `Database.SetInitializer<IconicFashionDbContext>(null);`, por lo que las migraciones deben aplicarse manualmente.
