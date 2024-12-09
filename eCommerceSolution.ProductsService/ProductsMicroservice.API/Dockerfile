FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProductsMicroservice.API/ProductsMicroservice.API.csproj", "ProductsMicroservice.API/"]
COPY ["BusinessLogicLayer/BusinessLogicLayer.csproj", "BusinessLogicLayer/"]
COPY ["DataAccessLayer/DataAccessLayer.csproj", "DataAccessLayer/"]
RUN dotnet restore "ProductsMicroservice.API/ProductsMicroservice.API.csproj"
COPY . .
WORKDIR "/src/ProductsMicroservice.API"
RUN dotnet build "ProductsMicroservice.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProductsMicroservice.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV MYSQL_HOST=localhost
ENV MYSQL_DATABASE=eCommerceProducts
ENV MYSQL_USER=root
ENV MYSQL_PASSWORD=Purejoy@123#
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductsMicroservice.API.dll"]
