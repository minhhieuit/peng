FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Bootstrapper/Peng.API/Peng.API.csproj", "src/Bootstrapper/Peng.API/"]
COPY ["src/SharedKernel/Peng.SharedKernel/Peng.SharedKernel.csproj", "src/SharedKernel/Peng.SharedKernel/"]
COPY ["src/Modules/Identity/Peng.Modules.Identity.Domain/Peng.Modules.Identity.Domain.csproj", "src/Modules/Identity/Peng.Modules.Identity.Domain/"]
COPY ["src/Modules/Identity/Peng.Modules.Identity.Application/Peng.Modules.Identity.Application.csproj", "src/Modules/Identity/Peng.Modules.Identity.Application/"]
COPY ["src/Modules/Identity/Peng.Modules.Identity.Infrastructure/Peng.Modules.Identity.Infrastructure.csproj", "src/Modules/Identity/Peng.Modules.Identity.Infrastructure/"]

RUN dotnet restore "src/Bootstrapper/Peng.API/Peng.API.csproj"

COPY . .
WORKDIR "/src/src/Bootstrapper/Peng.API"
RUN dotnet build "Peng.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Peng.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Peng.API.dll"]
