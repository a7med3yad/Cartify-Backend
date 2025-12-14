# 🛒 Cartify — Full-Stack Multi-Vendor E-Commerce Platform

[![License](https://img.shields.io/github/license/Taqey/Cartify?style=flat-square)](./LICENSE)
[![Stars](https://img.shields.io/github/stars/Taqey/Cartify?style=flat-square)](#)
[![Forks](https://img.shields.io/github/forks/Taqey/Cartify?style=flat-square)](#)
[![Backend](https://img.shields.io/badge/Backend-.NET%209-512BD4?logo=dotnet&style=flat-square)](#)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-CC2927?logo=microsoftsqlserver&style=flat-square)](#)
[![Cloud](https://img.shields.io/badge/Cloud-AWS-FF9900?logo=amazonaws&style=flat-square)](#)

---

## 🧩 Overview

**Cartify** is a **production-ready multi-vendor e-commerce platform** built with **ASP.NET Core (.NET 9)** following **Clean Architecture** principles.

This project demonstrates enterprise-level system design with:
- Scalable backend architecture
- Secure AWS cloud deployment
- Professional database design
- Complete frontend integration
- Real-world security practices

**This is not a tutorial project** — it's built with the mindset of a professional software engineer, simulating real production systems.

---

## 🎯 Key Highlights

✅ **Clean Architecture** with strict layer separation  
✅ **JWT Authentication** with refresh tokens & role-based authorization  
✅ **Multi-vendor marketplace** (Customers / Merchants / Admins)  
✅ **AWS Cloud Infrastructure** (EC2, RDS, S3, VPC)  
✅ **Complete API suite** with RESTful design  
✅ **Frontend integration** with dynamic rendering  
✅ **Production-grade security** (Bastion host, private subnets, Security Group and Network ACLS)

---

## ✨ Features

### 👤 Customer Features
- Browse products with advanced filtering
- View product variants (storage, size, attributes)
- Shopping cart & checkout flow
- Order history & real-time tracking
- Profile & address management
- Wishlist functionality

### 🏬 Merchant Features
- Store creation & management
- Product catalog with variants
- Inventory control & pricing
- Order management & fulfillment
- Sales analytics dashboard

### 🧑‍💻 Admin Features
- Platform-wide oversight
- User & merchant moderation
- Category & product approval
- System reports & analytics
- Support ticket management

---

## 🧱 System Architecture

Cartify follows **Clean Architecture** with clear separation of concerns:

```
┌─────────────────────────────────────────────┐
│           API Layer (Controllers)           │
│         ↓ HTTP Requests & Responses         │
└─────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────┐
│    Application Layer (Business Logic)       │
│   Services, DTOs, Use Cases, Interfaces     │
└─────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────┐
│      Domain Layer (Core Business Rules)     │
│        Entities, Value Objects, Rules       │
└─────────────────────────────────────────────┘
                      ↑
┌─────────────────────────────────────────────┐
│   Infrastructure Layer (Data & External)    │
│   EF Core, Repositories, AWS S3, Database   │
└─────────────────────────────────────────────┘
```

### Design Patterns & Principles
- **Repository Pattern** for data access abstraction
- **Unit of Work** for transaction management
- **Dependency Injection** throughout all layers
- **DTOs & AutoMapper** for clean data transfer
- **SOLID Principles** enforced across codebase

---

## ☁️ AWS Cloud Architecture

### Infrastructure Components

| Service | Purpose | Configuration |
|---------|---------|---------------|
| **EC2** | API Hosting | Public subnet, acts as Bastion/Entry Point |
| **RDS** | SQL Server Database | Private subnet, no direct internet access |
| **S3** | Image Storage | Pre-signed URLs for secure uploads |
| **VPC** | Network Isolation | Custom VPC with public/private subnets |
| **Subnets** | Network Segmentation | Public (EC2) & Private (RDS) subnet separation |
| **NACLs** | Network Layer Security | Stateless firewall rules at subnet level |
| **Security Groups** | Instance Firewalls | Stateful firewall rules, least-privilege access |

### Security Architecture
- ✅ **Multi-layer subnet design** — Public subnet for web tier, private for database
- ✅ **Network ACLs (NACLs)** — Additional subnet-level security layer
- ✅ **Private subnet isolation** — Database has no internet access
- ✅ **Bastion host access** — SSH only through controlled entry point
- ✅ **Security group hardening** — Minimal port exposure with stateful rules
- ✅ **Internet Gateway** — Controlled public internet access for EC2

This setup simulates **real enterprise cloud architecture**, not basic student deployments.

---

## 🏗️ Architecture Diagrams

### ☁️ AWS Infrastructure
Complete AWS setup showing VPC design, subnet configuration, and service connectivity.

![AWS Architecture](./Project%20Architecture/Architecture.png)

### 🗄️ Database Schema (ERD)
Full entity-relationship diagram with all tables, relationships, and constraints.

![Database ERD](./Project%20Architecture/ERD.jpg)

---

## 🗄️ Database Design

### Core Entities & Relationships

**User Management**
- `User` → Customers, Merchants, Admins
- `RefreshToken` → JWT token rotation
- `Address` → User delivery addresses

**Product Catalog**
- `Category` → `SubCategory` (1:N)
- `Product` → `ProductDetail` (1:N variants)
- `ProductDetail` → `Inventory` (1:1)
- `Product` → `Attributes` & `MeasureUnits`

**Store & Orders**
- `Store` → `Product` (1:N)
- `User` → `Order` (1:N)
- `Order` → `OrderItem` (1:N)
- `Product` → `Review` (1:N)

**Features**
- `Cart` & `Wishlist` per user
- `Coupon` system
- `HelpTicket` for support

### Database Best Practices
- Proper indexing on foreign keys
- Cascade delete rules configured
- Validation at entity level
- Optimized query performance

---

## 🎨 Frontend Integration

### Technology Stack
- **HTML5** for semantic structure
- **CSS3** for responsive design
- **Vanilla JavaScript** for full control

### Key Features
- RESTful API communication
- JWT token management
- Dynamic content rendering
- Secure S3 image uploads via pre-signed URLs
- Real-time order tracking
- Responsive design (mobile-first)

### API Integration Pattern
```javascript
// Centralized API client with authentication
const API = {
    baseURL: 'https://api.cartify.com',
    
    async request(method, endpoint, data) {
        const token = localStorage.getItem('jwt_token');
        const response = await fetch(`${this.baseURL}${endpoint}`, {
            method,
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: data ? JSON.stringify(data) : null
        });
        return response.json();
    }
};
```

---

## 🔐 Authentication & Security

### JWT Implementation
- **Access Tokens** (short-lived, 15 minutes)
- **Refresh Tokens** (long-lived, stored in database)
- **Token rotation** on refresh
- **Role-based claims** (Customer, Merchant, Admin)

### Security Features
- Password hashing with industry standards
- SQL injection prevention via parameterized queries
- XSS protection with input sanitization
- CORS configuration for frontend
- HTTPS enforcement in production
- Rate limiting on sensitive endpoints

---

## 🚀 API Documentation

### Core Endpoints

#### Authentication
```
POST   /api/auth/register          - User registration
POST   /api/auth/login             - User login
POST   /api/auth/refresh           - Refresh access token
POST   /api/auth/forgot-password   - Password reset request
```

#### Products
```
GET    /api/products               - List all products (paginated)
GET    /api/products/{id}          - Get product details
POST   /api/products               - Create product (Merchant)
PUT    /api/products/{id}          - Update product (Merchant)
DELETE /api/products/{id}          - Delete product (Merchant)
GET    /api/products/{id}/details  - Get product variants
```

#### Orders
```
GET    /api/orders                 - User's order history
GET    /api/orders/{id}            - Order details
POST   /api/orders                 - Place new order
GET    /api/orders/{id}/track      - Track order status
```

#### Merchant Dashboard
```
GET    /api/merchant/dashboard     - Sales analytics
GET    /api/merchant/orders        - Merchant orders
PUT    /api/merchant/orders/{id}   - Update order status
```

#### Admin
```
GET    /api/admin/dashboard        - Platform overview
GET    /api/admin/users            - User management
PUT    /api/admin/products/{id}    - Moderate products
GET    /api/admin/reports          - System reports
```

### Response Standards
- **Consistent JSON structure**
- **Proper HTTP status codes** (200, 201, 400, 401, 404, 500)
- **Error messages** in standardized format
- **Pagination** for list endpoints

---

## 👥 Team Contributions

### 👑 Ahmed Ibrahim — Team Lead / Backend & Cloud Architect
- Designed complete system architecture and database schema
- Implemented all core backend modules (Products, Orders, Inventory, Customers)
- Enforced Clean Architecture across all layers
- Integrated AWS S3 with pre-signed URLs for secure image handling
- Led AWS deployment (EC2, RDS, S3, VPC, Transit Gateway)
- Conducted code reviews, debugging, and team integration

### ⚙️ Taqey Eldeen — Backend Developer
- Implemented JWT + Refresh Token authentication system
- Built role-based authorization (Customer, Merchant, Admin)
- Created authentication endpoints (Login, Register, Password Reset)
- Contributed to Clean Architecture structure and Repository Pattern

### 🖥️ Amr Khaled — Frontend Developer
- Built product catalog pages (Categories, Products, Product Details)
- Implemented dynamic data relations and component integration
- Ensured smooth frontend-backend API communication

### 🎨 Mark Osama Atia — Frontend Developer
- Implemented complete checkout flow and order confirmation
- Built customer profile and account management pages
- Developed order tracking with real-time status updates
- Ensured responsive design across all devices

### 🛠️ Mohamed Raouf — Backend Developer
- Implemented admin controller and business logic
- Created admin DTOs and validation rules
- Ensured API consistency across administrative features

### 🧪 Mostafa Nasr — Backend / QA
- Implemented cart and wishlist features
- Built order tracking, cart, and wishlist pages
- Assisted in testing, validation, and system stability

---

## 🗺️ Project Roadmap

### ✅ Phase 1 — Foundation (Completed)
- Clean Architecture implementation
- Database design & ERD
- JWT authentication & authorization
- Core API endpoints (Products, Orders, Categories)
- Basic frontend UI
- AWS infrastructure setup (EC2, RDS, S3, VPC)

### ✅ Phase 2 — Core Features (Completed)
- Product variants & attributes system
- Inventory management
- Shopping cart & checkout flow
- Merchant dashboard
- Secure S3 image uploads
- Order tracking system

### 🔄 Phase 3 — Advanced Features (In Progress)
- Payment gateway integration (Stripe/PayPal)
- Email notifications & order updates
- Advanced merchant analytics
- Admin reporting & data exports
- Search optimization

### 🚀 Phase 4 — Production (Planned)
- CI/CD pipeline automation
- Docker containerization
- Load balancing & auto-scaling
- Monitoring & logging (CloudWatch)
- Custom domain & SSL certificates
- Performance optimization

---

## 🛠️ Tech Stack

### Backend
- **Framework:** ASP.NET Core 9
- **Language:** C# 12
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Authentication:** JWT with Refresh Tokens
- **Cloud Storage:** AWS S3

### Frontend
- **Markup:** HTML5
- **Styling:** CSS3
- **Logic:** Vanilla JavaScript (ES6+)
- **API Communication:** Fetch API

### Cloud & Infrastructure
- **Hosting:** AWS EC2
- **Database:** AWS RDS (SQL Server)
- **Storage:** AWS S3
- **Networking:** AWS VPC
- **Security:** IAM, Security Groups

### Tools & Practices
- **Version Control:** Git & GitHub
- **API Testing:** Postman
- **Architecture:** Clean Architecture
- **Design Patterns:** Repository, Unit of Work, Dependency Injection

---

## 📦 Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server or AWS RDS instance
- AWS account (for S3 and deployment)
- Node.js (optional, for frontend tooling)

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/nhahub/NHA-262.git
cd Cartify
```

2. **Configure database**
```bash
cd backend
dotnet ef database update
```

3. **Set up environment variables**
```bash
# appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string"
  },
  "JWT": {
    "SecretKey": "your-secret-key",
    "Issuer": "Cartify",
    "Audience": "CartifyUsers"
  },
  "AWS": {
    "BucketName": "your-s3-bucket",
    "Region": "us-east-1"
  }
}
```

4. **Run the application**
```bash
dotnet run
```

5. **Open frontend**
```bash
cd ../frontend
# Open index.html in browser or use live server
```

---

## 📊 Project Statistics

- **Total Lines of Code:** 20,000+
- **API Endpoints:** 40+
- **Database Tables:** 20+
- **Cloud Services:** 6 AWS services
- **Team Members:** 6 developers
- **Development Time:** 3+ months

---

## 📝 Code Quality Standards

- Clean Architecture enforced across all layers
- SOLID principles followed
- Comprehensive input validation
- Proper error handling & logging
- Consistent naming conventions
- Code reviews before merging
- Git branching strategy (feature branches)

---

## 🎓 Learning Outcomes

This project demonstrates:
- ✅ Real-world system design thinking
- ✅ Secure authentication & authorization
- ✅ Cloud infrastructure knowledge (AWS)
- ✅ Database design & optimization
- ✅ RESTful API best practices
- ✅ Team collaboration & Git workflows
- ✅ Production-ready security practices

---

## 📞 Contact & Links

- **GitHub Repository:** [github.com/nhahub/Cartify](https://github.com/nhahub/NHA-262)
- **Live Demo:** [Demo](cartify.runasp.net/swagger/index.html)

---

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🎯 Final Note

Cartify is **not a tutorial clone** — it's a **production-style system** built to demonstrate:

- Professional system design
- Enterprise-level architecture
- Cloud infrastructure expertise
- Secure coding practices
- Real team collaboration

**Built with engineering discipline, not just code tutorials** 🚀

---

<p align="center">Made with 💙 by the Cartify Team</p>