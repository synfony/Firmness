# Proyecto Firmness

## Descripción General del Sistema

El proyecto Firmness es una solución integral para la gestión de productos, clientes y ventas, diseñada para ser modular y escalable. Consta de una aplicación web (Razor Pages) para administración y una API RESTful (ASP.NET Core) para ser consumida por otros clientes, como aplicaciones Blazor o móviles.

### Componentes Principales:
- **Firmness.Web**: Aplicación web basada en Razor Pages para la administración del sistema.
- **FIrmnessAPI**: API RESTful que expone endpoints para gestionar productos, clientes y ventas. Incluye autenticación JWT y un servicio de correo electrónico.
- **Firmness.Core**: Librería de clases que contiene los modelos de dominio, la lógica de negocio central y los servicios compartidos (como `PdfService` y `IEmailService`).
- **Firmness.ViewModels**: Librería de clases que contiene los Data Transfer Objects (DTOs) y ViewModels utilizados por la API y la aplicación web.
- **Firmness.Tests**: Proyecto de pruebas unitarias para validar la funcionalidad del sistema.

## Características Clave

- **Gestión de Usuarios y Roles**: Autenticación y autorización basada en ASP.NET Core Identity con roles de "Administrator" y "Client".
- **API RESTful**: Endpoints para operaciones CRUD en productos, clientes y ventas.
- **Autenticación JWT**: Seguridad para la API mediante JSON Web Tokens.
- **Servicio de Correo Electrónico**: Envío de notificaciones (ej. bienvenida) a través de SMTP (configurado para Gmail).
- **Generación de PDFs**: Creación de recibos de venta en formato PDF.
- **Documentación de API**: Integración con Swagger/OpenAPI para documentación interactiva.
- **Base de Datos**: PostgreSQL.

## Diagramas Técnicos

Los diagramas técnicos son fundamentales para comprender la arquitectura y el diseño del sistema. Se recomienda generar y colocar los siguientes diagramas en una carpeta `docs/diagrams` en la raíz del proyecto:

- **Modelo Entidad-Relación (ERD)**: Muestra la estructura de la base de datos, incluyendo tablas, relaciones y atributos.
- **Diagrama de Clases**: Representa la estructura estática de las clases del proyecto, sus atributos, métodos y las relaciones entre ellas.

**Herramientas Sugeridas para Diagramas:**
- **Lucidchart, draw.io (Diagrams.net)**: Para ERD y Diagramas de Clases.
- **Visual Studio (Code Maps)**: Para generar diagramas de clases a partir del código.

## Instalación y Ejecución

### Requisitos Previos

- .NET 8 SDK
- Docker y Docker Compose
- PostgreSQL (si no se usa Docker para la base de datos)

### Configuración Local (sin Docker)

1.  **Clonar el Repositorio**:
    ```bash
    git clone [URL_DEL_REPOSITORIO]
    cd Firmness
    ```
2.  **Configurar la Base de Datos**:
    Asegúrate de que tu instancia de PostgreSQL esté corriendo.
    Actualiza la cadena de conexión en `appsettings.json` de `Firmness.Web` y `FIrmnessAPI` para que apunte a tu base de datos local.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Port=5432;Database=firmness_db;Username=postgres;Password=your_password"
    }
    ```
3.  **Aplicar Migraciones y Seed Data**:
    Abre una terminal en la carpeta `Firmness.Web` y ejecuta:
    ```bash
    dotnet ef database update
    ```
    Esto creará la base de datos y aplicará las migraciones. El `SeedData` se ejecutará automáticamente al iniciar la aplicación web.
4.  **Ejecutar la Aplicación Web**:
    En la carpeta `Firmness.Web`:
    ```bash
    dotnet run
    ```
    La aplicación estará disponible en `http://localhost:8080` (o el puerto configurado).
5.  **Ejecutar la API**:
    En la carpeta `FIrmnessAPI`:
    ```bash
    dotnet run
    ```
    La API estará disponible en `http://localhost:5000` o `https://localhost:5001` (o los puertos configurados).

### Ejecución con Docker Compose

La forma recomendada de ejecutar el proyecto es usando Docker Compose, ya que gestiona la base de datos y ambos servicios (Web y API) automáticamente.

1.  **Construir y Levantar los Servicios**:
    Desde la raíz del proyecto (`/home/Coder/Escritorio/Firmness/`):
    ```bash
    docker compose up --build
    ```
    Esto construirá las imágenes de Docker para la API y la Web, y levantará todos los servicios definidos en `docker-compose.yml`, incluyendo la base de datos PostgreSQL.
2.  **Acceder a las Aplicaciones**:
    -   **Aplicación Web**: `http://localhost:8080`
    -   **API (Swagger UI)**: `http://localhost:5000/swagger` (o `https://localhost:5001/swagger` si está configurado para HTTPS)

    **Credenciales de Prueba (creadas por SeedData):**
    -   **Administrador**:
        -   Email: `admin@firmness.com`
        -   Password: `Admin123*`
    -   **Cliente**:
        -   Email: `cliente@firmness.com`
        -   Password: `Client123*`

## Consumo de la API y Pruebas de Endpoints

La API está documentada usando Swagger/OpenAPI, lo que permite explorar y probar los endpoints directamente desde el navegador.

1.  **Acceder a Swagger UI**:
    Una vez que la API esté corriendo (ya sea localmente o con Docker), abre tu navegador y ve a `http://localhost:5000/swagger` (o el puerto HTTPS si aplica).
2.  **Autenticación JWT en Swagger**:
    -   Haz clic en el botón **"Authorize"** en la parte superior derecha de la interfaz de Swagger.
    -   En el diálogo que aparece, utiliza el endpoint `/api/Auth/login` para obtener un token JWT.
    -   Copia el token (solo la cadena del token, sin "Bearer ").
    -   Pega el token en el campo de valor (con el prefijo "Bearer ", ej. `Bearer eyJ...`) y haz clic en "Authorize".
    -   Ahora podrás probar los endpoints protegidos por autenticación.
3.  **Probar Endpoints**:
    -   Expande cualquier endpoint (ej. `GET /api/Products`).
    -   Haz clic en "Try it out".
    -   Haz clic en "Execute".
    -   Verás la respuesta de la API, incluyendo el código de estado y el cuerpo de la respuesta.

## Pruebas Unitarias

El proyecto `Firmness.Tests` contiene pruebas unitarias para componentes clave del sistema.

1.  **Ejecutar Pruebas**:
    Desde la raíz del proyecto, o desde la carpeta `Firmness.Tests`:
    ```bash
    dotnet test
    ```
    Esto ejecutará todas las pruebas unitarias y mostrará los resultados en la consola.

---
**Nota:** Recuerda actualizar las credenciales de correo electrónico en `FIrmnessAPI/appsettings.json` para que el servicio de envío de correos funcione correctamente.
