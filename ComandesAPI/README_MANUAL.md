# API de Comandes JDSR

API desenvolupada en C# amb .NET 9.0 per gestionar articles i comandes. Utilitza Entity Framework Core amb MySQL Server i inclou documentació Swagger integrada.

## Característiques

- **Framework**: .NET 9.0
- **Base de dades**: MySQL Server
- **ORM**: Entity Framework Core
- **Documentació API**: Swagger/OpenAPI
- **Missatges**: Tots en català

## Estructura del projecte

```
ComandesAPI/
├── Controllers/          # Controladors de l'API
│   └── ArticlesController.cs
├── Data/                # Context de base de dades
│   └── ComandesDbContext.cs
├── DTOs/                # Objectes de transferència de dades
│   └── ArticleDto.cs
├── Models/              # Models de dades
│   └── Article.cs
├── Migrations/          # Migracions d'Entity Framework
└── Program.cs           # Configuració de l'aplicació
```

## Configuració

### 1. Configuració de la base de dades

Modifica la cadena de connexió al fitxer `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ComandesJDSR;User=root;Password=la_teva_contrasenya;"
  }
}
```

### 2. Crear la base de dades

```bash
# Aplicar les migracions per crear la base de dades
dotnet ef database update
```

## Execució

```bash
# Executar l'aplicació
dotnet run
```

L'API estarà disponible a:
- **HTTP**: http://localhost:5128
- **Swagger UI**: http://localhost:5128/ (arrel)

## Funcionalitats disponibles

### API d'Articles (`/api/articles`)

L'API proporciona operacions CRUD completes per gestionar articles:

#### Endpoints disponibles:

- **GET `/api/articles`** - Obtenir tots els articles
  - Query parameters:
    - `categoria`: Filtrar per categoria
    - `actius`: Filtrar per articles actius (true/false)

- **GET `/api/articles/{id}`** - Obtenir un article específic per ID

- **POST `/api/articles`** - Crear un nou article
  ```json
  {
    "nom": "Nom de l'article",
    "descripcio": "Descripció opcional",
    "preu": 99.99,
    "estoc": 10,
    "categoria": "Categoria",
    "actiu": true
  }
  ```

- **PUT `/api/articles/{id}`** - Actualitzar un article existent
  ```json
  {
    "nom": "Nou nom",
    "descripcio": "Nova descripció",
    "preu": 89.99,
    "estoc": 15,
    "categoria": "Nova categoria",
    "actiu": false
  }
  ```

- **DELETE `/api/articles/{id}`** - Eliminar un article

- **GET `/api/articles/categories`** - Obtenir totes les categories disponibles

### Validacions implementades

- **Nom**: Obligatori, màxim 100 caràcters, únic
- **Descripció**: Opcional, màxim 500 caràcters
- **Preu**: Obligatori, superior a 0
- **Estoc**: Obligatori, no negatiu
- **Categoria**: Opcional, màxim 20 caràcters

### Dades d'exemple

L'API inclou dades d'exemple (seed data) amb 3 articles d'informàtica per facilitar les proves.

## Tecnologies utilitzades

- **.NET 9.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 9.0**
- **Pomelo.EntityFrameworkCore.MySql 9.0**
- **Swashbuckle.AspNetCore 9.0** (Swagger)
- **MySQL Server**

## Pròxims passos

Aquest projecte està preparat per expandir-se amb:
- Autenticació i autorització
- API de comandes
- API d'usuaris/clients
- Sistema de notificacions
- Integració amb sistemes de pagament

## Enllaços útils

- [Documentació de .NET](https://docs.microsoft.com/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Swagger/OpenAPI](https://swagger.io/)