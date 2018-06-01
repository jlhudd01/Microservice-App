FROM microsoft/aspnetcore-build:2.0 as build
WORKDIR /app

# COPY csproj and restore distinct layers
COPY *.csproj ./
RUN dotnet restore

#copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

#copy runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "OrderWebAPI.dll" ]