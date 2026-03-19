# **Full Stack Task Management MVP (All-in-One\!)**

Welcome to my modern, responsive To-Do app\! It's built with a .NET Core backend and a Vue.js frontend, all bundled up into a single, neat project.

## **🚀 Let's Get Started\!**


### **What You Need**

* [.NET 10 SDK](https://dotnet.microsoft.com/download) (Make sure you have this installed!)

### **How to Run**

1. **Install .NET 10 SDK** if you haven't already: [Download .NET 10 SDK](https://dotnet.microsoft.com/download)
2. **Restore dependencies** (from the root or `src/tasks` folder):
   ```
   dotnet restore
   ```
3. **Run the backend and frontend (served together):**
   ```
   dotnet run --project src/tasks/Tasks.csproj
   ```
   - The app will start on [http://localhost:5000](http://localhost:5000) (or as shown in the console).
   - The Vue frontend is served from the same address.
   - API docs are available at `/swagger` (e.g., http://localhost:5000/swagger).

**No Node.js or separate frontend build is required.** All static files are served by ASP.NET Core.

## **🏗 How It's Built**

### **Everything in One Place**

* **Static Files:** The entire Vue app lives inside wwwroot/index.html. ASP.NET Core just serves this right to you\!  
* **No CORS Headaches:** Since the frontend and backend share the exact same origin and port, we don't have to worry about CORS rules or preflight requests. It makes things faster and so much easier.  
* **No Node.js Needed:** We're loading Vue 3 via a CDN and using the Composition API right in the browser.

### **The Backend (.NET Core Minimal API)**

* **In-Memory Database:** We're using EF Core In-Memory. This makes testing a breeze since you don't need to set up a real database right now\!  
* **Dependency Injection:** The database context gets passed right into the controllers. Easy\!

### **The Frontend (Vue 3 \+ Tailwind CSS)**

* **Clean Setup:** We're using Vue 3's awesome Composition API (setup(), ref, computed) to keep the code super tidy.  
* **Cool MVP Features:** \* **Smooth Error Handling:** If the API acts up, the UI tells you nicely instead of just breaking.  
  * **Loading States:** You'll get a visual heads-up while your data is loading.  
  * **Filtering:** Easily sort through All, Active, or Completed tasks.  
  * **Mobile-Friendly:** It looks great on both phones and desktops thanks to Tailwind\!

## **⚖️ Trade-offs & Things to Keep in Mind**

1. **Browser Compilation:** Because we're using Vue from a CDN, your browser compiles the HTML templates on the fly. It's super fast, but pre-compiling with a build step is definitely better for real-world production.  
2. **In-Memory Data:** Remember, since the database is in-memory, all your tasks will vanish if you restart the backend\!  
3. **No Logins:** Skipped authentication for now since it's just a simple MVP.  
4. **Icons:** Custom SVG icons to avoid the hassle of setting up icon libraries without a bundler.

## **🔮 Where Do We Go From Here?**

If we were going to take this app to the big leagues, here's what I'd add:

1. **A Real Build Pipeline:** Bring in a tool like Vite so we can use Single-File Components (.vue files) and squish everything down for maximum speed.  
2. **A Real Database:** Swap out the in-memory setup for a real database like PostgreSQL or SQL Server.  
3. **User Accounts:** Add ASP.NET Core Identity so users can actually log in and save their own personal task lists.  
4. **Bigger Architecture:** As the app gets more complex, we'd upgrade to a more robust setup, like the Repository Pattern or CQRS with MediatR.