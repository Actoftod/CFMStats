import Link from 'next/link'
import { Button } from '@/components/ui/button'
import { ArrowRight, BarChart3, Brain, Zap, Users, Trophy, TrendingUp } from 'lucide-react'

export default function HomePage() {
  return (
    <div className="flex min-h-screen flex-col">
      {/* Hero Section */}
      <section className="relative overflow-hidden bg-gradient-to-b from-primary/10 via-background to-background py-20 sm:py-32">
        <div className="container mx-auto px-4">
          <div className="mx-auto max-w-3xl text-center">
            <h1 className="mb-6 text-4xl font-bold tracking-tight sm:text-6xl">
              CFMStats <span className="text-primary">2.0</span>
            </h1>
            <p className="mb-4 text-xl text-muted-foreground sm:text-2xl">
              The Modern Way to Track Your Madden Franchise
            </p>
            <p className="mb-8 text-lg text-muted-foreground">
              AI-powered analytics, real-time updates, and beautiful visualizations
              for your Madden NFL Franchise Mode
            </p>
            <div className="flex flex-col gap-4 sm:flex-row sm:justify-center">
              <Button asChild size="lg" className="gap-2">
                <Link href="/register">
                  Get Started <ArrowRight className="h-4 w-4" />
                </Link>
              </Button>
              <Button asChild variant="outline" size="lg">
                <Link href="/login">Sign In</Link>
              </Button>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="py-20">
        <div className="container mx-auto px-4">
          <h2 className="mb-12 text-center text-3xl font-bold">
            Everything You Need, Reimagined
          </h2>
          <div className="grid gap-8 md:grid-cols-2 lg:grid-cols-3">
            <FeatureCard
              icon={<Brain className="h-10 w-10 text-primary" />}
              title="AI-Powered Analytics"
              description="Get intelligent insights, player predictions, and strategic recommendations powered by advanced AI"
            />
            <FeatureCard
              icon={<Zap className="h-10 w-10 text-primary" />}
              title="Real-Time Updates"
              description="Instant stat updates and live game tracking with modern real-time technology"
            />
            <FeatureCard
              icon={<BarChart3 className="h-10 w-10 text-primary" />}
              title="Advanced Visualizations"
              description="Beautiful charts, graphs, and dashboards that make your data come alive"
            />
            <FeatureCard
              icon={<Users className="h-10 w-10 text-primary" />}
              title="Player Management"
              description="Complete player profiles with ratings, abilities, traits, and contract details"
            />
            <FeatureCard
              icon={<Trophy className="h-10 w-10 text-primary" />}
              title="League Management"
              description="Manage multiple leagues, track standings, schedules, and team performance"
            />
            <FeatureCard
              icon={<TrendingUp className="h-10 w-10 text-primary" />}
              title="Statistical Records"
              description="Track season records, career stats, and historical performance data"
            />
          </div>
        </div>
      </section>

      {/* AI Features Section */}
      <section className="bg-muted/50 py-20">
        <div className="container mx-auto px-4">
          <div className="mx-auto max-w-3xl text-center">
            <Brain className="mx-auto mb-4 h-12 w-12 text-primary" />
            <h2 className="mb-4 text-3xl font-bold">AI-Powered Features</h2>
            <p className="mb-8 text-lg text-muted-foreground">
              Harness the power of AI to gain deeper insights into your franchise
            </p>
            <div className="grid gap-6 text-left md:grid-cols-2">
              <div className="rounded-lg border bg-background p-6">
                <h3 className="mb-2 font-semibold">Natural Language Queries</h3>
                <p className="text-sm text-muted-foreground">
                  Ask questions in plain English: "Who are my top 5 receivers this season?"
                </p>
              </div>
              <div className="rounded-lg border bg-background p-6">
                <h3 className="mb-2 font-semibold">Performance Predictions</h3>
                <p className="text-sm text-muted-foreground">
                  AI-powered predictions for player development and game outcomes
                </p>
              </div>
              <div className="rounded-lg border bg-background p-6">
                <h3 className="mb-2 font-semibold">Smart Recommendations</h3>
                <p className="text-sm text-muted-foreground">
                  Get AI suggestions for trades, draft picks, and roster moves
                </p>
              </div>
              <div className="rounded-lg border bg-background p-6">
                <h3 className="mb-2 font-semibold">Automated Analysis</h3>
                <p className="text-sm text-muted-foreground">
                  Automatic game summaries and weekly performance reports
                </p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="py-20">
        <div className="container mx-auto px-4">
          <div className="mx-auto max-w-2xl text-center">
            <h2 className="mb-4 text-3xl font-bold">Ready to Elevate Your Franchise?</h2>
            <p className="mb-8 text-lg text-muted-foreground">
              Join CFMStats 2.0 today and experience the future of franchise mode tracking
            </p>
            <Button asChild size="lg" className="gap-2">
              <Link href="/register">
                Create Your Account <ArrowRight className="h-4 w-4" />
              </Link>
            </Button>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="border-t py-8">
        <div className="container mx-auto px-4 text-center text-sm text-muted-foreground">
          <p>© 2025 CFMStats 2.0. Built with Next.js, TypeScript, and AI.</p>
        </div>
      </footer>
    </div>
  )
}

function FeatureCard({
  icon,
  title,
  description,
}: {
  icon: React.ReactNode
  title: string
  description: string
}) {
  return (
    <div className="rounded-lg border bg-card p-6 transition-shadow hover:shadow-lg">
      <div className="mb-4">{icon}</div>
      <h3 className="mb-2 text-xl font-semibold">{title}</h3>
      <p className="text-muted-foreground">{description}</p>
    </div>
  )
}
