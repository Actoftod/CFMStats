CREATE TABLE [dbo].[tblTradeHistory]
(
    [tradeId]               INT IDENTITY(1, 1) NOT NULL,
    [leagueId]              INT NOT NULL,
    [seasonIndex]           INT NOT NULL,
    [weekIndex]             INT NOT NULL,
    [playerId]              INT NOT NULL,
    [playerName]            VARCHAR(100) NOT NULL,
    [position]              VARCHAR(10) NOT NULL,
    [fromTeamId]            INT NOT NULL,
    [fromTeamName]          VARCHAR(50) NOT NULL,
    [toTeamId]              INT NOT NULL,
    [toTeamName]            VARCHAR(50) NOT NULL,
    [tradeDate]             DATETIME2 NOT NULL DEFAULT GETDATE(),
    [CreatedOn]             DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_tblTradeHistory] PRIMARY KEY ([tradeId])
)
