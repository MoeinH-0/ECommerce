# 🛒 Modular-Commerce

![CI/CD](https://github.com/MoeinH-0/Modular-Commerce/actions/workflows/main-pipeline.yml/badge.svg)

An e-commerce web application built with **C# and ASP.NET Core**, designed as a
**Modular Monolith** following **Clean Architecture** principles, with logically
separated read/write operations (**CQS**).

## 🏗️ Architecture

- **Modular Monolith** — each module owns its Domain and Application layers, fully independent of Infrastructure.
- **Clean Architecture** — dependencies point inward; Infrastructure is replaceable.
- **CQS** — read and write operations are logically separated.
- **Modular DbContexts** — each module manages its own EF Core `DbContext`.
- Cross-module communication happens in the Application layer.
- UI with **Razor Pages**, **Cookie-Based Authentication**, and **Role-Based Authorization**.

## 📦 Modules Overview

| Module | Responsibility |
|---|---|
| **AccountManagement** | Registration, authentication, and role management |
| **ShopManagement** | Products, categories, brands, and product images |
| **InventoryManagement** | Stock tracking and warehouse operations |
| **DiscountManagement** | Pricing strategies and discount codes |
| **BlogManagement** | Articles and content management |
| **CommentManagement** | User comments and moderation |

## 🚀 Technologies & Tools

- **Language:** C#
- **Framework:** ASP.NET Core (Razor Pages)
- **ORM:** Entity Framework Core
- **Database:** PostgreSQL
- **Architecture & Patterns:** Modular Monolith, Clean Architecture, CQS, SOLID
- **Authentication:** Cookie-Based Authentication, Role-Based Authorization
- **Containerization:** Docker & Docker Compose
- **CI/CD:** GitHub Actions (Build → Push → Auto-Deploy)

## 🚀 Getting Started

### Run with Docker (Recommended)

1. Create a `.env` file from `.env.example` and set your PostgreSQL credentials.
2. Run `docker compose up -d`.
3. Open `http://localhost:8080`.

### Local Development

1. Set up PostgreSQL and update the connection string in `appsettings.Development.json`.
2. Run `dotnet run --project ServiceHost`.

## 🔄 CI/CD & Deployment

A GitHub Actions pipeline (`.github/workflows/ci-cd.yml`) runs on every push to `main`:

1. **Build & Push** — builds the Docker image and pushes it to Docker Hub.
2. **Deploy** — copies `docker-compose.prod.yml` to the server over SSH and runs `docker compose up -d`.

Required secrets: `DOCKER_USERNAME`, `DOCKER_PASSWORD`, `SERVER_IP`, `SERVER_USERNAME`, `SERVER_SSH_KEY`.

In production, an **Nginx reverse proxy** (configured via `nginx.conf` in this repository,
running as a separate stack on the server) forwards incoming HTTP traffic to the app
container through the external `shared_network`.

> **Note:** the server must already have the `.env` file, `init.sql`, and the external
> `shared_network` created before the first deployment.
`
