import { NextRequest, NextResponse } from 'next/server'
import { prisma } from '@/lib/prisma'
import { getServerSession } from 'next-auth'
import { authConfig } from '@/lib/auth'

export async function GET(request: NextRequest) {
  try {
    const session = await getServerSession(authConfig)

    if (!session?.user) {
      return NextResponse.json({ error: 'Unauthorized' }, { status: 401 })
    }

    const leagues = await prisma.league.findMany({
      where: {
        OR: [
          { ownerId: session.user.id },
          { isPublic: true },
        ],
      },
      include: {
        owner: {
          select: {
            name: true,
            email: true,
          },
        },
        _count: {
          select: {
            teams: true,
            players: true,
          },
        },
      },
      orderBy: {
        updatedAt: 'desc',
      },
    })

    return NextResponse.json({ leagues })
  } catch (error) {
    console.error('Get leagues error:', error)
    return NextResponse.json(
      { error: 'Failed to fetch leagues' },
      { status: 500 }
    )
  }
}

export async function POST(request: NextRequest) {
  try {
    const session = await getServerSession(authConfig)

    if (!session?.user) {
      return NextResponse.json({ error: 'Unauthorized' }, { status: 401 })
    }

    const { name, exportId, isPublic } = await request.json()

    if (!name) {
      return NextResponse.json(
        { error: 'League name is required' },
        { status: 400 }
      )
    }

    const league = await prisma.league.create({
      data: {
        name,
        exportId,
        isPublic: isPublic || false,
        ownerId: session.user.id,
      },
    })

    return NextResponse.json({ league }, { status: 201 })
  } catch (error) {
    console.error('Create league error:', error)
    return NextResponse.json(
      { error: 'Failed to create league' },
      { status: 500 }
    )
  }
}
