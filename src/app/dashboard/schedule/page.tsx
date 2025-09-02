'use client'

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../../components/ui/card'
import { Button } from '../../../components/ui/button'
import { Plus } from 'lucide-react'

export default function SchedulePage() {
  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Schedule</h1>
          <p className="text-gray-600">Manage project schedules and timelines</p>
        </div>
        <Button>
          <Plus className="mr-2 h-4 w-4" />
          Add Schedule
        </Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Project Schedule</CardTitle>
          <CardDescription>
            Overview of all project schedules
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="text-center py-8 text-gray-500">
            Schedule management functionality coming soon...
          </div>
        </CardContent>
      </Card>
    </div>
  )
}
