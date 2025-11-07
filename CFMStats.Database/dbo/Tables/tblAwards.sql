CREATE TABLE [dbo].[tblAwards]
(
    [awardId]               INT IDENTITY(1, 1) NOT NULL,
    [leagueId]              INT NOT NULL,
    [seasonIndex]           INT NOT NULL,
    [awardType]             VARCHAR(50) NOT NULL, -- 'MVP', 'OROY', 'DROY', 'OPOY', 'DPOY', 'ProBowl'
    [playerId]              INT NOT NULL,
    [playerName]            VARCHAR(100) NOT NULL,
    [position]              VARCHAR(10) NOT NULL,
    [teamId]                INT NOT NULL,
    [teamName]              VARCHAR(50) NOT NULL,
    [CreatedOn]             DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_tblAwards] PRIMARY KEY ([awardId])
)
