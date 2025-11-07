-- =============================================
-- Author:		AI Assistant
-- Create date:	2025 NOV 07
-- Description:	Select Injury Report for a league
-- =============================================

CREATE PROCEDURE [dbo].[InjuryReport_select]
	@leagueId INT,
	@teamId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		p.playerId,
		p.firstName + ' ' + p.lastName AS playerName,
		p.position,
		p.teamId,
		p.teamName,
		p.injuryType,
		p.injuryLength,
		i.InjuryName,
		p.isOnIR,
		p.age,
		p.yearsPro
	FROM
		tblPlayerProfile p
	LEFT JOIN tblInjury i ON p.injuryType = i.ID
	WHERE
		p.leagueId = @leagueId
		AND p.isActive = 1
		AND p.isRetired = 0
		AND (p.injuryLength > 0 OR p.isOnIR = 1)
		AND (@teamId IS NULL OR p.teamId = @teamId)
	ORDER BY
		CASE WHEN p.isOnIR = 1 THEN 0 ELSE 1 END,
		p.injuryLength DESC,
		p.teamName ASC,
		p.lastName ASC;

END
