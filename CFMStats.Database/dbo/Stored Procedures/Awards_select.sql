-- =============================================
-- Author:		AI Assistant
-- Create date:	2025 NOV 07
-- Description:	Select Awards for a league
-- =============================================

CREATE PROCEDURE [dbo].[Awards_select]
	@leagueId INT,
	@seasonIndex INT = NULL,
	@awardType VARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		a.awardId,
		a.leagueId,
		a.seasonIndex,
		a.awardType,
		a.playerId,
		a.playerName,
		a.position,
		a.teamId,
		a.teamName,
		a.CreatedOn
	FROM
		tblAwards a
	WHERE
		a.leagueId = @leagueId
		AND (@seasonIndex IS NULL OR a.seasonIndex = @seasonIndex)
		AND (@awardType IS NULL OR a.awardType = @awardType)
	ORDER BY
		a.seasonIndex DESC,
		CASE a.awardType
			WHEN 'MVP' THEN 1
			WHEN 'OPOY' THEN 2
			WHEN 'DPOY' THEN 3
			WHEN 'OROY' THEN 4
			WHEN 'DROY' THEN 5
			WHEN 'ProBowl' THEN 6
			ELSE 7
		END,
		a.playerName ASC;

END
