# ---- Etapa de build ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["RPG_Sheet_characters_admin/RPG_Sheet_characters_admin.csproj", "RPG_Sheet_characters_admin/"]
RUN dotnet restore "RPG_Sheet_characters_admin/RPG_Sheet_characters_admin.csproj"

COPY . .
WORKDIR "/src/RPG_Sheet_characters_admin"
RUN dotnet publish "RPG_Sheet_characters_admin.csproj" -c Release -o /app/publish --no-restore

# ---- Etapa de runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render injeta a variável PORT; por padrão usamos 10000 caso não venha nenhuma
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "RPG_Sheet_characters_admin.dll"]
