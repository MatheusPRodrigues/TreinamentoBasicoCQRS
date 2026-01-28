FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY *.slnx ./
COPY CQRS.API/CQRS.API.csproj CQRS.API/
COPY CQRS.Application/CQRS.Application.csproj CQRS.Application/
COPY CQRS.Domain/CQRS.Domain.csproj CQRS.Domain/
COPY CQRS.Infraestructure/CQRS.Infraestructure.csproj CQRS.Infraestructure/
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
EXPOSE 80
COPY --from=build /out .
ENTRYPOINT [ "dotnet", "CQRS.API.dll" ]