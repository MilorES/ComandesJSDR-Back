# Guia de Configuració - ComandesAPI

Aquesta guia explica com configurar l'aplicació per als diferents entorns (Development, Production).

## Configuració d'Entorns

L'aplicació suporta múltiples entorns mitjançant fitxers de configuració i variables d'entorn.

### Fitxers de Configuració

- `appsettings.json` - Configuració base (SENSE informació sensible)
- `appsettings.Development.json` - Configuració per a desenvolupament local (NO es puja a Git)
- `appsettings.Production.json` - Configuració per a producció (segura per a Git)

## Configuració per a Desenvolupament Local

### Pas 1: Crear fitxer de configuració de desenvolupament

Copia el fitxer d'exemple i configura'l amb les teves credencials locals:

```bash
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
      "http://localhost:5173"
    ]
  }
}
```

### Pas 3: Executar l'aplicació

```bash
dotnet run
```

L'aplicació s'executarà en mode Development per defecte.

## Configuració per a Docker (Desenvolupament)

### Pas 1: Crear fitxer .env

```bash
cp .env.example .env
```

### Pas 2: Configurar variables a .env

Edita el fitxer `.env` amb les teves credencials:

```env
MYSQL_ROOT_PASSWORD=rootpassword
MYSQL_DATABASE=comandesjdsr
MYSQL_USER=userapi
MYSQL_PASSWORD=passwordapi
MYSQL_PORT=3306

DB_HOST=db
DB_NAME=comandesjdsr
DB_USER=userapi
DB_PASS=passwordapi
```

### Pas 3: Executar amb Docker Compose

```bash
docker-compose up -d
```

## Configuració per a Producció

En producció, **MAI** utilitzis fitxers de configuració per a informació sensible. Utilitza variables d'entorn.

### Variables d'Entorn Requerides

#### Connexió a Base de Dades

**Opció 1: Variables individuals**
```bash
export DB_HOST=el-teu-servidor-db.com
export DB_NAME=comandesjdsr
export DB_USER=usuari_produccio
export DB_PASS=password_segur_produccio
```

**Opció 2: Connection String completa**
```bash
export ConnectionStrings__DefaultConnection="Server=el-teu-servidor-db.com;Database=comandesjdsr;User=usuari_produccio;Password=password_segur_produccio;"
```

#### Configuració JWT

```bash
export JWT_SECRET_KEY="clau_super_secreta_de_produccio_minim_32_caracters"
export JWT_ISSUER="ComandesJSDR"
export JWT_AUDIENCE="ComandesJSDR-API"
```

#### Executar en Producció

```bash
export ASPNETCORE_ENVIRONMENT=Production
dotnet ComandesAPI.dll
```

### Azure App Service / AWS / Google Cloud

En serveis cloud, configura les variables d'entorn al panell de configuració:

**Azure App Service:**
- Configuració → Configuració de l'aplicació → Afegir nova configuració

**AWS Elastic Beanstalk:**
- Configuració → Software → Variables d'entorn

**Google Cloud Run:**
- Editar servei → Variables i secrets

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
