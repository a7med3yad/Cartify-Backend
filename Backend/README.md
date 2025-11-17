# 🛒 Cartify — Full-Stack E-Commerce Platform  

[![License](https://img.shields.io/github/license/Taqey/Cartify?style=flat-square)](./LICENSE)
[![Issues](https://img.shields.io/github/issues/Taqey/Cartify?style=flat-square)](https://github.com/Taqey/Cartify/issues)
[![Pull Requests](https://img.shields.io/github/issues-pr/Taqey/Cartify?style=flat-square)](https://github.com/Taqey/Cartify/pulls)
[![Stars](https://img.shields.io/github/stars/Taqey/Cartify?style=flat-square)](https://github.com/Taqey/Cartify/stargazers)
[![Forks](https://img.shields.io/github/forks/Taqey/Cartify?style=flat-square)](https://github.com/Taqey/Cartify/network/members)
[![.NET](https://img.shields.io/badge/Backend-.NET%209-68217A?logo=dotnet&style=flat-square)](#)
[![DB](https://img.shields.io/badge/Database-SQL%20Server-CC2927?logo=microsoftsqlserver&style=flat-square)](#)
[![Frontend](https://img.shields.io/badge/Frontend-HTML%20%7C%20CSS%20%7C%20JS-333?logo=html5&style=flat-square)](#)
[![CI](https://img.shields.io/badge/CI-GitHub%20Actions-2088FF?logo=githubactions&style=flat-square)](#)
[![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker&style=flat-square)](#)

---

## 🧩 Overview

**Cartify** is a full-stack **E-Commerce web application** built with **ASP.NET Core** and **Clean Architecture**.  
It provides a seamless shopping experience for customers, efficient store management tools for merchants, and powerful administrative control for platform admins.  

The project focuses on:
- **Scalability**
- **Maintainability**
- **Layered separation of concerns**
- **Reusable and clean code**

---

## ✨ Features

### 👤 Customer Portal
- 🏠 **Home Page:** Browse latest products, categories, and deals.  
- 🗂 **Categories:** Filter and sort products dynamically.  
- 📦 **Product Details:** View specifications, reviews, and ratings.  
- 🛒 **Cart Management:** Add, remove, and update cart items; apply coupons.  
- 💳 **Checkout:** Secure payment (Cash on Delivery, Card, Wallet).  
- 🚚 **Order Tracking:** View live order status and history.  
- 💖 **Wishlist:** Save products for future purchases.  
- ⚙️ **Profile Settings:** Manage personal data, addresses, and payment info.  
- 🔔 **Notifications:** Stay updated with offers and order updates.  
- 💬 **Support Center:** FAQs, support tickets, and live chat integration.  

### 🏬 Merchant Dashboard
- 🏢 **Store Registration & Profile Management.**  
- 📊 **Dashboard:** Sales, orders, and inventory overview with charts.  
- 🛍️ **Product Management:** Add, edit, and delete products and variants.  
- 📦 **Order Management:** Manage customer orders efficiently.  
- 💹 **Analytics:** Insights into top-selling products and revenue.  

### 🧑‍💻 Admin Panel
- 🖥️ **Global Dashboard:** Monitor users, merchants, and overall sales.  
- 👥 **User & Store Management:** Approve, suspend, or delete accounts.  
- 📦 **Product & Order Control:** Manage platform-wide listings and transactions.  
- 🗂 **Category Management:** Add and organize categories/subcategories.  
- 📑 **Reports & Analytics:** Export CSV/Excel reports and apply filters.  
- ⚙️ **Settings:** Configure payments, shipping, and policy preferences.  

---

## 🧱 Architecture Overview

Cartify follows **Clean Architecture** and **SOLID principles**, divided into four main layers:

```
┌─────────────────────────────────────────────────────────────┐
│                     Presentation Layer                      │
│              (API Controllers, Middlewares)                 │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                    Application Layer                        │
│         (Business Logic, DTOs, Services, CQRS)              │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                      Domain Layer                           │
│         (Entities, Value Objects, Domain Logic)             │
└─────────────────────────────────────────────────────────────┘
                              ↑
┌─────────────────────────────────────────────────────────────┐
│                  Infrastructure Layer                       │
│     (EF Core, Repositories, External Services, Data)        │
└─────────────────────────────────────────────────────────────┘
```

### Key Design Patterns
- **Repository Pattern**  
- **Unit of Work**  
- **DTOs & AutoMapper**  
- **Dependency Injection (DI)**  
- **Paging, Filtering, Sorting** built into repository queries.  

---

## 🛠️ Tech Stack

### 🖥️ Frontend
![HTML5](https://img.shields.io/badge/HTML5-E34F26?logo=html5&logoColor=white)
![CSS3](https://img.shields.io/badge/CSS3-1572B6?logo=css3&logoColor=white)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?logo=javascript&logoColor=black)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?logo=bootstrap&logoColor=white)

### ⚙️ Backend
![.NET](https://img.shields.io/badge/.NET%209-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![EntityFramework](https://img.shields.io/badge/Entity%20Framework-68217A?logo=ef&logoColor=white)
![SQLServer](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![AutoMapper](https://img.shields.io/badge/AutoMapper-ff6600?style=flat-square)
![JWT](https://img.shields.io/badge/JWT-000000?logo=jsonwebtokens&logoColor=white)

### 🧰 DevOps & Tools
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/GitHub%20Actions-2088FF?logo=githubactions&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=black)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91?logo=visualstudio&logoColor=white)

---

## 📂 Project Structure

```bash
Cartify/
├── frontend/                 # HTML/CSS/JS files
├── backend/
│   ├── Cartify.API/          # Presentation layer (Controllers, Middlewares)
│   ├── Cartify.Application/  # Business logic, DTOs, Services
│   ├── Cartify.Domain/       # Entities, Value Objects, Enums
│   ├── Cartify.Infrastructure/# EF Core, Repositories, Persistence
│   └── Cartify.sln
├── docs/                     # ERD, architecture diagrams
├── scripts/                  # Database migrations, deployment scripts
└── README.md
```

---

## 🗄️ Domain Model

**Core Entities:**
- **User** — roles: `Client`, `StoreOwner`, `Admin`
- **Store** — linked to `User`
- **Category** & **SubCategory**
- **Product** — linked to multiple `ProductDetails`
- **ProductDetail** — represents a product variant *(e.g., color, size, storage)*
- **Inventory** — holds stock quantity and price per variant
- **Order** & **OrderItem**
- **Review**, **Rating**, **Coupon**, **RefreshToken**

**Entity Relationships:**
- `Category` → `SubCategory` — **One-to-Many**
- `Product` → `ProductDetail` — **One-to-Many**
- `ProductDetail` → `Inventory` — **One-to-One**
- `User` → `Orders` — **One-to-Many**
- `Order` → `OrderItems` — **One-to-Many**

---

## 🔐 Authentication & Security

- **JWT-based Authentication**
  - Includes both **Access** and **Refresh tokens**
  - Refresh tokens are stored per user using the *owned entity* pattern

- **Role-based Authorization**
  - Supports roles: `Client`, `Merchant`, and `Admin`
  - Access control applied via **policies and attributes**

- **Best Practices Implemented**
  - ✅ HTTPS enforced  
  - ✅ Strong password policy  
  - ✅ Token expiration with refresh rotation  
  - ✅ Centralized exception handling middleware  
  - ✅ Logging and auditing for sensitive actions  

---

## 📡 Example API Endpoints

| **Type** | **Endpoint** | **Description** |
|-----------|--------------|-----------------|
| `POST` | `/api/auth/register` | Register new user |
| `POST` | `/api/auth/login` | Login & retrieve JWT tokens |
| `POST` | `/api/auth/refresh` | Refresh expired access token |
| `GET` | `/api/products` | List all products |
| `GET` | `/api/products/{id}` | Get detailed product info |
| `POST` | `/api/orders` | Place a new order |
| `GET` | `/api/orders` | Retrieve all user orders |
| `GET` | `/api/merchant/dashboard` | Get merchant analytics data |
| `GET` | `/api/admin/dashboard` | Get global platform statistics |

---

## ⚙️ Setup & Run

### 🧩 Backend Setup

```bash
git clone https://github.com/Taqey/Cartify.git
cd Cartify/backend
```

### 1️⃣ Configure Database

Create or edit `appsettings.json` in **Cartify.API**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=CartifyDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "your-secret-key-min-32-characters-long",
    "Issuer": "Cartify",
    "Audience": "Cartify.Clients",
    "AccessTokenMinutes": 15,
    "RefreshTokenDays": 7
  }
}
```

### 2️⃣ Apply Migrations

```bash
dotnet ef database update --project Cartify.Infrastructure --startup-project Cartify.API
```

### 3️⃣ Run the API

```bash
dotnet run --project Cartify.API
```

Navigate to `https://localhost:5001/swagger` to explore the API.

---

### 💻 Frontend Setup

```bash
cd frontend
# Using serve (simple static server)
npm install -g serve
serve .
```

Or open `index.html` directly in your browser.

---

### 🐳 Docker Deployment

**Dockerfile:**

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY backend/ .
RUN dotnet restore Cartify.sln
RUN dotnet publish Cartify.API/Cartify.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Cartify.API.dll"]
```

**Build & Run:**

```bash
docker build -t cartify-api .
docker run -p 8080:8080 cartify-api
```

---

## 📊 Development Status

| Area                    | Status | Details                                  |
| ----------------------- | ------ | ---------------------------------------- |
| **Frontend UI**         | ✅      | Home, Cart, Checkout, Profile pages      |
| **Backend Core**        | ✅      | Clean architecture implemented           |
| **Authentication**      | ✅      | JWT + Refresh tokens + Role-based access |
| **Product Module**      | ⏳     | CRUD + pagination + filtering            |
| **Inventory & Pricing** | ⏳     | Linked to product variants               |
| **Order System**        | ⏳      | Checkout flow and order tracking         |
| **Merchant Dashboard**  | ⏳      | Sales & product analytics                |
| **Admin Panel**         | ⏳    | Global analytics and management tools    |
| **Deployment (CI/CD)**  | ⏳      | In progress                              |

---

## 🧪 Testing

- **Unit Tests:** Application layer with mock repositories.
- **Integration Tests:** API endpoints using in-memory/SQLite database.
- **Load Testing:** Planned via k6 / JMeter.

---

## 🚀 CI/CD

- **GitHub Actions**
- Automated build & test pipelines
- Docker image publishing on push to main
- Future deployment to AWS / Azure Container Apps

---

## 🗺 Roadmap

- [x] UI/UX Design & Frontend
- [x] Core API Architecture
- [x] Authentication & Authorization
- [x] Product & Inventory System
- [x] Orders & Checkout
- [ ] Payment Gateway Integration
- [ ] Notifications (Email, Push)
- [ ] Advanced Analytics Dashboard
- [ ] Docker Compose (API + DB + Frontend)
- [ ] Cloud Deployment (AWS/Azure)

---

## 👥 Team

| Name              | Role                          | GitHub                                     |
| ----------------- | ----------------------------- | ------------------------------------------ |
| **Ahmed Ayad**    | Backend Developer / Architect | [@a7med3yad](https://github.com/a7med3yad) |
| **Taqey Eldeen**  | Full Stack Developer          | [@Taqey](https://github.com/Taqey)         |
| **Mark Osama**    | Frontend Developer            | [@MarkOsama](#)                            |
| **Mohamed Raouf** | Backend Developer             | [@MohamedRaouf](#)                         |
| **Mustafa Nasr**  | DevOps / QA                   | [@MustafaNasr](#)                          |

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](./LICENSE) file for details.

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome!  
Feel free to check the [issues page](https://github.com/Taqey/Cartify/issues).

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📞 Contact

For questions or support, please open an issue or contact the team.

**Happy Coding! 🚀**