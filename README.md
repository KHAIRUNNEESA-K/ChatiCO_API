💬 ChatiCO – Real-Time Chat Application

ChatiCO is a real-time chat application built with React (Frontend) and ASP.NET Core Web API (Backend) using SignalR for real-time communication.
The project follows Clean Architecture and uses a Hybrid ORM approach for optimized data access.

🚀 Features
🔐 Authentication

OTP-based login using Twilio

OTP stored temporarily using In-Memory Cache

JWT-based authentication & authorization

👤 User Management

View all registered users

Add users to contacts for chatting

Edit profile (Name, Bio, Profile Picture)

Profile image upload using Cloudinary

Online / Offline status & last seen

Block & unblock users

💬 Chat System

One-to-one real-time chat

Group chat

Archive chats

Delete messages (soft delete)

Read & delivery status

Typing indicators

Real-time updates using SignalR

🛠️ Tech Stack
Frontend

React.js

JavaScript

Tailwind CSS

REST API integration

SignalR client

Backend

ASP.NET Core Web API (.NET 8)

SignalR (WebSockets)

JWT Authentication

In-Memory Cache (OTP)

Hybrid ORM:

Dapper – Read (GET) operations

Entity Framework Core – Write (POST, PUT, PATCH, DELETE)

SQL Server

Third-Party Services

Twilio – OTP delivery

Cloudinary – Image upload & media storage

🧱 Architecture

Architecture Style: Clean Architecture / Three-Tier

Frontend (React)
     ↓
API Controllers
     ↓
Application Layer (DTOs, Services)
     ↓
Domain Layer (Entities)
     ↓
Infrastructure Layer (EF Core, Dapper, SignalR)
     ↓
SQL Server


Dapper improves read performance

EF Core ensures transactional integrity for writes

SignalR enables real-time messaging

🔄 Data Flow

User requests OTP → Stored in In-Memory Cache → Sent via Twilio

OTP verification → JWT issued

User connects to chat → SignalR Hub established

Messages stored in SQL Server

Media uploaded to Cloudinary → URL saved in DB

📦 Modules
1️⃣ Authentication Module

OTP generation & validation

JWT authentication

Secure login & logout

2️⃣ User Module

Profile management

View all users

Add contacts

Block / unblock users

3️⃣ Chat Module

One-to-one & group chat

Archive chat

Soft delete messages

Read receipts

4️⃣ Media Module

Image upload via Cloudinary

Media message handling
