# API de Gestión de Empleados

API REST desarrollada con ASP.NET Core para la administración de empleados, incluyendo manejo de imágenes, departamentos, puestos y estados.

##  Tecnologías utilizadas

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* Arquitectura por capas
* DTOs (Data Transfer Objects)

## Funcionalidades

* CRUD completo de empleados
* Gestión de departamentos, puestos y estados
* Subida y almacenamiento de imágenes
* Relación entre empleados e imágenes
* Uso de DTOs para desacoplar entidades

##  Estructura del proyecto

* **Controllers** → Endpoints de la API
* **Models** → Entidades de base de datos
* **Dtos** → Objetos de transferencia de datos
* **Data/DbContext** → Configuración de EF Core

##  Endpoints principales

### Empleados

* `GET /api/empleados` → Listar empleados
* `GET /api/empleados/{id}` → Obtener empleado por ID
* `POST /api/empleados` → Crear empleado
* `PUT /api/empleados/{id}` → Editar empleado
* `DELETE /api/empleados/{id}` → Eliminar empleado

### Imágenes

* `POST /api/imagenes` → Registrar imagen

##  Manejo de imágenes

Las imágenes se almacenan físicamente en el servidor (`wwwroot/images`) y sus metadatos se guardan en la base de datos.

Cada empleado está relacionado con una imagen mediante `IdImagen`.

##  Configuración

1. Clonar el repositorio
2. Configurar la cadena de conexión en `appsettings.json`
3. Ejecutar migraciones:

   ```bash
   dotnet ef database update
   ```
4. Ejecutar la API:

   ```bash
   dotnet run
   ```

##  Buenas prácticas implementadas

* Separación de responsabilidades
* Uso de DTOs
* Manejo de relaciones con Entity Framework
* Validación de datos

##  Autor

Proyecto desarrollado como práctica de desarrollo backend con ASP.NET Core.
