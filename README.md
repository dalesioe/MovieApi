# 🎬 Movie Info API

API REST construida en .NET 8 que permite obtener información de películas y sugerencias similares usando la API pública de [The Movie Database (TMDB)](https://developers.themoviedb.org/3). Implementa Swagger, caching con Redis, y está preparada para ejecutarse en Docker.

---

## Características principales

- Consulta por título de película.
- Devuelve:
  - Título
  - Título original
  - Fecha de estreno
  - Sinopsis
  - Puntuación promedio
  - Lista de hasta 5 películas similares (formato: `Título (Año)`)
- Cache de resultados con Redis por 10 minutos.
- Swagger habilitado.
- Preparada para correr en contenedor vía Docker Compose.

---

## Tecnologías utilizadas

- .NET 8 / C#
- Redis (via `Microsoft.Extensions.Caching.StackExchangeRedis`)
- Swagger / Swashbuckle
- TMDB API (requiere API Key)
- Docker + Docker Compose

---

## 🛠️ Configuración del entorno

### 1. Clonar el repositorio

```bash
git clone https://github.com/dalesioe/MovieApi-Redis-Swagger-Docker.git
cd MovieApi-Redis-Swagger-Docker
```

### 2. Levantar la API + Redis con Docker

```bash
docker-compose up --build
```

La API quedará disponible en:  
[http://localhost:5001/swagger](http://localhost:5001/swagger)

---

## 🧪 Ejemplo de uso

### Endpoint:
```http
GET /api/movies/Inception
```

### Respuesta:
```json
{
  "title": "Inception",
  "originalTitle": "Inception",
  "voteAverage": 8.4,
  "releaseDate": "2010-07-15",
  "overview": "Dom Cobb es un ladrón...",
  "similarMovies": [
    "The Matrix (1999)",
    "Interstellar (2014)",
    "Tenet (2020)"
  ]
}
```

---

## 🧠 Consideraciones

- Si realizás muchas consultas, la API de TMDB puede bloquear tu clave temporalmente.
- Redis cachea los resultados para evitar llamadas repetidas.

---

## 📄 Licencia

MIT © 2025 - Desarrollado por Emiliano D’Alesio
