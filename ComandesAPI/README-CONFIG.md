# Guia de Configuració - ComandesAPI

Aquesta guia explica com configurar l'aplicació per als diferents entorns (Development, Production).

## Configuració d'Entorns

L'aplicació suporta múltiples entorns mitjançant fitxers de configuració i variables d'entorn.

### Fitxers de Configuració

- `appsettings.json` - Configuració base (SENSE informació sensible).
- `appsettings.Development.json` - Configuració per a desenvolupament local (NO es puja a Git).
- `appsettings.Production.json` - Configuració per a producció (segura per a Git).

## Configuració per a Desenvolupament Local

### Pas 1: Crear fitxer de configuració de desenvolupament

- Requereix MariaDB en funcionament.
- Treballar al directori `/ComandesAPI`.

Copia el fitxer d'exemple i configura'l amb les teves credencials locals:
 
```shell
cp appsettings.Development.example.json appsettings.Development.json
```

### Pas 2: Editar appsettings.Development.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=comandesjdsr;User=EL_TEU_USUARI;Password=LA_TEVA_PASSWORD;"
  },
  "Jwt": {
    "SecretKey": "LA_TEVA_CLAU_SECRETA_JWT_DE_ALMENYS_32_CARACTERS",
    "Issuer": "ComandesJSDR",
    "Audience": "ComandesJSDR-API",
    "ExpirationMinutes": 60
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:3000",
      "http://localhost:5173",
      "http://localhost:4200"
    ]
  }
}
```

### Pas 3: Executar l'aplicació

```shell
# Des de /ComandesAPI
dotnet run
```

L'aplicació s'executarà en mode Development per defecte.

## Configuració per a Docker

Requereix Docker y Docker Compose.

### Pas 0: Editar docker-compose.yml

Configuració per desenvolupament
```yml
- ASPNETCORE_ENVIRONMENT=Development
```
Configuració per producció
```yml
- ASPNETCORE_ENVIRONMENT=Production
```
En producció, **MAI** utilitzis fitxers de configuració per a informació sensible. Utilitza variables d'entorn.

### Pas 1: Si no existeixen crear els fitxers 

```shell
# Des de l'arrel /
cp .env.example .env
# Des de /ComandesAPI
cp appsettings.Development.example.json appsettings.Development.json
```
**IMPORTANT** No afegir aquests fitxers al GIT `.env` ni `appsettings.Development.json`

### Pas 2: Configurar variables

Desenvolupament: edita el fitxer `/ComandesAPI/appsettings.Development.json` amb les credencials i JWT.

Producció: edita el fitxer `/.env` amb les credencials i JWT.

### Opcional: Començar des de zero
```shell
# Des de l'arrel ./
docker compose down -v --remove-orphans
```

### Pas 3: Executar amb Docker Compose

```shell
# Des de l'arrel ./
docker compose up --build -d
```
### Logs

```shell
# Tots
docker compose logs -f 
# ASPNETCORE
docker compose logs -f comandesapi
# MARIADB
docker compose logs -f mariadb
```

## Prioritat de Configuració

L'aplicació busca la configuració en el següent ordre (de major a menor prioritat):

1. **Variables d'entorn específiques** (`JWT_SECRET_KEY`, `DB_HOST`, etc.)
2. **Variables d'entorn de .NET** (`ConnectionStrings__DefaultConnection`)
3. **Fitxer appsettings.{Environment}.json** (Development, Production)
4. **Fitxer appsettings.json** (base)

## Configuració CORS

### Development
Per defecte, es permeten els orígens:
- http://localhost:3000 (React)
- http://localhost:5173 (Vite)
- http://localhost:4200 (Angular)

### Production
Configura els orígens permesos a `appsettings.Production.json`:

```json
{
  "Cors": {
    "AllowedOrigins": [
      "https://la-teva-aplicacio.com",
      "https://www.la-teva-aplicacio.com"
    ]
  }
}
```

## Logging

### Development
- Nivell: Debug
- Es mostren queries d'Entity Framework
- Errors detallats habilitats

### Production
- Nivell: Warning
- Queries d'EF minimitzades
- Errors genèrics per al client
- Detalls complets als logs del servidor

## Seguretat

⚠️ **IMPORTANT:**

1. **MAI** pugis `appsettings.Development.json` al repositori
2. **MAI** incloguis contrasenyes o secrets a `appsettings.json`
3. **SEMPRE** usa variables d'entorn en producció
4. Genera una clau JWT segura d'almenys 32 caràcters
5. Canvia totes les contrasenyes per defecte

## Resolució de Problemes

### Error: "La clau secreta JWT no està configurada"

**Solució:** Assegura't de tenir configurada la variable `JWT_SECRET_KEY` o el valor a `appsettings.Development.json`.

### Error de connexió a base de dades

**Solució:** Verifica que les variables `DB_HOST`, `DB_NAME`, `DB_USER`, `DB_PASS` estiguin correctament configurades.

### Error CORS al navegador

**Solució:** Verifica que l'origen del teu frontend estigui a la llista `Cors:AllowedOrigins` del fitxer de configuració corresponent.

## Verificar Configuració Actual

Per veure quina cadena de connexió està utilitzant l'aplicació, revisa els logs a l'iniciar:

```
Using connection string: Server=...
```

## Suport

Per a més informació sobre la configuració d'ASP.NET Core:
- [Documentació oficial de Configuration](https://learn.microsoft.com/ca-es/aspnet/core/fundamentals/configuration/)
- [Documentació de Variables d'Entorn](https://learn.microsoft.com/ca-es/aspnet/core/fundamentals/configuration/#environment-variables)
