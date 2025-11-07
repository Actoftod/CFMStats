# CFMStats 2.0 - Modern Madden Franchise Mode Statistics

> A complete modernization of CFMStats with AI-powered analytics, real-time updates, and beautiful visualizations.

## What's New in 2.0

CFMStats 2.0 is a **complete rewrite** of the original CFMStats application using modern web technologies and AI capabilities.

### Key Improvements

#### 🚀 Modern Tech Stack
- **Frontend**: Next.js 15 (App Router) with React 19
- **Language**: TypeScript for type safety
- **Styling**: Tailwind CSS with shadcn/ui components
- **Database**: PostgreSQL with Prisma ORM
- **Authentication**: NextAuth.js with secure JWT sessions

#### 🤖 AI-Powered Features
- **Natural Language Queries**: Ask questions in plain English
  - "Who are my top 5 receivers this season?"
  - "Which QB should I start this week?"
- **Player Performance Predictions**: AI-powered development forecasts
- **Intelligent Analysis**: Automated game summaries and insights
- **Smart Recommendations**: AI suggestions for trades, drafts, and roster moves

#### 🎨 Modern UI/UX
- Beautiful, responsive design that works on all devices
- Dark mode support
- Advanced data visualization with interactive charts
- Real-time statistics updates
- Lightning-fast performance

#### 📊 Enhanced Features
- **Advanced Analytics Dashboard**: Comprehensive league overview
- **Player Profiles**: Detailed stats, ratings, and AI predictions
- **Team Management**: Complete team stats and roster management
- **Interactive Charts**: Visualize trends and performance data
- **Real-time Sync**: Instant updates from Madden Companion App

## Quick Start

### Prerequisites

- Node.js 20+ and npm 10+
- PostgreSQL 16+
- Docker (optional, for containerized deployment)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/CFMStats.git
   cd CFMStats
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Set up environment variables**
   ```bash
   cp .env.example .env
   ```

   Edit `.env` and configure:
   - `DATABASE_URL`: PostgreSQL connection string
   - `NEXTAUTH_SECRET`: Generate with `openssl rand -base64 32`
   - `ANTHROPIC_API_KEY` or `OPENAI_API_KEY`: For AI features

4. **Set up the database**
   ```bash
   npm run db:push
   npm run db:seed  # Optional: seed with sample data
   ```

5. **Run the development server**
   ```bash
   npm run dev
   ```

6. **Open your browser**
   Navigate to [http://localhost:3000](http://localhost:3000)

## Docker Deployment

For production deployment using Docker:

```bash
# Build and start services
docker-compose up -d

# View logs
docker-compose logs -f app

# Stop services
docker-compose down
```

## Features Overview

### 1. Authentication & User Management
- Secure registration and login
- Password hashing with bcrypt
- JWT-based sessions
- Email verification (optional)

### 2. League Management
- Create and manage multiple leagues
- Public/private league settings
- League ownership and permissions
- Automatic data synchronization

### 3. Player Statistics
- Comprehensive stat tracking:
  - Passing, Rushing, Receiving
  - Defense, Kicking, Punting
- Weekly and season aggregations
- Career statistics
- Historical data

### 4. AI-Powered Analytics

#### Natural Language Queries
```typescript
POST /api/ai/query
{
  "leagueId": "...",
  "query": "Who are my top 5 receivers this season?"
}
```

#### Player Predictions
```typescript
POST /api/ai/predictions
{
  "playerId": "..."
}
// Returns: development forecast, projected stats, recommendations
```

#### Game Analysis
```typescript
POST /api/ai/analyze
{
  "leagueId": "...",
  "type": "player|league|team",
  "query": "Analyze my QB's performance"
}
```

### 5. Data Visualization
- Interactive charts with Chart.js and Recharts
- Performance trends over time
- Comparative analysis
- Team vs. team visualizations

### 6. Real-time Updates
- Server-Sent Events for live data
- Automatic sync from Madden Companion App
- WebSocket support (future)

## API Documentation

### Authentication

**Register**
```http
POST /api/register
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "securepassword"
}
```

**Login**
```http
POST /api/auth/callback/credentials
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "securepassword"
}
```

### Leagues

**Get All Leagues**
```http
GET /api/leagues
Authorization: Bearer <token>
```

**Create League**
```http
POST /api/leagues
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "My CFM League",
  "exportId": "export-123",
  "isPublic": false
}
```

### Players

**Get League Players**
```http
GET /api/leagues/{leagueId}/players?position=QB&page=1&limit=50
Authorization: Bearer <token>
```

### Data Sync

**Sync League Data**
```http
POST /api/sync
Authorization: Bearer <token>
Content-Type: application/json

{
  "leagueId": "...",
  "exportUrl": "https://...",
  "dataType": "rosters|standings|schedule|stats"
}
```

## Database Schema

The application uses PostgreSQL with Prisma ORM. Key models include:

- **User**: User accounts and authentication
- **League**: CFM leagues
- **Team**: Team information and rosters
- **Player**: Player profiles and attributes
- **PlayerRating**: Detailed player ratings (60+ attributes)
- **StatsPassing/Rushing/Receiving/Defense/Kicking/Punting**: Statistics by category
- **ScheduleGame**: Game schedules and results
- **TeamStanding**: Win/loss records and standings

See `prisma/schema.prisma` for the complete schema.

## Technology Stack

| Category | Technology |
|----------|------------|
| **Frontend** | Next.js 15, React 19, TypeScript |
| **Styling** | Tailwind CSS, shadcn/ui |
| **State Management** | TanStack Query, Zustand |
| **Database** | PostgreSQL 16 |
| **ORM** | Prisma |
| **Authentication** | NextAuth.js v5 |
| **AI** | Anthropic Claude, OpenAI GPT |
| **Charts** | Chart.js, Recharts |
| **Forms** | React Hook Form, Zod |
| **Deployment** | Docker, Vercel |

## Development

### Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run start` - Start production server
- `npm run lint` - Run ESLint
- `npm run typecheck` - Run TypeScript compiler checks
- `npm run db:generate` - Generate Prisma Client
- `npm run db:push` - Push schema changes to database
- `npm run db:studio` - Open Prisma Studio
- `npm run test` - Run tests
- `npm run e2e` - Run end-to-end tests

### Project Structure

```
cfmstats/
├── prisma/
│   └── schema.prisma       # Database schema
├── src/
│   ├── app/                # Next.js app router
│   │   ├── (auth)/         # Auth pages (login, register)
│   │   ├── api/            # API routes
│   │   │   ├── ai/         # AI endpoints
│   │   │   ├── auth/       # Authentication
│   │   │   ├── leagues/    # League management
│   │   │   └── sync/       # Data synchronization
│   │   ├── dashboard/      # Dashboard pages
│   │   ├── leagues/        # League pages
│   │   ├── players/        # Player pages
│   │   └── layout.tsx      # Root layout
│   ├── components/         # React components
│   │   ├── ui/             # shadcn/ui components
│   │   └── ...             # Custom components
│   ├── hooks/              # Custom React hooks
│   ├── lib/                # Utility libraries
│   │   ├── auth.ts         # Auth configuration
│   │   ├── prisma.ts       # Prisma client
│   │   └── utils.ts        # Helper functions
│   └── types/              # TypeScript types
├── docker-compose.yml      # Docker configuration
├── Dockerfile              # Container build file
└── package.json            # Dependencies
```

## Migration from Old CFMStats

The old ASP.NET Web Forms application is preserved in the `CFMStats/` directory. The new Next.js application coexists alongside it.

### Data Migration

To migrate data from the old SQL Server database to the new PostgreSQL database:

1. Export data from SQL Server
2. Transform to match new schema
3. Import using Prisma seed scripts

(Migration scripts coming soon)

## AI Features Configuration

### Anthropic Claude (Recommended)
```env
ANTHROPIC_API_KEY=sk-ant-...
```

### OpenAI
```env
OPENAI_API_KEY=sk-...
```

The application will automatically use whichever API key is configured.

## Performance Optimizations

- Server-side rendering (SSR) for SEO
- Static site generation (SSG) where possible
- Image optimization with Next.js Image
- Code splitting and lazy loading
- Database query optimization with Prisma
- Redis caching (future enhancement)

## Security

- Secure password hashing with bcrypt
- JWT-based authentication
- CORS protection
- SQL injection prevention (Prisma)
- XSS protection (React)
- CSRF protection
- Environment variable validation

## Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## License

See the [LICENSE](LICENSE) file for details.

## Support

For issues, questions, or feature requests:
- Open an issue on GitHub
- Check the documentation at docs/
- Join our community Discord (coming soon)

## Roadmap

### Version 2.1 (Q2 2025)
- [ ] Mobile app (React Native)
- [ ] Advanced AI coach assistant
- [ ] Social features (comments, sharing)
- [ ] Discord bot integration
- [ ] Custom league rules engine

### Version 2.2 (Q3 2025)
- [ ] Multi-league comparisons
- [ ] Historical analytics
- [ ] Export to Excel/PDF
- [ ] API webhooks
- [ ] Third-party integrations

## Acknowledgments

- Original CFMStats by [Original Author]
- shadcn/ui for the component library
- Anthropic for Claude AI
- Vercel for Next.js
- The Madden CFM community

---

**CFMStats 2.0** - Built with ❤️ for the Madden CFM community
