# Golobe Travel API 🌍

A ASP.NET Core Web API backend for the **Golobe Travel** web app featuring hotel search, bookings, favorites, and user authentication via AWS Cognito.

---

## ✨ Features
- 📦 **RESTful API** using ASP.NET Core 8
- 🐘 **PostgreSQL** database with EF Core ORM
- 📄 **Swagger (OpenAPI)** for testing and documentation
- 📧 **Email notifications** via Resend API
- 🐳 **Dockerized** for local development and cloud deployment

---

## 🏗️ Tech Stack

| Layer       | Tech                        |
|-------------|-----------------------------|
| Backend     | ASP.NET Core Web API (C#)   |
| Database    | PostgreSQL                  |
| Auth        | AWS Cognito                 |
| ORM         | Entity Framework Core       |
| Docs/Test   | Swagger / xUnit             |
| Email       | Resend                      |
| DevOps      | Docker + EC2/Beanstalk (AWS)|

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker](https://www.docker.com/)
- [PostgreSQL](https://www.postgresql.org/)
- AWS credentials if using Cognito/Resend

### Clone the repo

```bash
git clone https://github.com/KhomotsoTibane/golobeTravelApi.git
cd golobeTravelApi
```

### Run with Docker

```bash
docker-compose up --build
```

> Make sure your `.env` file includes the correct connection strings and AWS settings.

---

## 🧪 Testing

```bash
dotnet test
```

Unit and integration tests written using **xUnit**.  
Testcontainers used to spin up temporary PostgreSQL instances.

---

## 📬 Email Integration

Resend is used to send:
- ✅ Welcome emails
- 🧾 Booking confirmations

---

## 🛠️ Project Structure

```
/GolobeTravelApi
├── Controllers
├── Data
├── Dtos
├── Helpers
├── InitialSeedData
├── Migrations
├── Models
├── Services
├── Program.cs
├── Dockerfile
└── docker-compose.yml

/GolobeTravelApi.Tests
├── CutsomWebApplicationFactory
├── UnitTest1
```

---

## 📦 Deployment

Supports deployment to:
- **AWS EC2**
- **Elastic Beanstalk**
- **Docker Hub**

Make sure to update environment variables and connection strings in your deployment pipeline.

---

## 🙋🏽‍♂️ Author

**Khomotso Tibane**  
💼 [LinkedIn](https://www.linkedin.com/in/khomotsotibane) | 🧑🏽‍💻 [GitHub](https://github.com/KhomotsoTibane)

---

## 📄 License

MIT — feel free to use this project as a base for your own travel platform!
