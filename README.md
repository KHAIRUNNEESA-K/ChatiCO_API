# ChatiCO – Real-Time Chat Application

ChatiCO is a real-time chat application built using React for the frontend and ASP.NET Core Web API for the backend.
It uses SignalR for real-time communication and follows Clean Architecture with a Hybrid ORM approach for better performance and maintainability.

Features

Authentication

- OTP-based login using Twilio

- OTP stored temporarily using in-memory cache

- JWT-based authentication and authorization

User Management

- View all registered users

- Add users to contacts for chatting

- Edit user profile (name, bio, profile picture)

- Upload profile images using Cloudinary

- Online and offline status tracking

- Last seen information

- Block and unblock users

Chat System

- One-to-one real-time chat

- Group chat

- Archive chats

- Soft delete messages

- Message read and delivery status

- Typing indicators

- Real-time updates using SignalR

Tech Stack

Frontend
- React.js

- JavaScript

- Tailwind CSS

- EST API integration

- SignalR client

Backend

- ASP.NET Core Web API (.NET 8)

- SignalR (WebSockets)

- JWT authentication

- In-memory cache for OTP

- Hybrid ORM approach:

     - Dapper for read (GET) operations

     - Entity Framework Core for write operations (POST, PUT, PATCH, DELETE)

- SQL Server

Third-Party Services

- Twilio for OTP delivery

- Cloudinary for image upload and media storage

Modules

Authentication Module
- OTP generation and validation

- JWT-based authentication

- Secure login and logout

User Module

- User profile management

- View all users

- Add contacts

- Block and unblock users

Chat Module

- One-to-one and group chat

- Archive chat functionality

- Soft delete messages

- Message read receipts

Media Module

- Image upload using Cloudinary

- Media message handling
