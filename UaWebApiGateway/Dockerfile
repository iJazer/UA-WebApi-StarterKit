# Use the official .NET 8 SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install Node.js and npm
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs

RUN npm install -g typescript
RUN npm install -g vite

RUN npm install @aas-core-works/aas-core3.0-typescript

WORKDIR /src/uarestgateway.client
COPY uarestgateway.client/package*.json ./
RUN npm ci
COPY uarestgateway.client .
RUN mkdir -p /root/.aspnet/https

RUN tsc --noEmit || true && vite build

WORKDIR /src
# Copy csproj and restore as distinct layers
COPY ["UaRestGateway.Server/*.csproj", "UaRestGateway.Server/"]
RUN dotnet restore "UaRestGateway.Server/UaRestGateway.Server.csproj"

# Copy everything else and build
COPY . .
WORKDIR /src/UaRestGateway.Server
RUN dotnet build "UaRestGateway.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UaRestGateway.Server.csproj" -c Release -o /app/publish

# Use the official .NET 8 ASP.NET runtime image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install Node.js and npm in the final image
RUN apt-get update && \
    apt-get install -y curl openssl && \
    curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs

COPY UaRestGateway.Server/Quickstarts.ReferenceClient.Config.xml ./
COPY --from=publish /app/publish .
COPY --from=build /src/uarestgateway.client /app/uarestgateway.client

ENV ASPNETCORE_URLS=http://0.0.0.0:44430
EXPOSE 44430
EXPOSE 4840
EXPOSE 46040
EXPOSE 62541
ENTRYPOINT ["dotnet", "UaRestGateway.Server.dll"]