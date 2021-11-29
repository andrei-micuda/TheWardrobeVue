## Running migrations

For now, migrations have to be run manually using `dotnet-fm`.

```c#
dotnet-fm migrate -p Postgres  -c "User ID=postgres;Password=p@ssw0rd;Host=172.17.0.4;Port=5432;Database=the_wardrobe_integration_tests;" -a ../TheWardrobe.API.Migrations/bin/Debug/net5.0/TheWardrobe.API.Migrations.dll
```