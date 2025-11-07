import { NextRequest, NextResponse } from 'next/server'
import { anthropic } from '@ai-sdk/anthropic'
import { generateText } from 'ai'
import { prisma } from '@/lib/prisma'

export async function POST(request: NextRequest) {
  try {
    const { leagueId, playerId, query, type } = await request.json()

    if (!query) {
      return NextResponse.json(
        { error: 'Query is required' },
        { status: 400 }
      )
    }

    let context = ''
    let analysisType = type || 'general'

    // Fetch relevant data based on the analysis type
    if (analysisType === 'player' && playerId) {
      const player = await prisma.player.findUnique({
        where: { id: playerId },
        include: {
          ratings: true,
          team: true,
          passingStats: {
            orderBy: { weekIndex: 'desc' },
            take: 10,
          },
          rushingStats: {
            orderBy: { weekIndex: 'desc' },
            take: 10,
          },
          receivingStats: {
            orderBy: { weekIndex: 'desc' },
            take: 10,
          },
          defenseStats: {
            orderBy: { weekIndex: 'desc' },
            take: 10,
          },
        },
      })

      if (player) {
        context = `Player: ${player.firstName} ${player.lastName}
Position: ${player.position}
Team: ${player.team?.displayName || 'Free Agent'}
Overall Rating: ${player.overallRating}
Age: ${player.age}
Years Pro: ${player.yearsPro}

Recent Performance:
${JSON.stringify(player.passingStats.length > 0 ? player.passingStats : player.rushingStats.length > 0 ? player.rushingStats : player.receivingStats.length > 0 ? player.receivingStats : player.defenseStats, null, 2)}

Player Ratings:
${JSON.stringify(player.ratings, null, 2)}
`
      }
    } else if (analysisType === 'league' && leagueId) {
      const league = await prisma.league.findUnique({
        where: { id: leagueId },
        include: {
          teamStandings: {
            orderBy: { wins: 'desc' },
            take: 10,
            include: {
              team: true,
            },
          },
        },
      })

      if (league) {
        context = `League: ${league.name}
Current Week: ${league.currentWeek}
Current Season: ${league.currentSeason}

Top Standings:
${league.teamStandings.map((standing, idx) => `${idx + 1}. ${standing.team.displayName}: ${standing.wins}-${standing.losses}`).join('\n')}
`
      }
    }

    // Generate AI response using Anthropic Claude
    const { text } = await generateText({
      model: anthropic('claude-3-5-sonnet-20241022'),
      system: `You are an expert Madden NFL analyst and CFM (Connected Franchise Mode) strategist.
      You provide detailed, actionable insights about player performance, team strategies, and franchise management.
      Use the provided context data to give specific, data-driven recommendations.
      Be concise but informative. Use statistics to support your analysis.`,
      prompt: `Context:\n${context}\n\nUser Question: ${query}\n\nProvide a detailed analysis and recommendations.`,
      maxTokens: 1000,
    })

    return NextResponse.json({ analysis: text })
  } catch (error) {
    console.error('AI Analysis error:', error)
    return NextResponse.json(
      { error: 'Failed to generate analysis' },
      { status: 500 }
    )
  }
}
