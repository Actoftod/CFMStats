-- =============================================
-- Author:		AI Assistant
-- Create date:	2025 NOV 07
-- Description:	Select Retired Players for a league
-- =============================================

CREATE PROCEDURE [dbo].[RetiredPlayers_select]
	@leagueId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		p.playerId,
		p.firstName + ' ' + p.lastName AS playerName,
		p.position,
		p.teamName AS lastTeam,
		p.age,
		p.yearsPro,
		p.rookieYear,
		p.legacyScore,
		r.overallRating,
		p.college,
		p.draftRound,
		p.draftPick
	FROM
		tblPlayerProfile p
	LEFT JOIN tblPlayerRatings r ON p.playerId = r.playerId AND p.leagueId = r.leagueId
	WHERE
		p.leagueId = @leagueId
		AND p.isRetired = 1
	ORDER BY
		p.legacyScore DESC,
		p.yearsPro DESC,
		p.lastName ASC;

END
