# Task Manager — PSP

Sistema de gestión de tareas desarrollado como proyecto de portafolio bajo la metodología PSP (Personal Software Process).

## Stack Tecnológico

| Capa | Tecnología |
|------|-----------|
| Frontend | Angular 17 — standalone components, SCSS, Tailwind CSS, Angular Material |
| Backend | .NET 10 Web API (C#) |
| Base de datos | PostgreSQL 15 via Docker |
| ORM | Entity Framework Core con Npgsql |
| Autenticación | JWT con BCrypt |
| Documentación API | Scalar (reemplaza Swashbuckle, incompatible con .NET 10) |

## Requisitos Previos

- [Node.js 20+](https://nodejs.org/)
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Angular CLI 17+](https://angular.io/cli) — `npm install -g @angular/cli`
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/)

## Estructura del Proyecto

```
Task_manager/
├── Backend/
│   ├── Task_manager_API/       # Web API principal (.NET 10)
│   │   ├── Controllers/        # UserController, TasksController, CategoriesController
│   │   ├── Data/               # AppDbContext
│   │   ├── DTOs/               # DTOs de entrada y salida
│   │   └── Migrations/
│   └── Task_manager.Core/
│       └── Entities/           # User, TaskItem, Category
├── Frontend/
│   └── src/
│       └── Task_manager-ui/
│           └── app/
│               ├── core/
│               │   ├── guards/         # auth.guard.ts
│               │   ├── interceptors/   # jwt.interceptor.ts, error.interceptor.ts
│               │   ├── models/         # task.model.ts, user.model.ts
│               │   └── services/       # auth.service.ts, task.service.ts
│               ├── features/
│               │   ├── auth/           # login, register
│               │   └── tasks/          # task-list, task-form
│               └── shared/
│                   └── components/
│                       └── navbar/
├── psp-docs/                   # TRL, DRL, PPS
├── docker-compose.yml
└── README.md
```

## Instalación y Setup

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/task-manager.git
cd task-manager
```

### 2. Levantar la base de datos con Docker

```bash
docker-compose up -d
```

Esto levanta PostgreSQL en el puerto `5432` y pgAdmin en `http://localhost:5050`.

Credenciales pgAdmin:
- Email: `admin@admin.com`
- Password: `admin`

Credenciales PostgreSQL:
- Host: `localhost` | Puerto: `5432`
- Usuario: `postgres` | Password: `postgres123`
- Base de datos: `taskmanagerdb`

### 3. Configurar el backend

Crea el archivo `Backend/Task_manager_API/appsettings.Development.json` (no incluido en el repo por seguridad):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=taskmanagerdb;Username=postgres;Password=postgres123"
  },
  "JwtSettings": {
    "SecretKey": "tu-clave-secreta-de-al-menos-32-caracteres",
    "Issuer": "TaskManagerAPI",
    "Audience": "TaskManagerClient",
    "ExpirationMinutes": 60
  }
}
```

Aplicar migraciones y correr la API:

```bash
cd Backend/Task_manager_API
dotnet ef database update
dotnet run
```

La API estará disponible en `https://localhost:7246`.
Documentación Scalar: `https://localhost:7246/scalar/v1`

### 4. Configurar el frontend

```bash
# Desde la raíz del proyecto
npm install
ng serve
```

La app estará disponible en `http://localhost:4200`.

> Importante: correr `ng serve` desde la raíz del proyecto donde está `angular.json`, no desde dentro de `Frontend/`.

## Endpoints principales

### Usuarios — `/api/User`
| Método | Ruta | Descripción |
|--------|------|-------------|
| POST | `/api/User` | Registro de usuario |
| POST | `/api/User/Login` | Login — retorna JWT |
| GET | `/api/User` | Lista todos los usuarios |

### Tareas — `/api/Task`
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Task` | Lista tareas del usuario autenticado |
| POST | `/api/Task` | Crear tarea |
| PUT | `/api/Task/{id}` | Actualizar tarea |
| DELETE | `/api/Task/{id}` | Eliminar tarea |

### Categorías — `/api/Category`
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Category` | Lista categorías del usuario |
| POST | `/api/Category` | Crear categoría |
| DELETE | `/api/Category/{id}` | Eliminar categoría |

> Todos los endpoints de tareas y categorías requieren JWT en el header `Authorization: Bearer <token>`.

## Variables de entorno del frontend

Archivo `Frontend/src/Task_manager-ui/environments/environment.development.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7246/api'
};
```

## Estado de Ciclos PSP

| Ciclo | Módulo | Estado |
|-------|--------|--------|
| 1 | Setup + Docker + DB Schema | ✅ Completado |
| 2 | Auth API (UserController) | ✅ Completado |
| 3 | TasksController CRUD | ✅ Completado |
| 4 | CategoriesController | ✅ Completado |
| 5 | Frontend AuthService + Login + Register | ✅ Completado |
| 6 | Frontend Tasks Module (task-list, task-form, navbar) | ✅ Completado |
| 7 | Frontend Dashboard | 🔄 Pendiente |
| 8 | Tests xUnit + CI/CD | 🔄 Pendiente |

## Notas de desarrollo

- Iniciar siempre Docker Desktop antes de levantar el proyecto
- Hacer commit antes de cambiar de rama para evitar conflictos
- Rama activa: `development`
- Los endpoints de prueba en `Program.cs` deben limpiarse antes del deploy
- El refresh token está pospuesto para un ciclo posterior
