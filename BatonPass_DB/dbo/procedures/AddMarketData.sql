CREATE PROCEDURE [dbo].[AddMarketData]
@time datetime,
@instrument nvarchar(50),
@price float,
@volume bigint

AS
INSERT INTO marketData (time, instrument, price, volume)
VALUES (@time,@instrument, @price, @volume);
