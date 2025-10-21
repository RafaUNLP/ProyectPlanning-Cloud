# Base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY backend.csproj ./
RUN dotnet restore backend.csproj

COPY . ./
RUN dotnet publish backend.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final-env
WORKDIR /app

COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "backend.dll" ]