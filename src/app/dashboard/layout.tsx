'use client'

import { useState } from 'react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'
import { useAuth } from '../../hooks/useAuth'
import { Button } from '../../components/ui/button'
import {
  LayoutDashboard,
  Users,
  Wrench,
  Building,
  FileText,
  Calendar,
  FolderOpen,
  BarChart3,
  LogOut
} from 'lucide-react'

const navigation = [
  { name: 'Dashboard', href: '/dashboard', icon: LayoutDashboard },
  { name: 'Projects', href: '/dashboard/projects', icon: Building },
  { name: 'Employees', href: '/dashboard/employees', icon: Users },
  { name: 'Equipment', href: '/dashboard/equipment', icon: Wrench },
  { name: 'Clients', href: '/dashboard/clients', icon: Users },
  { name: 'Invoices', href: '/dashboard/invoices', icon: FileText },
  { name: 'Schedule', href: '/dashboard/schedule', icon: Calendar },
  { name: 'Documents', href: '/dashboard/documents', icon: FolderOpen },
  { name: 'Reports', href: '/dashboard/reports', icon: BarChart3 },
]

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode
}) {
  const [sidebarOpen, setSidebarOpen] = useState(false)
  const pathname = usePathname()
  const { logout } = useAuth()

  const handleLogout = () => {
    logout()
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Sidebar */}
      <div className="fixed inset-y-0 left-0 z-50 w-64 bg-white shadow-lg transform transition-transform duration-300 ease-in-out lg:translate-x-0 lg:static lg:inset-0">
        <div className="flex items-center justify-center h-16 px-4 bg-blue-600">
          <h1 className="text-white text-xl font-bold">Construction SaaS</h1>
        </div>
        <nav className="mt-8">
          <div className="px-4 space-y-2">
            {navigation.map((item) => {
              const isActive = pathname === item.href
              return (
                <Link
                  key={item.name}
                  href={item.href}
                  className={`flex items-center px-4 py-2 text-sm font-medium rounded-md transition-colors ${
                    isActive
                      ? 'bg-blue-100 text-blue-700'
                      : 'text-gray-600 hover:bg-gray-100 hover:text-gray-900'
                  }`}
                >
                  <item.icon className="mr-3 h-5 w-5" />
                  {item.name}
                </Link>
              )
            })}
          </div>
        </nav>
        <div className="absolute bottom-0 w-full p-4">
          <Button
            onClick={handleLogout}
            variant="outline"
            className="w-full flex items-center justify-center"
          >
            <LogOut className="mr-2 h-4 w-4" />
            Logout
          </Button>
        </div>
      </div>

      {/* Main content */}
      <div className="lg:pl-64">
        <main className="p-8">
          {children}
        </main>
      </div>
    </div>
  )
}
