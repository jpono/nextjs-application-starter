```markdown
# Detailed Implementation Plan

This plan covers two main parts: the .NET backend API and the Next.js React frontend. It assumes you will create a new .NET Web API project for the backend (named “ConstructionSaaSBackend”) and use your existing Next.js project for the frontend. The plan incorporates multi-tenancy using row-level security with hooks to later scale to schema per tenant, uses ASP.NET Core Identity for authentication, and provides comprehensive construction business features.

---

## 1. Backend (.NET API) – ConstructionSaaSBackend

### A. Project Initialization & Configuration
- **Create Project:**  
  • Run `dotnet new webapi -n ConstructionSaaSBackend` to create a new API project.  
  • Add necessary NuGet packages for Entity Framework Core, SQL Server, and ASP.NET Core Identity.
- **appsettings.json:**  
  • Configure the SQL Server connection string.  
  • Add optional configuration flag for multi-tenancy mode (e.g., `"MultiTenancyMode": "RowLevel"`).

### B. Models & Data Layer
- **Models Folder:**  
  • Create model files for Tenant (e.g., Tenant.cs with TenantId, Name), ApplicationUser (extend IdentityUser to include TenantId), and business entities: Project.cs, Employee.cs, Equipment.cs, Client.cs, Invoice.cs, Schedule.cs, Document.cs, Report.cs.  
- **Data Folder & ApplicationDbContext.cs:**  
  • Inherit from `IdentityDbContext<ApplicationUser>` and add DbSet properties for each business entity.  
  • In the `OnModelCreating` method, set up global query filters on every entity that includes a TenantId field for row-level security.
- **Migration:**  
  • Run EF Core migrations to initialize the database schema on SQL Server.

### C. Authentication & Multi-Tenancy Middleware
- **ASP.NET Core Identity:**  
  • In Program.cs (or Startup.cs), add services: `services.AddIdentity<ApplicationUser, IdentityRole>()…` and configure Identity with default token providers.
- **Tenant Resolution Middleware:**  
  • Create a middleware (TenantMiddleware.cs) that extracts the TenantID from an HTTP header, query string or user claims and stores it (using, for example, HttpContext.Items).
  • Update ApplicationDbContext to retrieve the TenantID from the current request and enforce it in queries.
- **Error Handling:**  
  • Implement a global exception handling middleware to catch errors and return standardized error responses, logging details as needed.

### D. Controllers & Services
- **Controllers Folder:**  
  • Create controllers (e.g., ProjectsController.cs, EmployeesController.cs, EquipmentController.cs, etc.) for CRUD operations on each entity.  
  • Ensure that each action validates the TenantID (by reading from the middleware or the token) and applies business rules.
- **Services Folder:**  
  • Create helper services (e.g., TenantService.cs, DocumentService.cs) to encapsulate multi-tenancy and document management logic.
- **Future Multi-Tenancy Scaling:**  
  • In your DbContext and TenantProvider logic, prepare abstraction points so you can later switch from row-level security to schema per tenant by altering connection strings or setting schema names dynamically.

---

## 2. Frontend (Next.js / React)

### A. Authentication & API Integration
- **Login & Authentication Pages:**  
  • Create a `pages/login.tsx` page for user login that calls the .NET backend authentication endpoints.  
  • Store the JWT or session token securely.
- **API Helper Module:**  
  • In `src/lib/api.ts`, implement functions to call backend endpoints (e.g., GET/POST functions for projects, employees, etc.) with proper error handling using try/catch and status code checks.

### B. Dashboard & Feature Pages
- **Dashboard Layout:**  
  • Create a modern, responsive layout with a header and a vertical sidebar (e.g., in `src/app/dashboard/layout.tsx`).  
  • The sidebar lists navigation links: Projects, Employees, Equipment, Clients, Invoicing, Scheduling, Documents, and Reports.
- **Feature Pages:**  
  • Create separate pages or components under `src/app/dashboard/`:
    - `projects.tsx` – Manage construction projects (list, add, edit, delete).  
    - `employees.tsx` – Employee/worker management UI.  
    - `equipment.tsx` – Equipment tracking management.
    - `clients.tsx` – Client/customer management.  
    - `invoices.tsx` – Invoicing and billing interface.  
    - `calendar.tsx` – Scheduling calendar interface with date/time pickers.  
    - `documents.tsx` – Document management for uploads/downloads.  
    - `reports.tsx` – Reporting/dashboard charts (using built-in HTML5 canvas or charting via plain JS libraries) with a clean layout.
- **UI Design Considerations:**  
  • Use Tailwind CSS and your existing shadcn/ui components to create forms and tables with clear typography, ample spacing, and a consistent color palette.  
  • Avoid external icon libraries; use text and CSS-based indicators for actions.  
  • Validate user inputs and show error messages inline.  
  • Use standard `<img>` tags with placeholder URLs (if required) using the format:  
    `<img src="https://placehold.co/1920x1080?text=Modern+minimalist+dashboard+interface" alt="Modern minimalist dashboard interface with clear typography and structured layout" onerror="this.style.display='none'" />`

### C. Routing & Environment Setup
- **Next.js Config (next.config.ts):**  
  • Add environment variable support for API endpoint URLs to target your .NET backend.  
- **Error Boundaries:**  
  • Wrap key UI components with error boundary components to catch and display fallback UI for runtime errors.

---

## 3. Testing & Documentation

### A. API Testing
- **Curl Commands:**  
  • Test each backend endpoint with curl. For example, for projects:  
    ```bash
    curl -X GET "http://localhost:5000/api/projects" -H "Authorization: Bearer YOUR_TOKEN"
    ```
  • Ensure error codes and response times meet expectations.
  
### B. Frontend Testing
- **Browser Testing:**  
  • Verify navigation flows, form validations, and API error messaging in the dashboard.
  
### C. Documentation
- **Update README.md:**  
  • Include setup instructions for running both the .NET backend and Next.js frontend, environment variables, and deployment notes.

---

# Summary

• A new .NET Web API project (“ConstructionSaaSBackend”) will be created using SQL Server and ASP.NET Core Identity for authentication.  
• Global query filters implementing row-level security will enforce multi-tenancy, with hooks to scale to schema per tenant later on.  
• Dedicated controllers and services will manage construction features including project, employee, equipment, client, invoicing, scheduling, document management, and reporting.  
• A tenant resolution middleware will extract TenantID from requests, ensuring data isolation and error handling via global exception middleware.  
• The Next.js frontend will feature a modern, responsive dashboard with separate pages for each business feature, secure authentication, and integrated API calls.  
• Clean UI design is ensured by using Tailwind CSS and standard HTML elements without external icon libraries.  
• Comprehensive testing procedures using curl for the backend and browser testing for the frontend are included, along with updated documentation in README.md.
