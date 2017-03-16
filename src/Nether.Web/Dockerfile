FROM microsoft/aspnetcore:1.1.1

WORKDIR /app

# TODO Set to Production once we support that
ENV ASPNETCORE_ENVIRONMENT Development

# copy the published folder created using
# dotnet publish -c Release
COPY ./bin/Release/netcoreapp1.1/publish ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "Nether.Web.dll"]  