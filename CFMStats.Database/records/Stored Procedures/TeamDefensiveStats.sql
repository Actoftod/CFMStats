-- =============================================
-- Author:		AI Assistant
-- Create date:	2025 NOV 07
-- Description:	Team Defensive Records (Game, Season)
-- =============================================

CREATE PROCEDURE [records].[TeamDefensiveStats] @leagueId   INT,
								 @stageIndex INT,
								 @orderBy    VARCHAR(20),
								 @duration   VARCHAR(20)
AS
    BEGIN
	   -- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	   SET NOCOUNT ON;
	   IF @duration = 'game'
		  BEGIN
			 SELECT TOP 10
				   -- Season and Week
				   s.seasonIndex AS Season,
				   s.weekIndex + 1 AS Week,
				   -- Stats
				   defTotalTackles AS Tackles,
				   defSacks AS Sacks,
				   defInts AS Interceptions,
				   defFumblesRec AS FumblesRecovered,
				   defTDs AS DefensiveTouchdowns,
				   -- Team
				   t.teamId,
				   t.displayName AS Team,
				   schedule.awayTeamID AS AwayTeamId,
				   (SELECT TOP 1 displayName FROM tblTeamInfo WHERE teamId = schedule.awayTeamId AND leagueid = @leagueId) AS AwayTeamName,
				   schedule.hometeamId AS HomeTeamId,
				   (SELECT TOP 1 displayName	FROM tblTeamInfo WHERE teamId = schedule.homeTeamID AND leagueid = @leagueId) AS HomeTeamName

				   FROM
					   tblStatsTeam AS s
				   JOIN tblTeamInfo AS t
					   ON t.teamId = s.teamId
						 AND t.leagueId = s.leagueId

				   LEFT JOIN tblSchedule AS schedule
					   ON schedule.scheduleId = s.scheduleId
						 AND schedule.leagueId = @leagueId
						 AND schedule.seasonIndex = s.seasonIndex

				   WHERE s.stageIndex = @stageIndex
					    AND s.leagueId = @leagueId
					    AND s.weekIndex BETWEEN 0 AND 16
				   ORDER BY
						  CASE
							 WHEN @orderBy = 'tackles' THEN defTotalTackles
							 WHEN @orderBy = 'sacks' THEN defSacks
							 WHEN @orderBy = 'interceptions' THEN defInts
							 WHEN @orderBy = 'fumbles' THEN defFumblesRec
							 WHEN @orderBy = 'touchdowns' THEN defTDs
						  END DESC;
	   END;
	   ELSE
		  BEGIN
			 SELECT TOP 10
			 -- Stats
				   SUM(CASE
						 WHEN @orderBy = 'tackles' THEN defTotalTackles
					  END) AS Tackles,
				   SUM(CASE
						 WHEN @orderBy = 'sacks' THEN defSacks
					  END) AS Sacks,
				   SUM(CASE
						 WHEN @orderBy = 'interceptions' THEN defInts
					  END) AS Interceptions,
				   SUM(CASE
						 WHEN @orderBy = 'fumbles' THEN defFumblesRec
					  END) AS FumblesRecovered,
				   SUM(CASE
						 WHEN @orderBy = 'touchdowns' THEN defTDs
					  END) AS DefensiveTouchdowns,
				   s.seasonIndex AS Season,
				   -- Team
				   t.teamId,
				   t.displayName AS Team,
				   -- Games Played
				   COUNT(s.weekIndex) AS Games
				   FROM
					   tblStatsTeam AS s
				   JOIN tblTeamInfo AS t
					   ON t.teamId = s.teamId
						 AND t.leagueId = s.leagueId
				   WHERE s.stageIndex = @stageIndex
					    AND s.leagueId = @leagueId
					    AND s.weekIndex BETWEEN 0 AND 16
				   GROUP BY
						  s.seasonIndex,
						  t.teamId,
						  t.displayName
				   ORDER BY
						  CASE
							 WHEN @orderBy = 'tackles' THEN SUM(defTotalTackles)
							 WHEN @orderBy = 'sacks' THEN SUM(defSacks)
							 WHEN @orderBy = 'interceptions' THEN SUM(defInts)
							 WHEN @orderBy = 'fumbles' THEN SUM(defFumblesRec)
							 WHEN @orderBy = 'touchdowns' THEN SUM(defTDs)
						  END DESC;
	   END;
    END;
