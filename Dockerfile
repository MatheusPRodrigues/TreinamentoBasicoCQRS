FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY *.sln ./
COPY CQRS.API/CQRS.API.csproj CQRS.API/
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
EXPOSE 80
COPY --from=build /out .
ENTRYPOINT [ "dotnet", "CQRS.API.dll" ]