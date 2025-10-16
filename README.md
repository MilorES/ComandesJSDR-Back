# API ComandesJSDR
ComandesJSDR és una plataforma centralitza la gestió de comandes, automatitzant processos que normalment són manuals. Gràcies a XML-UBL, permet interoperabilitat amb altres sistemes i compliment normatiu sense complicacions.

# Desenvolupament

Requereix MariaDB en funcionament. 

 La API obté les dades de conexió des de `ConnectionStrings__DefaultConnection` definida en `appsettings.json`.

```shell
# Des de ComandesAPI del repo
dotnet run
```

# Desplegament
Aquest repositori inclou un `docker-compose.yml` i `.env.template` per aixecar l'API amb MariaDB.

Requereix Docker y Docker Compose. L'API obté les dades de conexió des de `.env`. 

```shell
# Des de l'arrel del repo
cp .env.template .env
docker compose up --build -d

# Veure logs
docker compose logs -f comandesapi

# Aturar i eliminar contenidors
docker compose down
```
