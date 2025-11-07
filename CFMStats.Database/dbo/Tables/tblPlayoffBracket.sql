CREATE TABLE [dbo].[tblPlayoffBracket]
(
    [playoffBracketId]      INT IDENTITY(1, 1) NOT NULL,
    [leagueId]              INT NOT NULL,
    [seasonIndex]           INT NOT NULL,
    [conference]            VARCHAR(10) NOT NULL, -- 'AFC' or 'NFC'
    [round]                 INT NOT NULL, -- 1=Wild Card, 2=Divisional, 3=Conference, 4=Super Bowl
    [matchupNumber]         INT NOT NULL, -- 1-4 for wild card/divisional, 1-2 for conference, 1 for super bowl
    [homeTeamId]            INT NULL,
    [awayTeamId]            INT NULL,
    [winnerTeamId]          INT NULL,
    [scheduleId]            INT NULL,
    [homeTeamScore]         INT NULL,
    [awayTeamScore]         INT NULL,
    [CreatedOn]             DATETIME2 NOT NULL DEFAULT GETDATE(),
    [ModifiedOn]            DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_tblPlayoffBracket] PRIMARY KEY ([playoffBracketId])
)
