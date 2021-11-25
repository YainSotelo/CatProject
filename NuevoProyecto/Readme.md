## Cat Project

# API

Paquetes Nuget utilizados

Version: Instalar la mas reciente

- Swashbuckle.AspNetCore.Swagger
- Swashbuckle.AspNetCore.SwaggerUI
- Swashbuckle.AspNetCore.SwaggerGen

Referencia: https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio

# Acceso a datos

Procedimiento

1. Instalar Entity Framework Core localmente

Desde el CMD o Simbolo de Sistema en Windows

Referencia: https://docs.microsoft.com/en-us/ef/core/cli/dotnet

Instalacion
```
dotnet tool install --global dotnet-ef
```

2. Instalar Paquetes Nuget utilizados en el proyecto C#

Version: 3.1.21 (Para todos)

- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer

3. Realizar Scaffold desde el CMD o Simbolo de Sistema en el directorio del proyecto (donde se encuentra el archivo Catproject.csproj)

Scaffold
```
dotnet ef dbcontext scaffold <CONNECTION_STRING> Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context GatosContexto
```

Referencia: https://docs.microsoft.com/en-us/ef/core/cli/dotnet#dotnet-ef-dbcontext-scaffold

4. Borrar credenciales en el archivo de la clase del Contexto (GatosContexto.cs) en el método **OnConfiguring** dentro del **if**

5. Colocar el connection string en el archivo **appsettings.json** con un idenficador asociado ("gatos_db")

6. Realizar la configuracion del archivo Startup.cs

# JWT

Instalar Paquetes Nuget utilizados en el proyecto C#

Version: 8.6.0

- JWT

Referencia: https://github.com/jwt-dotnet/jwt