# Construction SaaS Platform

A comprehensive SaaS platform for construction management, built with .NET Core backend and Next.js frontend. Features include client management, project tracking, employee management, equipment tracking, invoicing, scheduling, document management, and reporting.

## Project Structure

- **Backend**: .NET Core API (`ConstructionSaaSBackend/`) - RESTful API with JWT authentication, multi-tenancy support, and SQLite database.
- **Frontend**: Next.js React app (`src/`) - Modern UI with Tailwind CSS and shadcn/ui components.

## Prerequisites

- .NET 8.0 SDK
- Node.js 18+ and npm
- SQLite (included with .NET)

## Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd ConstructionSaaSBackend
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

4. Run the backend server:
   ```bash
   dotnet run
   ```

The backend will start on `https://localhost:5156` (or `http://localhost:5156` in development). Swagger UI is available at `https://localhost:5156/swagger`.

## Frontend Setup

1. Install dependencies:
   ```bash
   npm install
   ```

2. Run the development server:
   ```bash
   npm run dev
   ```

Open [http://localhost:3000](http://localhost:3000) with your browser to access the application.

## Testing Backend APIs with curl

The backend requires JWT authentication for most endpoints. First, obtain a token by logging in:

### 1. Login to get JWT token
```bash
curl -X POST "http://localhost:5156/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@example.com",
    "password": "Admin123!"
  }'
```

Copy the `token` from the response for use in subsequent requests.

### 2. Test User Management APIs
```bash
# Get all users
curl -X GET "http://localhost:5156/api/User" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"

# Create a new user
curl -X POST "http://localhost:5156/api/User" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "email": "newuser@example.com",
    "password": "TempPass123!",
    "firstName": "John",
    "lastName": "Doe",
    "isActive": true
  }'

# Get user by ID
curl -X GET "http://localhost:5156/api/User/USER_ID_HERE" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"

# Update user
curl -X PUT "http://localhost:5156/api/User/USER_ID_HERE" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "email": "updateduser@example.com",
    "firstName": "Jane",
    "lastName": "Smith",
    "isActive": true
  }'

# Delete user
curl -X DELETE "http://localhost:5156/api/User/USER_ID_HERE" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 3. Test Client APIs
```bash
# Get all clients
curl -X GET "http://localhost:5156/api/Client" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"

# Create a new client
curl -X POST "http://localhost:5156/api/Client" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "name": "ABC Construction",
    "contactPerson": "John Doe",
    "email": "john@abc.com",
    "phoneNumber": "123-456-7890",
    "address": "123 Main St",
    "city": "Anytown",
    "state": "CA",
    "zipCode": "12345",
    "country": "USA",
    "type": "Contractor",
    "notes": "New client",
    "isActive": true
  }'

# Get client by ID
curl -X GET "http://localhost:5156/api/Client/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"

# Update client
curl -X PUT "http://localhost:5156/api/Client/1" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "name": "Updated Construction",
    "contactPerson": "Jane Doe",
    "email": "jane@updated.com",
    "phoneNumber": "987-654-3210",
    "address": "456 Oak St",
    "city": "Newtown",
    "state": "NY",
    "zipCode": "67890",
    "country": "USA",
    "type": "Developer",
    "notes": "Updated client info",
    "isActive": true
  }'

# Delete client
curl -X DELETE "http://localhost:5156/api/Client/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 4. Test Project APIs
```bash
# Get all projects
curl -X GET "http://localhost:5156/api/Project" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"

# Create a new project
curl -X POST "http://localhost:5156/api/Project" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "name": "Office Building",
    "description": "New office construction",
    "clientId": 1,
    "address": "789 Business Ave",
    "startDate": "2024-01-01",
    "endDate": "2024-12-31",
    "budget": 1000000.00,
    "status": "In Progress"
  }'
```

### 4. Test Other APIs
Similar patterns apply for Employee, Equipment, Invoice, Schedule, Document, and Report endpoints:

- **Employee**: `/api/Employee`
- **Equipment**: `/api/Equipment`
- **Invoice**: `/api/Invoice`
- **Schedule**: `/api/Schedule`
- **Document**: `/api/Document`
- **Report**: `/api/Report`

## Testing Frontend Functionality

1. **Login Flow**:
   - Navigate to the login page
   - Enter valid credentials (use the same email/password as in curl login)
   - Verify redirect to dashboard

2. **Dashboard Navigation**:
   - Check all navigation links (Users, Projects, Employees, Equipment, Clients, Invoices, Schedule, Documents, Reports)
   - Verify each page loads correctly

3. **CRUD Operations**:
   - **Create**: Add new users, clients, projects, employees, etc.
   - **Read**: View lists and individual items
   - **Update**: Edit existing records
   - **Delete**: Remove records (with confirmation)

4. **Form Validation**:
   - Test required fields
   - Test data type validation (email, dates, numbers)
   - Test error handling for invalid inputs

5. **Responsive Design**:
   - Test on different screen sizes
   - Verify mobile navigation and layouts

## Environment Variables

Create a `.env.local` file in the root directory for frontend:

```
NEXT_PUBLIC_API_BASE_URL=http://localhost:5156/api
```

For backend, configure `appsettings.json` or `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ConstructionSaaSDb.db"
  },
  "Jwt": {
    "SecretKey": "your-secret-key-here",
    "Issuer": "ConstructionSaaS",
    "Audience": "ConstructionSaaS"
  }
}
```

## Learn More

- [Next.js Documentation](https://nextjs.org/docs)
- [.NET Core Documentation](https://docs.microsoft.com/en-us/dotnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [JWT Authentication in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/jwt)
