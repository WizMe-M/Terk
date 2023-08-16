# Terk

Client-server app that allows user to create and view orders

## Deploy

> How to deploy applications?

1. Start Docker Engine
2. Open some terminal in solution's root (`C:\some_path\..\Terk\`)
3. Call `docker compose build`
4. Call `docker compose up -d`
5. Make sure MS SQL Server has Terk's database initialized 
   - Check section "**How to init _Terk_ database?**" below
6. Start `Terk.DesktopClient`

---

> What if I don't want to pull heavy (~3Gb) MS SQL Server image from docker hub? 
> How to connect API to database that is not being hosted in a docker container?

1. Remove service `db` (and `depends on: db` from `api`) from `docker-compose.yml`
2. Change connection string in `api` (`environment: CONNECTIONSTRINGS__DEFAULT`) to connect to your SQL Server

---

> How to init _Terk_ database?

1. Init-db script is located at `\Terk\Terk.DB\Scripts\init_db.sql`
2. Open MS SQL Server Management studio on your server
   - If SQL Server is being hosted in container: 
     - Connect to server in container (connection string in `docker-compose.yml`)
   - If SQL Server is being hosted somewhere else (e.g. on localhost, where desktop client will run)
     - Connect to your server
3. Create new query
4. Copy-paste `init_db.sql` script into MS SQL Server Management Studio
5. Execute the query