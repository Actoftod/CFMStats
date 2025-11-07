-- =============================================
-- Author:		AI Assistant
-- Create date:	2025 NOV 07
-- Description:	Update or Insert Playoff Bracket matchup
-- =============================================

CREATE PROCEDURE [dbo].[PlayoffBracket_update]
	@leagueId INT,
	@seasonIndex INT,
	@conference VARCHAR(10),
	@round INT,
	@matchupNumber INT,
	@homeTeamId INT,
	@awayTeamId INT,
	@winnerTeamId INT = NULL,
	@scheduleId INT = NULL,
	@homeTeamScore INT = NULL,
	@awayTeamScore INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- Check if record exists
	IF EXISTS (SELECT 1 FROM tblPlayoffBracket
			   WHERE leagueId = @leagueId
			   AND seasonIndex = @seasonIndex
			   AND conference = @conference
			   AND round = @round
			   AND matchupNumber = @matchupNumber)
	BEGIN
		-- Update existing record
		UPDATE tblPlayoffBracket
		SET
			homeTeamId = @homeTeamId,
			awayTeamId = @awayTeamId,
			winnerTeamId = @winnerTeamId,
			scheduleId = @scheduleId,
			homeTeamScore = @homeTeamScore,
			awayTeamScore = @awayTeamScore,
			ModifiedOn = GETDATE()
		WHERE
			leagueId = @leagueId
			AND seasonIndex = @seasonIndex
			AND conference = @conference
			AND round = @round
			AND matchupNumber = @matchupNumber;
	END
	ELSE
	BEGIN
		-- Insert new record
		INSERT INTO tblPlayoffBracket
			(leagueId, seasonIndex, conference, round, matchupNumber,
			 homeTeamId, awayTeamId, winnerTeamId, scheduleId,
			 homeTeamScore, awayTeamScore, CreatedOn, ModifiedOn)
		VALUES
			(@leagueId, @seasonIndex, @conference, @round, @matchupNumber,
			 @homeTeamId, @awayTeamId, @winnerTeamId, @scheduleId,
			 @homeTeamScore, @awayTeamScore, GETDATE(), GETDATE());
	END

END
