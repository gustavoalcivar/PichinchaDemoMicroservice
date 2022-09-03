FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY PichinchaDemoApi/ .
#RUN dotnet tool install --global dotnet-ef
#ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "PichinchaDemoApi.dll"]
