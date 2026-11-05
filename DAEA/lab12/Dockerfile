FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Veterinaria.Api/Veterinaria.Api.csproj Veterinaria.Api/
RUN dotnet restore Veterinaria.Api/Veterinaria.Api.csproj
COPY Veterinaria.Api/ Veterinaria.Api/
RUN dotnet publish Veterinaria.Api/Veterinaria.Api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Veterinaria.Api.dll"]
