CREATE TABLE [dbo].[marketData]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [time] DATETIME NOT NULL, 
    [instrument] NVARCHAR(50) NOT NULL, 
    [price] FLOAT NOT NULL, 
    [volume] BIGINT NOT NULL
)
