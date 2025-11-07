import { NextRequest, NextResponse } from 'next/server'
import { prisma } from '@/lib/prisma'
import { getServerSession } from 'next-auth'
import { authConfig } from '@/lib/auth'

// This endpoint handles data synchronization from Madden Companion App
export async function POST(request: NextRequest) {
  try {
    const session = await getServerSession(authConfig)

    if (!session?.user) {
      return NextResponse.json({ error: 'Unauthorized' }, { status: 401 })
    }

    const { leagueId, exportUrl, dataType } = await request.json()

    if (!leagueId || !exportUrl) {
      return NextResponse.json(
        { error: 'League ID and export URL are required' },
        { status: 400 }
      )
    }

    // Verify user owns the league
    const league = await prisma.league.findUnique({
      where: { id: leagueId },
    })

    if (!league || league.ownerId !== session.user.id) {
      return NextResponse.json(
        { error: 'Unauthorized access to league' },
        { status: 403 }
      )
    }

    // Fetch data from external source (Firebase/Madden Companion App)
    const response = await fetch(exportUrl)

    if (!response.ok) {
      return NextResponse.json(
        { error: 'Failed to fetch data from export URL' },
        { status: 502 }
      )
    }

    const data = await response.json()

    // Process the data based on type
    let syncResult: any = { updated: 0, created: 0 }

    if (dataType === 'rosters' && data.rosterInfoList) {
      syncResult = await syncRosters(leagueId, data.rosterInfoList)
    } else if (dataType === 'standings' && data.teamStandingInfoList) {
      syncResult = await syncStandings(leagueId, data.teamStandingInfoList)
    } else if (dataType === 'schedule' && data.gameScheduleInfoList) {
      syncResult = await syncSchedule(leagueId, data.gameScheduleInfoList)
    } else if (dataType === 'stats') {
      syncResult = await syncStats(leagueId, data)
    }

    // Update league sync timestamp
    await prisma.league.update({
      where: { id: leagueId },
      data: { lastSyncAt: new Date() },
    })

    return NextResponse.json({
      message: 'Sync completed successfully',
      result: syncResult,
    })
  } catch (error) {
    console.error('Sync error:', error)
    return NextResponse.json(
      { error: 'Failed to sync data' },
      { status: 500 }
    )
  }
}

async function syncRosters(leagueId: string, rosterData: any[]) {
  let created = 0
  let updated = 0

  for (const rosterInfo of rosterData) {
    const existingPlayer = await prisma.player.findFirst({
      where: {
        leagueId,
        rosterId: rosterInfo.rosterId,
      },
    })

    const playerData = {
      firstName: rosterInfo.firstName,
      lastName: rosterInfo.lastName,
      position: rosterInfo.position,
      jerseyNum: rosterInfo.jerseyNum,
      age: rosterInfo.age,
      height: rosterInfo.height,
      weight: rosterInfo.weight,
      overallRating: rosterInfo.overallRating,
      // ... map other fields
    }

    if (existingPlayer) {
      await prisma.player.update({
        where: { id: existingPlayer.id },
        data: playerData,
      })
      updated++
    } else {
      await prisma.player.create({
        data: {
          ...playerData,
          leagueId,
          rosterId: rosterInfo.rosterId,
        },
      })
      created++
    }
  }

  return { created, updated }
}

async function syncStandings(leagueId: string, standingsData: any[]) {
  // Similar implementation for standings
  return { created: 0, updated: 0 }
}

async function syncSchedule(leagueId: string, scheduleData: any[]) {
  // Similar implementation for schedule
  return { created: 0, updated: 0 }
}

async function syncStats(leagueId: string, statsData: any) {
  // Similar implementation for statistics
  return { created: 0, updated: 0 }
}
