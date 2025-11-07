-- =============================================
-- Author:		AI Assistant
-- Create date:	2025 NOV 07
-- Description:	Team Offensive Records (Game, Season)
-- =============================================

CREATE PROCEDURE [records].[TeamOffensiveStats] @leagueId   INT,
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
				   passYds AS PassYards,
				   rushYds AS RushYards,
				   passYds + rushYds AS TotalYards,
				   passTD + rushTD AS Touchdowns,
				   passAtt AS PassAttempts,
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
							 WHEN @orderBy = 'passyards' THEN passYds
							 WHEN @orderBy = 'rushyards' THEN rushYds
							 WHEN @orderBy = 'totalyards' THEN passYds + rushYds
							 WHEN @orderBy = 'touchdowns' THEN passTD + rushTD
							 WHEN @orderBy = 'passatt' THEN passAtt
						  END DESC;
	   END;
	   ELSE
		  BEGIN
			 SELECT TOP 10
			 -- Stats
				   SUM(CASE
						 WHEN @orderBy = 'passyards' THEN passYds
					  END) AS PassYards,
				   SUM(CASE
						 WHEN @orderBy = 'rushyards' THEN rushYds
					  END) AS RushYards,
				   SUM(CASE
						 WHEN @orderBy = 'totalyards' THEN passYds + rushYds
					  END) AS TotalYards,
				   SUM(CASE
						 WHEN @orderBy = 'touchdowns' THEN passTD + rushTD
					  END) AS Touchdowns,
				   SUM(CASE
						 WHEN @orderBy = 'passatt' THEN passAtt
					  END) AS PassAttempts,
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
							 WHEN @orderBy = 'passyards' THEN SUM(passYds)
							 WHEN @orderBy = 'rushyards' THEN SUM(rushYds)
							 WHEN @orderBy = 'totalyards' THEN SUM(passYds + rushYds)
							 WHEN @orderBy = 'touchdowns' THEN SUM(passTD + rushTD)
							 WHEN @orderBy = 'passatt' THEN SUM(passAtt)
						  END DESC;
	   END;
    END;
