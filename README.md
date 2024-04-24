# Documentación del Backend - Aplicación de Gestión de Empleos

Este documento describe el funcionamiento y las características del backend de la aplicación de gestión de empleos.

## Tecnologías Utilizadas

- **ASP.NET Core**: Framework de desarrollo web de código abierto, multiplataforma y de alto rendimiento para la construcción de aplicaciones modernas basadas en la nube.

- **Entity Framework Core**: ORM (Object-Relational Mapper) para .NET que permite trabajar con bases de datos relacionales utilizando objetos.

- **SQL Server**: Sistema de gestión de bases de datos relacional desarrollado por Microsoft.

## Configuración del Proyecto

1. Clona este repositorio

2. Abre el proyecto en Visual Studio o cualquier otro editor de código compatible con ASP.NET Core.

3. Configura la conexión a la base de datos SQL Server en el archivo `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=tu-servidor;Database=tu-base-de-datos;User Id=tu-usuario;Password=tu-contraseña;"
     }
   }
4. En la consola de administrador de paquetes nuget ejecuta los siguientes comandos:

4.1: Antes de ejecutarlo verifica que en la consola como proyecto prederminado: PortalEmpleoDB

4.2: Add-Migration "CualquierNombre"

4.3: Update-Database
