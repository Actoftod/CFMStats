-- =============================================
-- Author:		AI Assistant
-- Create date:	2025 NOV 07
-- Description:	Insert Trade History record
-- =============================================

CREATE PROCEDURE [dbo].[TradeHistory_insert]
	@leagueId INT,
	@seasonIndex INT,
	@weekIndex INT,
	@playerId INT,
	@playerName VARCHAR(100),
	@position VARCHAR(10),
	@fromTeamId INT,
	@fromTeamName VARCHAR(50),
	@toTeamId INT,
	@toTeamName VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	-- Check if trade already exists (prevent duplicates)
	IF NOT EXISTS (SELECT 1 FROM tblTradeHistory
				   WHERE leagueId = @leagueId
				   AND playerId = @playerId
				   AND seasonIndex = @seasonIndex
				   AND weekIndex = @weekIndex
				   AND fromTeamId = @fromTeamId
				   AND toTeamId = @toTeamId)
	BEGIN
		INSERT INTO tblTradeHistory
			(leagueId, seasonIndex, weekIndex, playerId, playerName, position,
			 fromTeamId, fromTeamName, toTeamId, toTeamName, tradeDate, CreatedOn)
		VALUES
			(@leagueId, @seasonIndex, @weekIndex, @playerId, @playerName, @position,
			 @fromTeamId, @fromTeamName, @toTeamId, @toTeamName, GETDATE(), GETDATE());
	END

END
