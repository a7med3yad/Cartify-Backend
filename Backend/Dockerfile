FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ./Cartify.Domain/*.csproj ./Cartify.Domain/
COPY ./Cartify.Application/*.csproj ./Cartify.Application/
COPY ./Cartify.Infrastructure/*.csproj ./Cartify.Infrastructure/
COPY ./Cartify.API/*.csproj ./Cartify.API/

RUN dotnet restore "./Cartify.API/Cartify.API.csproj"

COPY . .

WORKDIR /src/Cartify.API
RUN dotnet build -c Release -o /app --no-restore



FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app .

ENV ASPNETCORE_URLS="http://0.0.0.0:5000"

EXPOSE 5000

ENTRYPOINT [ "dotnet", "Cartify.API.dll" ]