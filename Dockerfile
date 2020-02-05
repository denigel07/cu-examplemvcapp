# build stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env
WORKDIR /app
# copy csproj file before dotnet restore, extra container cache layer 
# only dotnet restore in build container if we change packages (csproj-file)
COPY *.csproj . 
RUN dotnet restore
# copy source code
COPY . . 
# build in release mode to folder out
RUN dotnet publish -c Release -o out 
# runtime image stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
# put release build-files in runtime image /app/out 
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet","ExampleMvcApp.dll" ]
