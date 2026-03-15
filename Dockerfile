# Base stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copy csproj and restore as distinct layers
COPY ["ExaAtendimento.API/ExaAtendimento.API.csproj", "ExaAtendimento.API/"]
COPY ["ExaAtendimento.Application/ExaAtendimento.Application.csproj", "ExaAtendimento.Application/"]
COPY ["ExaAtendimento.Domain/ExaAtendimento.Domain.csproj", "ExaAtendimento.Domain/"]
COPY ["ExaAtendimento.InfraData/ExaAtendimento.InfraData.csproj", "ExaAtendimento.InfraData/"]
RUN dotnet restore "ExaAtendimento.API/ExaAtendimento.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/ExaAtendimento.API"
RUN dotnet build "ExaAtendimento.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ExaAtendimento.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExaAtendimento.API.dll"]
