# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 25

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/mssql/server
# Create work directory
RUN mkdir -p /usr/work
WORKDIR /app
# Copy all scripts into working directory
COPY . .
# Grant permissions for the import-data script to be executable
#RUN chmod +x /usr/work/import-data.sh
#EXPOSE 1433
#CMD /bin/bash ./entrypoint.sh

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS http://*:9096
ENTRYPOINT ["dotnet", "NRH_UserAdress.dll"]
