# Proyecto Firmness

## Descripción General del Sistema

El proyecto Firmness es una solución integral para la gestión de productos, clientes y ventas, diseñada para ser modular y escalable. Consta de una aplicación web (Razor Pages) para administración y una API RESTful (ASP.NET Core) para ser consumida por otros clientes, como aplicaciones Blazor o móviles.

### Componentes Principales:
- **Firmness.Web**: Aplicación web basada en Razor Pages para la administración del sistema.
- **FIrmnessAPI**: API RESTful que expone endpoints para gestionar productos, clientes y ventas.
- **Firmness.Core**: Librería de clases que contiene los modelos de dominio, la lógica de negocio y los servicios compartidos.
- **Firmness.ViewModels**: Librería de clases que contiene los Data Transfer Objects (DTOs) y ViewModels.
- **Firmness.Tests**: Proyecto de pruebas unitarias para validar la funcionalidad del sistema.

---

## 🚀 Ejecución con Docker (Método Recomendado)

Esta es la forma más sencilla y recomendada para levantar todo el entorno, ya que gestiona la base de datos y ambos servicios (Web y API) automáticamente.

1.  **Construir y Levantar los Servicios**:
    Desde la raíz del proyecto, ejecuta:
    ```bash
    docker compose up --build
    ```
    Esto construirá las imágenes de Docker, creará la base de datos, aplicará las migraciones y levantará todos los servicios.

2.  **Acceder a las Aplicaciones**:
    -   **Aplicación Web**: `http://localhost:8080`
    -   **API (Swagger UI)**: `http://localhost:8081/swagger`

    > **Nota**: La cadena de conexión en los archivos `appsettings.json` está preconfigurada para este entorno (`Host=db`).

3.  **Credenciales de Prueba (creadas por `SeedData`)**:
    -   **Administrador**:
        -   Email: `admin@firmness.com`
        -   Password: `Admin123*`
    -   **Cliente**:
        -   Email: `cliente@firmness.com`
        -   Password: `Client123*`

---

## 🔧 Ejecución Local (Sin Docker)

Si prefieres ejecutar los servicios localmente sin Docker, sigue estos pasos:

1.  **Requisitos Previos**:
    -   .NET 8 SDK
    -   PostgreSQL instalado y corriendo localmente.

2.  **Configurar la Base de Datos**:
    Modifica la cadena de conexión en **todos** los siguientes archivos para que apunte a tu base de datos local:
    -   `Firmness.Web/appsettings.Development.json`
    -   `FIrmnessAPI/appsettings.Development.json`

    Ejemplo de cadena de conexión local:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Port=5432;Database=firmness_db;Username=postgres;Password=your_password"
    }
    ```

3.  **Aplicar Migraciones**:
    Desde la raíz del proyecto, ejecuta:
    ```bash
    dotnet ef database update -s Firmness.Web
    ```

4.  **Ejecutar los Proyectos**:
    Abre dos terminales separadas.
    -   En la primera terminal, ejecuta la API:
        ```bash
        cd FIrmnessAPI
        dotnet run
        ```
    -   En la segunda terminal, ejecuta la Web:
        ```bash
        cd Firmness.Web
        dotnet run
        ```

---

## 🗃️ Gestión de Migraciones (Entity Framework)

Debido a la estructura multi-proyecto, es importante usar los comandos de `dotnet ef` con los parámetros correctos.

-   **Para Añadir una Nueva Migración**:
    Desde la raíz del proyecto, ejecuta:
    ```bash
    dotnet ef migrations add <NombreDeLaMigracion> -p Firmness.Core -s Firmness.Web
    ```
    -   `-p Firmness.Core`: Especifica que el proyecto de destino (donde se guardan las migraciones) es `Firmness.Core`.
    -   `-s Firmness.Web`: Especifica que el proyecto de inicio (que tiene la configuración) es `Firmness.Web`.

-   **Para Aplicar Migraciones a la Base de Datos**:
    ```bash
    dotnet ef database update -s Firmness.Web
    ```

---

## 🤔 Solución de Problemas (Troubleshooting)

-   **Error: `Host can't be null` o `Connection string 'Default' not found` al ejecutar `dotnet ef`**
    -   **Causa**: Este error ocurre porque la herramienta EF CLI no puede encontrar una cadena de conexión válida.
    -   **Solución**: Asegúrate de que la sección `ConnectionStrings` con el nombre `DefaultConnection` exista y sea correcta en **TODOS** los siguientes archivos:
        -   `Firmness.Web/appsettings.json`
        -   `Firmness.Web/appsettings.Development.json`
        -   `FIrmnessAPI/appsettings.json`
        -   `FIrmnessAPI/appsettings.Development.json`

---

## 🧪 Pruebas Unitarias

El proyecto `Firmness.Tests` contiene pruebas unitarias. Para ejecutarlas:

```bash
dotnet test
```

---
> **Nota Final:** Recuerda actualizar las credenciales de correo electrónico en `FIrmnessAPI/appsettings.json` para que el servicio de envío de correos funcione correctamente.
