-- =============================================
-- Author:		AI Assistant
-- Create date:	2025 NOV 07
-- Description:	Insert Award record
-- =============================================

CREATE PROCEDURE [dbo].[Awards_insert]
	@leagueId INT,
	@seasonIndex INT,
	@awardType VARCHAR(50),
	@playerId INT,
	@playerName VARCHAR(100),
	@position VARCHAR(10),
	@teamId INT,
	@teamName VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	-- Check if award already exists (prevent duplicates)
	IF NOT EXISTS (SELECT 1 FROM tblAwards
				   WHERE leagueId = @leagueId
				   AND seasonIndex = @seasonIndex
				   AND awardType = @awardType
				   AND playerId = @playerId)
	BEGIN
		INSERT INTO tblAwards
			(leagueId, seasonIndex, awardType, playerId, playerName, position,
			 teamId, teamName, CreatedOn)
		VALUES
			(@leagueId, @seasonIndex, @awardType, @playerId, @playerName, @position,
			 @teamId, @teamName, GETDATE());
	END

END
