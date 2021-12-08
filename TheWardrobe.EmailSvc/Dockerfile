# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy everything else and build
COPY ./TheWardrobe.EmailSvc ./TheWardrobe.EmailSvc
RUN dotnet publish ./TheWardrobe.EmailSvc/TheWardrobe.EmailSvc.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
EXPOSE 80
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "TheWardrobe.EmailSvc.dll"]