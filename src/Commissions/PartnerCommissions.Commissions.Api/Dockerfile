FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Commissions/PartnerCommissions.Commissions.Domain/PartnerCommissions.Commissions.Domain.csproj src/Commissions/PartnerCommissions.Commissions.Domain/
COPY src/Commissions/PartnerCommissions.Commissions.Application/PartnerCommissions.Commissions.Application.csproj src/Commissions/PartnerCommissions.Commissions.Application/
COPY src/Commissions/PartnerCommissions.Commissions.Infrastructure/PartnerCommissions.Commissions.Infrastructure.csproj src/Commissions/PartnerCommissions.Commissions.Infrastructure/
COPY src/Commissions/PartnerCommissions.Commissions.Api/PartnerCommissions.Commissions.Api.csproj src/Commissions/PartnerCommissions.Commissions.Api/
RUN dotnet restore src/Commissions/PartnerCommissions.Commissions.Api/PartnerCommissions.Commissions.Api.csproj

COPY src/Commissions/ src/Commissions/
RUN dotnet publish src/Commissions/PartnerCommissions.Commissions.Api/PartnerCommissions.Commissions.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "PartnerCommissions.Commissions.Api.dll"]
