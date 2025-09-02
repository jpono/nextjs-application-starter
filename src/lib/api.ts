const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL || 'http://localhost:5156/api'

class ApiClient {
  private baseURL: string

  constructor(baseURL: string) {
    this.baseURL = baseURL
  }

  private getAuthHeaders(): HeadersInit {
    const token = localStorage.getItem('token')
    return {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    }
  }

  async get<T>(endpoint: string): Promise<T> {
    const response = await fetch(`${this.baseURL}${endpoint}`, {
      method: 'GET',
      headers: this.getAuthHeaders(),
    })

    if (!response.ok) {
      throw new Error(`API Error: ${response.status} ${response.statusText}`)
    }

    return response.json()
  }

  async post<T>(endpoint: string, data: any): Promise<T> {
    const response = await fetch(`${this.baseURL}${endpoint}`, {
      method: 'POST',
      headers: this.getAuthHeaders(),
      body: JSON.stringify(data),
    })

    if (!response.ok) {
      throw new Error(`API Error: ${response.status} ${response.statusText}`)
    }

    return response.json()
  }

  async put<T>(endpoint: string, data: any): Promise<T> {
    const response = await fetch(`${this.baseURL}${endpoint}`, {
      method: 'PUT',
      headers: this.getAuthHeaders(),
      body: JSON.stringify(data),
    })

    if (!response.ok) {
      throw new Error(`API Error: ${response.status} ${response.statusText}`)
    }

    return response.json()
  }

  async delete(endpoint: string): Promise<void> {
    const response = await fetch(`${this.baseURL}${endpoint}`, {
      method: 'DELETE',
      headers: this.getAuthHeaders(),
    })

    if (!response.ok) {
      throw new Error(`API Error: ${response.status} ${response.statusText}`)
    }
  }
}

export const apiClient = new ApiClient(API_BASE_URL)

// Auth endpoints
export const authApi = {
  login: async (email: string, password: string) => {
    const response = await fetch(`${API_BASE_URL}/../auth/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email, password }),
    })

    if (!response.ok) {
      throw new Error('Invalid credentials')
    }

    return response.json()
  },
}

// Project endpoints
export const projectApi = {
  getAll: () => apiClient.get('/Project'),
  getById: (id: number) => apiClient.get(`/Project/${id}`),
  create: (data: any) => apiClient.post('/Project', data),
  update: (id: number, data: any) => apiClient.put(`/Project/${id}`, data),
  delete: (id: number) => apiClient.delete(`/Project/${id}`),
}

// Employee endpoints
export const employeeApi = {
  getAll: () => apiClient.get('/Employee'),
  getById: (id: number) => apiClient.get(`/Employee/${id}`),
  create: (data: any) => apiClient.post('/Employee', data),
  update: (id: number, data: any) => apiClient.put(`/Employee/${id}`, data),
  delete: (id: number) => apiClient.delete(`/Employee/${id}`),
}

// Equipment endpoints
export const equipmentApi = {
  getAll: () => apiClient.get('/Equipment'),
  getById: (id: number) => apiClient.get(`/Equipment/${id}`),
  create: (data: any) => apiClient.post('/Equipment', data),
  update: (id: number, data: any) => apiClient.put(`/Equipment/${id}`, data),
  delete: (id: number) => apiClient.delete(`/Equipment/${id}`),
}

// Client endpoints
export const clientApi = {
  getAll: () => apiClient.get('/Client'),
  getById: (id: number) => apiClient.get(`/Client/${id}`),
  create: (data: any) => apiClient.post('/Client', data),
  update: (id: number, data: any) => apiClient.put(`/Client/${id}`, data),
  delete: (id: number) => apiClient.delete(`/Client/${id}`),
}

// Invoice endpoints
export const invoiceApi = {
  getAll: () => apiClient.get('/Invoice'),
  getById: (id: number) => apiClient.get(`/Invoice/${id}`),
  create: (data: any) => apiClient.post('/Invoice', data),
  update: (id: number, data: any) => apiClient.put(`/Invoice/${id}`, data),
  delete: (id: number) => apiClient.delete(`/Invoice/${id}`),
}

// Schedule endpoints
export const scheduleApi = {
  getAll: () => apiClient.get('/Schedule'),
  getById: (id: number) => apiClient.get(`/Schedule/${id}`),
  create: (data: any) => apiClient.post('/Schedule', data),
  update: (id: number, data: any) => apiClient.put(`/Schedule/${id}`, data),
  delete: (id: number) => apiClient.delete(`/Schedule/${id}`),
}

// Document endpoints
export const documentApi = {
  getAll: () => apiClient.get('/Document'),
  getById: (id: number) => apiClient.get(`/Document/${id}`),
  create: (data: any) => apiClient.post('/Document', data),
  update: (id: number, data: any) => apiClient.put(`/Document/${id}`, data),
  delete: (id: number) => apiClient.delete(`/Document/${id}`),
}

// Report endpoints
export const reportApi = {
  getAll: () => apiClient.get('/Report'),
  getById: (id: number) => apiClient.get(`/Report/${id}`),
  create: (data: any) => apiClient.post('/Report', data),
  update: (id: number, data: any) => apiClient.put(`/Report/${id}`, data),
  delete: (id: number) => apiClient.delete(`/Report/${id}`),
}

// User endpoints
export const userApi = {
  getAll: () => apiClient.get('/User'),
  getById: (id: string) => apiClient.get(`/User/${id}`),
  create: (data: any) => apiClient.post('/User', data),
  update: (id: string, data: any) => apiClient.put(`/User/${id}`, data),
  delete: (id: string) => apiClient.delete(`/User/${id}`),
}
