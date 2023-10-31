FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DSCC.CW1.9713.MVC/DSCC.CW1.9713.MVC.csproj", "DSCC.CW1.9713.MVC/"]
RUN dotnet restore "DSCC.CW1.9713.MVC/DSCC.CW1.9713.MVC.csproj"
COPY . .
WORKDIR "/src/DSCC.CW1.9713.MVC"
RUN dotnet build "DSCC.CW1.9713.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DSCC.CW1.9713.MVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DSCC.CW1.9713.MVC.dll"]