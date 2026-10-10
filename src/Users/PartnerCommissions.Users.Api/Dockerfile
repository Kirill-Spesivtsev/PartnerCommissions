FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Users/PartnerCommissions.Users.Domain/PartnerCommissions.Users.Domain.csproj src/Users/PartnerCommissions.Users.Domain/
COPY src/Users/PartnerCommissions.Users.Application/PartnerCommissions.Users.Application.csproj src/Users/PartnerCommissions.Users.Application/
COPY src/Users/PartnerCommissions.Users.Infrastructure/PartnerCommissions.Users.Infrastructure.csproj src/Users/PartnerCommissions.Users.Infrastructure/
COPY src/Users/PartnerCommissions.Users.Api/PartnerCommissions.Users.Api.csproj src/Users/PartnerCommissions.Users.Api/
RUN dotnet restore src/Users/PartnerCommissions.Users.Api/PartnerCommissions.Users.Api.csproj

COPY src/Users/ src/Users/
RUN dotnet publish src/Users/PartnerCommissions.Users.Api/PartnerCommissions.Users.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "PartnerCommissions.Users.Api.dll"]
