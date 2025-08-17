# 🎫 IT Department Ticketing System  

A **Ticket Management System** built with **Blazor Server** for handling IT support requests inside an organization.  
It provides full lifecycle management of tickets — from creation to resolution — with real-time collaboration, notifications, and performance tracking.  

---

## 🚀 Features  

### 🎟 Ticket Management  
- Create, assign, escalate, close, and schedule tickets.  
- Add **solutions, comments, and attachments** (photo uploads supported).  
- Define **priority levels** (High, Medium, Low).  
- Auto-assign tickets based on schedule & availability.  

### 💬 Real-Time Collaboration  
- **SignalR integration** for:  
  - Instant notifications (new tickets, updates, escalations).  
  - Real-time **chat per ticket** between users and agents.  

### 📊 Agent Performance  
- Dedicated **Agent Performance Dashboard**:  
  - Track assigned tickets, closure time, ratings, and statistics.  
  - Visual insights via charts & tables.  

### 👥 User Management & Roles  
- **3 Roles** with authorization:  
  - **Admin** → Manage users, tickets, system settings.  
  - **Agent** → Handle assigned tickets, respond & resolve.  
  - **User** → Create tickets, approve solutions, rate agents.  
- Admin can fully manage user accounts.  

### 🔐 Security  
- Role-based **authorization** & access control.  
- **Data hashing & secure authentication**.  

### 📱 Responsive Design  
- Fully responsive UI for **desktop, tablet, and mobile**.  

---

## 🛠 Tech Stack  

- **Frontend & Backend:** Blazor Server (.NET 8)  
- **Database:** SQL Server  
- **Real-Time:** SignalR  
- **Authentication & Authorization:** ASP.NET Identity  
- **Styling:** Bootstrap (responsive design)  

---

## ⚙️ Installation & Setup  

1. **Clone the repository**  
   ```bash
   git clone https://github.com/Abdelrahman2264/it-ticketing-system.git
   cd it-ticketing-system
   ```

2. **Configure Database**  
   - Update connection string in `appsettings.json`.  
   - Run migrations:  
     ```bash
     dotnet ef database update
     ```

3. **Run the project**  
   ```bash
   dotnet run
   ```

4. Open browser at `https://localhost:5001`  

---

## 📸 Screenshots (Optional)  
*(Add some screenshots/gifs here for UI, ticket dashboard, chat, and performance page)*  

---

## 🔮 Future Enhancements  
- Email & SMS notifications.  
- Advanced analytics for admin reports.  
- Integration with external systems (e.g., Slack, Teams).  
- AI-powered ticket classification.  

---

## 👨‍💻 Roles Overview  

| Role   | Capabilities |
|--------|--------------|
| **Admin** | Manage users, assign roles, oversee all tickets |
| **Agent** | Handle tickets, update solutions, chat with users |
| **User**  | Create tickets, chat with agents, rate & approve solutions |

---

## 📄 License  

This project is licensed under the **MIT License** – free to use and modify.  
