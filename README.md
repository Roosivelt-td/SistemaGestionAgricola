# Sistema de Gestión Agrícola

API RESTful para la gestión integral de operaciones agrícolas, incluyendo administración de agricultores, compradores, terrenos, cultivos, cosechas y ventas.

## Tabla de Contenidos

- [Características](#características)
- [Tecnologías utilizadas](#tecnologías-utilizadas)
- [Instalación](#instalación)
- [Configuración](#configuración)
- [Documentación de la API](#documentación-de-la-api)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Endpoints Principales](#endpoints-principales)
- [Autenticación](#autenticación)
- [Roles de Usuarios](#roles-de-usuarios)
- [Contribución](#contribución)
- [Licencia](#licencia)

## Características

- Autenticación y autorización basada en JWT
- Roles de usuarios (admin, agricultor, comprador)
- Gestión completa de ciclo agrícola (terrenos, cultivos, cosechas)
- Sistema de ventas integrado
- Validación de datos robusta
- Manejo de errores estructurado
- Integración con base de datos MySQL
- Documentación automática con Swagger/OpenAPI

## Tecnologías utilizadas

- **ASP.NET Core 8** - Framework web
- **Entity Framework Core** - Mapeo objeto-relacional
- **MySQL** - Base de datos relacional
- **JWT** - Autenticación y autorización
- **Swagger/OpenAPI** - Documentación de API
- **Pomelo.EntityFrameworkCore.MySql** - Conector MySQL para EF Core

## Instalación

1. Clonar el repositorio:
```bash
git clone git@github.com:Roosivelt-td/SistemaGestionAgricola.git
cd SistemaGestionAgricola
git checkout Develop
```

2. Restaurar dependencias:
```bash
dotnet restore
```

3. Configurar la base de datos en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=sistema_gestion_agricola;User=root;Password=tu_contraseña;"
  }
}
```

4. Aplicar migraciones:
```bash
dotnet ef database update
```

5. Ejecutar la aplicación:
```bash
dotnet run
```

## Configuración

### Variables de Entorno

Las principales configuraciones están en `appsettings.json`:

- **JWT**: Configuración de tokens de autenticación
- **EmailSettings**: Configuración del servicio de correo electrónico
- **ConnectionStrings**: Cadena de conexión a la base de datos

### Configuración de Correo Electrónico

Para habilitar funcionalidades de notificación por correo:
1. Actualizar credenciales en `appsettings.json`
2. Habilitar aplicaciones menos seguras o usar contraseñas de aplicación según el proveedor SMTP

## Documentación de la API

La documentación interactiva de la API está disponible a través de Swagger UI:

- Desarrollo: `http://localhost:5173`
- Producción: `https://tu-dominio.com/swagger`

La especificación OpenAPI en formato YAML se encuentra en: `openapi.yaml`

## Estructura del Proyecto

```
Controllers/
├── AuthController.cs           # Autenticación y registro
├── AgricultoresController.cs   # Gestión de agricultores
├── CompradoresController.cs    # Gestión de compradores
├── TerrenosController.cs       # Gestión de terrenos
├── CultivosController.cs       # Gestión de cultivos
├── CosechasController.cs       # Gestión de cosechas
├── VentasController.cs         # Gestión de ventas
├── UsuariosController.cs       # Gestión de usuarios
└── ...                         # Otros controladores

Models/
├── Entities/                   # Modelos de entidad
├── DTOs/                       # Objetos de transferencia de datos
└── ...

Services/                       # Servicios de negocio
├── JwtService.cs               # Servicio de JWT
├── EmailService.cs             # Servicio de correo
└── ...

Interfaces/                     # Interfaces de servicios
DbContext/                      # Contexto de base de datos
Helpers/                        # Extensiones y ayudantes
Middleware/                     # Middlewares personalizados
Migrations/                     # Migraciones de base de datos
```

## Endpoints Principales

### Autenticación
- `POST /api/Auth/login` - Iniciar sesión
- `POST /api/Auth/register` - Registrar nuevo usuario

### Usuarios
- `GET /api/Usuarios` - Listar usuarios (admin)
- `GET /api/Usuarios/{id}` - Obtener usuario por ID
- `PUT /api/Usuarios/{id}` - Actualizar usuario
- `DELETE /api/Usuarios/{id}` - Eliminar usuario (admin)

### Agricultores
- `GET /api/Agricultores` - Listar agricultores (admin)
- `GET /api/Agricultores/{id}` - Obtener agricultor por ID
- `POST /api/Agricultores` - Crear nuevo agricultor
- `PUT /api/Agricultores/{id}` - Actualizar agricultor
- `DELETE /api/Agricultores/{id}` - Eliminar agricultor (admin)

### Compradores
- `GET /api/Compradores` - Listar compradores (admin)
- `GET /api/Compradores/{id}` - Obtener comprador por ID
- `POST /api/Compradores` - Crear nuevo comprador
- `PUT /api/Compradores/{id}` - Actualizar comprador
- `DELETE /api/Compradores/{id}` - Eliminar comprador (admin)

### Terrenos
- `GET /api/Terrenos` - Listar terrenos
- `GET /api/Terrenos/{id}` - Obtener terreno por ID
- `POST /api/Terrenos` - Crear nuevo terreno
- `PUT /api/Terrenos/{id}` - Actualizar terreno
- `DELETE /api/Terrenos/{id}` - Eliminar terreno

### Cultivos
- `GET /api/Cultivos` - Listar cultivos
- `GET /api/Cultivos/{id}` - Obtener cultivo por ID
- `POST /api/Cultivos` - Crear nuevo cultivo
- `PUT /api/Cultivos/{id}` - Actualizar cultivo
- `DELETE /api/Cultivos/{id}` - Eliminar cultivo

### Cosechas
- `GET /api/Cosechas` - Listar cosechas
- `GET /api/Cosechas/{id}` - Obtener cosecha por ID
- `POST /api/Cosechas` - Crear nueva cosecha
- `PUT /api/Cosechas/{id}` - Actualizar cosecha
- `DELETE /api/Cosechas/{id}` - Eliminar cosecha

### Ventas
- `GET /api/Ventas` - Listar ventas
- `GET /api/Ventas/{id}` - Obtener venta por ID
- `POST /api/Ventas` - Crear nueva venta
- `PUT /api/Ventas/{id}` - Actualizar venta
- `DELETE /api/Ventas/{id}` - Eliminar venta

## Autenticación

Todos los endpoints protegidos requieren un token JWT válido en el encabezado de autorización:

```
Authorization: Bearer <token-jwt>
```

### Flujo de Autenticación

1. Registrar usuario vía `POST /api/Auth/register`
2. Iniciar sesión vía `POST /api/Auth/login` para obtener token JWT
3. Usar el token en el encabezado `Authorization` para acceder a endpoints protegidos

## Roles de Usuarios

- **Admin**: Acceso completo al sistema, puede gestionar todos los recursos
- **Agricultor**: Puede gestionar sus propios terrenos, cultivos y cosechas
- **Comprador**: Puede gestionar sus propias compras y ver información de productos

## Contribución

1. Haz fork del proyecto
2. Crea una rama para tu feature (`git checkout -b feature/NuevaFeature`)
3. Realiza tus cambios y haz commit (`git commit -am 'Añadir NuevaFeature'`)
4. Sube tus cambios (`git push origin feature/NuevaFeature`)
5. Abre un Pull Request

## Licencia

Este proyecto está licenciado bajo la Licencia MIT - ver el archivo [LICENSE.md](LICENSE.md) para más detalles.

## Contacto

- Desarrollador: [Tu Nombre]
- Email: [tu-email@ejemplo.com]
