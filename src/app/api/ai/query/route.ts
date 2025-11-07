import { NextRequest, NextResponse } from 'next/server'
import { anthropic } from '@ai-sdk/anthropic'
import { generateText } from 'ai'
import { prisma } from '@/lib/prisma'

export async function POST(request: NextRequest) {
  try {
    const { leagueId, query } = await request.json()

    if (!query || !leagueId) {
      return NextResponse.json(
        { error: 'League ID and query are required' },
        { status: 400 }
      )
    }

    // Interpret the natural language query and fetch relevant data
    // This is a simplified example - in production, you'd use more sophisticated NL processing

    let data: any = {}
    const lowerQuery = query.toLowerCase()

    // Determine query intent and fetch appropriate data
    if (lowerQuery.includes('top') || lowerQuery.includes('best') || lowerQuery.includes('leading')) {
      if (lowerQuery.includes('passer') || lowerQuery.includes('qb') || lowerQuery.includes('quarterback')) {
        data.passers = await prisma.statsPassing.findMany({
          where: { leagueId },
          include: { player: true },
          orderBy: { passYards: 'desc' },
          take: 10,
        })
      }

      if (lowerQuery.includes('rusher') || lowerQuery.includes('running')) {
        data.rushers = await prisma.statsRushing.findMany({
          where: { leagueId },
          include: { player: true },
          orderBy: { rushYards: 'desc' },
          take: 10,
        })
      }

      if (lowerQuery.includes('receiver') || lowerQuery.includes('wr') || lowerQuery.includes('te')) {
        data.receivers = await prisma.statsReceiving.findMany({
          where: { leagueId },
          include: { player: true },
          orderBy: { recYards: 'desc' },
          take: 10,
        })
      }

      if (lowerQuery.includes('defense') || lowerQuery.includes('tackler') || lowerQuery.includes('sack')) {
        data.defenders = await prisma.statsDefense.findMany({
          where: { leagueId },
          include: { player: true },
          orderBy: { defenseSacks: 'desc' },
          take: 10,
        })
      }
    }

    if (lowerQuery.includes('standing') || lowerQuery.includes('record') || lowerQuery.includes('team')) {
      data.standings = await prisma.teamStanding.findMany({
        where: { leagueId },
        include: { team: true },
        orderBy: { wins: 'desc' },
        take: 10,
      })
    }

    // Use AI to format and present the data
    const { text } = await generateText({
      model: anthropic('claude-3-5-sonnet-20241022'),
      system: `You are a helpful CFM Stats assistant. Answer the user's question about their Madden franchise using the provided data.
      Present the information in a clear, formatted way. Use tables when appropriate.
      If the data doesn't contain enough information to answer the question fully, say so and provide what you can.`,
      prompt: `User Question: "${query}"

Available Data:
${JSON.stringify(data, null, 2)}

Please answer the user's question using this data. Format your response clearly and include relevant statistics.`,
      maxTokens: 1500,
    })

    return NextResponse.json({
      answer: text,
      rawData: data,
    })
  } catch (error) {
    console.error('Natural language query error:', error)
    return NextResponse.json(
      { error: 'Failed to process query' },
      { status: 500 }
    )
  }
}
