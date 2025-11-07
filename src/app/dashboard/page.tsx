import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { BarChart3, Users, Trophy, TrendingUp, Brain } from 'lucide-react'
import Link from 'next/link'
import { Button } from '@/components/ui/button'

export default function DashboardPage() {
  return (
    <div className="min-h-screen bg-background">
      {/* Header */}
      <header className="border-b">
        <div className="container mx-auto flex h-16 items-center justify-between px-4">
          <h1 className="text-2xl font-bold">CFMStats Dashboard</h1>
          <nav className="flex items-center gap-4">
            <Link href="/leagues" className="text-sm hover:text-primary">
              My Leagues
            </Link>
            <Link href="/players" className="text-sm hover:text-primary">
              Players
            </Link>
            <Link href="/ai" className="text-sm hover:text-primary">
              AI Assistant
            </Link>
          </nav>
        </div>
      </header>

      <main className="container mx-auto px-4 py-8">
        {/* Stats Overview */}
        <div className="mb-8 grid gap-4 md:grid-cols-2 lg:grid-cols-4">
          <StatsCard
            title="Total Leagues"
            value="3"
            description="Active leagues"
            icon={<Trophy className="h-4 w-4 text-muted-foreground" />}
          />
          <StatsCard
            title="Players Tracked"
            value="1,234"
            description="Across all leagues"
            icon={<Users className="h-4 w-4 text-muted-foreground" />}
          />
          <StatsCard
            title="Games Played"
            value="156"
            description="Total games this season"
            icon={<BarChart3 className="h-4 w-4 text-muted-foreground" />}
          />
          <StatsCard
            title="AI Insights"
            value="42"
            description="New recommendations"
            icon={<Brain className="h-4 w-4 text-muted-foreground" />}
          />
        </div>

        <div className="grid gap-8 md:grid-cols-2">
          {/* Recent Activity */}
          <Card>
            <CardHeader>
              <CardTitle>Recent Activity</CardTitle>
              <CardDescription>Latest updates from your leagues</CardDescription>
            </CardHeader>
            <CardContent>
              <div className="space-y-4">
                <ActivityItem
                  title="Week 8 stats synced"
                  description="Monday Night League"
                  time="2 hours ago"
                />
                <ActivityItem
                  title="New player signed"
                  description="Tom Brady to Patriots"
                  time="5 hours ago"
                />
                <ActivityItem
                  title="AI prediction generated"
                  description="Patrick Mahomes development forecast"
                  time="1 day ago"
                />
              </div>
            </CardContent>
          </Card>

          {/* AI Features */}
          <Card>
            <CardHeader>
              <CardTitle>AI-Powered Insights</CardTitle>
              <CardDescription>Get intelligent recommendations</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="rounded-lg border p-4">
                <h4 className="mb-2 font-semibold">Natural Language Queries</h4>
                <p className="mb-3 text-sm text-muted-foreground">
                  Ask questions about your franchise in plain English
                </p>
                <Button asChild size="sm">
                  <Link href="/ai/query">Try Now</Link>
                </Button>
              </div>
              <div className="rounded-lg border p-4">
                <h4 className="mb-2 font-semibold">Player Predictions</h4>
                <p className="mb-3 text-sm text-muted-foreground">
                  AI-powered development forecasts for your players
                </p>
                <Button asChild size="sm" variant="outline">
                  <Link href="/ai/predictions">View Predictions</Link>
                </Button>
              </div>
            </CardContent>
          </Card>
        </div>

        {/* Quick Actions */}
        <Card className="mt-8">
          <CardHeader>
            <CardTitle>Quick Actions</CardTitle>
          </CardHeader>
          <CardContent className="flex flex-wrap gap-4">
            <Button asChild>
              <Link href="/leagues/new">Create League</Link>
            </Button>
            <Button asChild variant="outline">
              <Link href="/sync">Sync Data</Link>
            </Button>
            <Button asChild variant="outline">
              <Link href="/players">Browse Players</Link>
            </Button>
            <Button asChild variant="outline">
              <Link href="/stats">View Statistics</Link>
            </Button>
          </CardContent>
        </Card>
      </main>
    </div>
  )
}

function StatsCard({
  title,
  value,
  description,
  icon,
}: {
  title: string
  value: string
  description: string
  icon: React.ReactNode
}) {
  return (
    <Card>
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
        <CardTitle className="text-sm font-medium">{title}</CardTitle>
        {icon}
      </CardHeader>
      <CardContent>
        <div className="text-2xl font-bold">{value}</div>
        <p className="text-xs text-muted-foreground">{description}</p>
      </CardContent>
    </Card>
  )
}

function ActivityItem({
  title,
  description,
  time,
}: {
  title: string
  description: string
  time: string
}) {
  return (
    <div className="flex items-start gap-4">
      <div className="flex h-9 w-9 items-center justify-center rounded-full bg-primary/10">
        <TrendingUp className="h-4 w-4 text-primary" />
      </div>
      <div className="flex-1 space-y-1">
        <p className="text-sm font-medium leading-none">{title}</p>
        <p className="text-sm text-muted-foreground">{description}</p>
        <p className="text-xs text-muted-foreground">{time}</p>
      </div>
    </div>
  )
}
