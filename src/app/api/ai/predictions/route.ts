import { NextRequest, NextResponse } from 'next/server'
import { anthropic } from '@ai-sdk/anthropic'
import { generateObject } from 'ai'
import { prisma } from '@/lib/prisma'
import { z } from 'zod'

const predictionSchema = z.object({
  playerName: z.string(),
  currentOverall: z.number(),
  predictedOverall: z.number(),
  confidenceLevel: z.enum(['low', 'medium', 'high']),
  developmentPotential: z.string(),
  keyStrengths: z.array(z.string()),
  areasForImprovement: z.array(z.string()),
  projectedStatLine: z.object({
    stat1: z.string(),
    stat2: z.string(),
    stat3: z.string(),
  }),
  recommendations: z.array(z.string()),
})

export async function POST(request: NextRequest) {
  try {
    const { playerId } = await request.json()

    if (!playerId) {
      return NextResponse.json(
        { error: 'Player ID is required' },
        { status: 400 }
      )
    }

    // Fetch player data with complete statistics
    const player = await prisma.player.findUnique({
      where: { id: playerId },
      include: {
        ratings: true,
        team: true,
        passingStats: {
          orderBy: { weekIndex: 'desc' },
          take: 17, // Full season
        },
        rushingStats: {
          orderBy: { weekIndex: 'desc' },
          take: 17,
        },
        receivingStats: {
          orderBy: { weekIndex: 'desc' },
          take: 17,
        },
        defenseStats: {
          orderBy: { weekIndex: 'desc' },
          take: 17,
        },
      },
    })

    if (!player) {
      return NextResponse.json(
        { error: 'Player not found' },
        { status: 404 }
      )
    }

    // Calculate trends
    const statsArray = player.passingStats.length > 0
      ? player.passingStats
      : player.rushingStats.length > 0
      ? player.rushingStats
      : player.receivingStats.length > 0
      ? player.receivingStats
      : player.defenseStats

    // Generate AI prediction
    const { object } = await generateObject({
      model: anthropic('claude-3-5-sonnet-20241022'),
      schema: predictionSchema,
      prompt: `Analyze this Madden NFL player and predict their development trajectory:

Player: ${player.firstName} ${player.lastName}
Position: ${player.position}
Current Overall: ${player.overallRating}
Age: ${player.age}
Years Pro: ${player.yearsPro}
Development Trait: ${player.devTrait}

Current Ratings:
${JSON.stringify(player.ratings, null, 2)}

Recent Performance (Last ${statsArray.length} games):
${JSON.stringify(statsArray, null, 2)}

Based on this data, provide:
1. Predicted overall rating for next season
2. Development potential assessment
3. Key strengths and areas for improvement
4. Projected stat line for next season
5. Specific recommendations for maximizing development

Consider factors like age, current performance trends, development trait, and position-specific metrics.`,
    })

    return NextResponse.json({ prediction: object })
  } catch (error) {
    console.error('AI Prediction error:', error)
    return NextResponse.json(
      { error: 'Failed to generate prediction' },
      { status: 500 }
    )
  }
}
