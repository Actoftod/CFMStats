-- =============================================
-- Author:		AI Assistant
-- Create date:	2025 NOV 07
-- Description:	Select Playoff Bracket for a league and season
-- =============================================

CREATE PROCEDURE [dbo].[PlayoffBracket_select]
	@leagueId INT,
	@seasonIndex INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		pb.playoffBracketId,
		pb.leagueId,
		pb.seasonIndex,
		pb.conference,
		pb.round,
		pb.matchupNumber,
		pb.homeTeamId,
		pb.awayTeamId,
		pb.winnerTeamId,
		pb.scheduleId,
		pb.homeTeamScore,
		pb.awayTeamScore,
		homeTeam.displayName AS homeTeamName,
		homeTeam.logo AS homeTeamLogo,
		awayTeam.displayName AS awayTeamName,
		awayTeam.logo AS awayTeamLogo,
		winnerTeam.displayName AS winnerTeamName
	FROM
		tblPlayoffBracket pb
	LEFT JOIN tblTeamInfo homeTeam ON pb.homeTeamId = homeTeam.teamId AND homeTeam.leagueId = @leagueId
	LEFT JOIN tblTeamInfo awayTeam ON pb.awayTeamId = awayTeam.teamId AND awayTeam.leagueId = @leagueId
	LEFT JOIN tblTeamInfo winnerTeam ON pb.winnerTeamId = winnerTeam.teamId AND winnerTeam.leagueId = @leagueId
	WHERE
		pb.leagueId = @leagueId
		AND pb.seasonIndex = @seasonIndex
	ORDER BY
		pb.round ASC,
		pb.conference ASC,
		pb.matchupNumber ASC;

END
