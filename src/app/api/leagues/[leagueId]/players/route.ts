import { NextRequest, NextResponse } from 'next/server'
import { prisma } from '@/lib/prisma'

export async function GET(
  request: NextRequest,
  { params }: { params: { leagueId: string } }
) {
  try {
    const { searchParams } = new URL(request.url)
    const position = searchParams.get('position')
    const teamId = searchParams.get('teamId')
    const page = parseInt(searchParams.get('page') || '1')
    const limit = parseInt(searchParams.get('limit') || '50')
    const sortBy = searchParams.get('sortBy') || 'overallRating'
    const sortOrder = searchParams.get('sortOrder') || 'desc'

    const where: any = {
      leagueId: params.leagueId,
    }

    if (position) {
      where.position = position
    }

    if (teamId) {
      where.teamId = teamId
    }

    const [players, total] = await Promise.all([
      prisma.player.findMany({
        where,
        include: {
          team: true,
          ratings: true,
          _count: {
            select: {
              passingStats: true,
              rushingStats: true,
              receivingStats: true,
              defenseStats: true,
            },
          },
        },
        orderBy: {
          [sortBy]: sortOrder as 'asc' | 'desc',
        },
        skip: (page - 1) * limit,
        take: limit,
      }),
      prisma.player.count({ where }),
    ])

    return NextResponse.json({
      players,
      pagination: {
        page,
        limit,
        total,
        totalPages: Math.ceil(total / limit),
      },
    })
  } catch (error) {
    console.error('Get players error:', error)
    return NextResponse.json(
      { error: 'Failed to fetch players' },
      { status: 500 }
    )
  }
}
