-- =============================================
-- Author:		AI Assistant
-- Create date:	2025 NOV 07
-- Description:	Select Trade History for a league
-- =============================================

CREATE PROCEDURE [dbo].[TradeHistory_select]
	@leagueId INT,
	@seasonIndex INT = NULL,
	@teamId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		th.tradeId,
		th.leagueId,
		th.seasonIndex,
		th.weekIndex,
		th.playerId,
		th.playerName,
		th.position,
		th.fromTeamId,
		th.fromTeamName,
		th.toTeamId,
		th.toTeamName,
		th.tradeDate,
		th.CreatedOn
	FROM
		tblTradeHistory th
	WHERE
		th.leagueId = @leagueId
		AND (@seasonIndex IS NULL OR th.seasonIndex = @seasonIndex)
		AND (@teamId IS NULL OR th.fromTeamId = @teamId OR th.toTeamId = @teamId)
	ORDER BY
		th.seasonIndex DESC,
		th.weekIndex DESC,
		th.tradeDate DESC;

END
