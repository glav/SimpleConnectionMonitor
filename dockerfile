FROM mcr.microsoft.com/dotnet/core/runtime:3.1 as base
WORKDIR /app
ENV MONITORFILE=/var/log

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

# Set the working directory.
WORKDIR /src

# Copy the file from your host to your current location.
COPY *.cs ./
COPY *.csproj ./
COPY appsettings.json .

RUN dotnet restore 
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish "ConnectionMonitor.csproj" -c Release -o /app

#./bin/Debug/netcoreapp3.1/* /

#RUN NBNConnectionMonitor.exe

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "ConnectionMonitor.dll"]

