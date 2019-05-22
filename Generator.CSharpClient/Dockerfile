FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
ARG VERSION=1.0.0
WORKDIR /src

COPY . .

RUN dotnet restore "/src/Generator.CSharpClient.csproj"

RUN dotnet build "Generator.CSharpClient.csproj" /p:AssemblyVersion=$VERSION /p:Version=$VERSION -c Release -o /app

FROM build AS publish
ARG VERSION=1.0.0
RUN dotnet publish "Generator.CSharpClient.csproj" /p:AssemblyVersion=$VERSION /p:Version=$VERSION -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Generator.CSharpClient.dll"]
