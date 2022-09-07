FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY PichinchaDemo.sln .
COPY PichinchaDemoApi/PichinchaDemoApi.csproj ./PichinchaDemoApi/
COPY PichinchaDemoApi.Test/PichinchaDemoApi.UnitTest.csproj ./PichinchaDemoApi.Test/
RUN dotnet restore

COPY PichinchaDemoApi/ ./PichinchaDemoApi/
COPY PichinchaDemoApi.Test/ ./PichinchaDemoApi.Test
RUN dotnet build

FROM build AS testrunner
WORKDIR /app/PichinchaDemoApi.Test
CMD ["dotnet", "test", "--logger:trx"]

FROM build AS publish
WORKDIR /app/PichinchaDemoApi
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=publish /app/PichinchaDemoApi/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "PichinchaDemoApi.dll"]
