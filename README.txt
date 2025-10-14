Notas actuales:

    Para correr la aplicación basta con hacer un 'docker compose up -d', dado que se incorporó el Dockerfile que se encarga de publicar la app.
    El docker-compose.yml se encarga de levantar el contenedor de postgres y luego el de la API (que utiliza  el Dockerfile).

Notas del backend previas a dockerizar todo:

- Para levantar la BD por primera vez, se debe utilizar Dcker y Entity Framework:
    0 - En la carpeta general del proyecto, levantar el docker con:
        docker compose up -d
    1 - Ubicarse en la carpeta backend
    2 - Crear la primer migración con: 
            dotnet tool run dotnet-ef migrations add "Migración inicial" (si falla, se puede probar corriendo el backend en debug con F5 y volviendo a crear la migración)
    3 - Opcionalmente, ver todas las migraciones existentes con:
            dotnet tool run dotnet-ef migrations list
    4 - Correr la migración con dotnet tool run dotnet-ef database update
    
    NOTA: en caso de querer borrar una migración creada, se puede borrar la última con:
        dotnet tool run dotnet-ef migrations remove

- Para correr la aplicación de .NET en modo debug, basta con tocar F5

- Por las dudas dejo dónde me levantó el Swagger a mí, en caso de que no se abra sólo:
    https://localhost:7166/swagger/index.html