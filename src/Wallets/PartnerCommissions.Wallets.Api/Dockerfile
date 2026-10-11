FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Wallets/PartnerCommissions.Wallets.Domain/PartnerCommissions.Wallets.Domain.csproj src/Wallets/PartnerCommissions.Wallets.Domain/
COPY src/Wallets/PartnerCommissions.Wallets.Application/PartnerCommissions.Wallets.Application.csproj src/Wallets/PartnerCommissions.Wallets.Application/
COPY src/Wallets/PartnerCommissions.Wallets.Infrastructure/PartnerCommissions.Wallets.Infrastructure.csproj src/Wallets/PartnerCommissions.Wallets.Infrastructure/
COPY src/Wallets/PartnerCommissions.Wallets.Api/PartnerCommissions.Wallets.Api.csproj src/Wallets/PartnerCommissions.Wallets.Api/
RUN dotnet restore src/Wallets/PartnerCommissions.Wallets.Api/PartnerCommissions.Wallets.Api.csproj

COPY src/Wallets/ src/Wallets/
RUN dotnet publish src/Wallets/PartnerCommissions.Wallets.Api/PartnerCommissions.Wallets.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "PartnerCommissions.Wallets.Api.dll"]
