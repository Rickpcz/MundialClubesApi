# MundialClubesApi (API de Fútbol Mundial)

Este proyecto proporciona una API REST construida en **.NET 8 + Entity Framework Core** para consultar datos de fútbol de manera global. Aunque originalmente fue diseñada para el Mundial de Clubes, ha evolucionado a una plataforma general que cubre:

- Partidos y resultados (fixtures)
- Equipos y jugadores
- Ligas y competencias

Los datos se obtienen desde **API-Football** y se almacenan en **MySQL** para evitar sobreuso de peticiones externas.

---

## Objetivo del Proyecto

Diseñar una **plataforma de consultas públicas de fútbol** que permita al frontend consumir datos deportivos organizados por secciones funcionales, con filtros intuitivos y sin requerir cuentas de usuario.

---

## Tecnologías Usadas

- **.NET 8 / ASP.NET Core Web API**
- **Entity Framework Core + Pomelo MySQL**
- **API-Football (RapidAPI)**
- **MySQL**
- **Swagger** para pruebas de endpoints

---

## Estructura de la API

### Ligas y Competencias

- `GET /api/ligas`: Todas las ligas
- `GET /api/ligas/{id}`: Liga por ID
- `GET /api/ligas/pais/{pais}`: Ligas por país
- `GET /api/ligas/tipo/{tipo}`: Ligas por tipo (ej. "Cup", "League")
- `GET /api/ligas/{id}/equipos`: Equipos de una liga
- `GET /api/ligas/{id}/partidos?season=2024`: Partidos de una liga por temporada

### Equipos y Plantillas

- `GET /api/equipos`: Todos los equipos
- `GET /api/equipos/{id}`: Detalle de equipo
- `GET /api/equipos/por-liga/{ligaId}`: Equipos por liga
- `GET /api/equipos/por-pais/{pais}`: Equipos por país
- `GET /api/equipos/{id}/jugadores`: Jugadores del equipo
- `GET /api/equipos/{id}/partidos`: Partidos del equipo

### Partidos y Resultados

- `GET /api/partidos`: Todos los partidos
- `GET /api/partidos/{id}`: Partido por ID
- `GET /api/partidos/por-liga/{ligaId}/{season}`: Partidos de una liga y temporada
- `GET /api/partidos/por-fecha/{fecha}`: Partidos por fecha
- `GET /api/partidos/estado/{estado}`: Partidos por estado (ej. "En juego", "Finalizado")
- `GET /api/partidos/detalle/{fixtureId}`: Alineaciones, estadísticas y eventos del partido

---

## Objetivos para el Frontend

El frontend deberá estar dividido en al menos **3 secciones principales**, con el siguiente enfoque:

### 1. **Partidos y Resultados**
Permite visualizar los resultados y detalles de cada encuentro.

- Funciones clave:
    -   Filtrar por fecha, liga, o estado del partido (en juego, terminado, próximo).
    -   Ver detalles de cada partido, incluyendo alineaciones, estadísticas y eventos.
    -   Resultado y goles.
    -   Alineaciones iniciales.
    -   Eventos importantes (goles, tarjetas, sustituciones).
    -   Estadísticas del encuentro.

### 2. **Equipos y Plantillas**
Visualiza la información de los equipos registrados.

- Funciones clave:
    -   Buscar equipos por país o liga.
    -   Acceder a la ficha del equipo:
        -   Nombre, país, logo.
        -   Jugadores registrados (nombre, número, posición, foto).
        -   Estadísticas del equipo.

### 3. **Ligas y Competencias**
Explora todas las competencias disponibles.

-  Funciones clave:
    -   Buscar ligas por país o tipo.
    -   Ver equipos de la liga.
        -   Nombre, país, logo.
        -   Jugadores registrados (nombre, número, posición, foto).
---

## Instrucciones de ejecución

1. Clonar el repositorio:

   ```bash
   git clone https://github.com/Rickpcz/MundialClubesApi.git

   ```

2. Restaurar paquetes
   ```bash
   dotnet restore
   ```
3. Configurar la cadena de conexión a la base de datos en `appsettings.json`.
   - Ejemplo:
     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "server=sql3.freesqldatabase.com;database=sql3785780;user=sql3785780;password=secret;port=3306;",
         "DefaultConnection": "Server=localhost;Database=futbol;User=root;Password=1234;SslMode=Preferred;port=3306;"
       }
     }
     ```
4. Aplicar migraciones (si usas EF Core):
   ```bash
   dotnet ef database update
   ```
5. Ejecutar la API:
   ```bash
   dotnet run
   ```
