CREATE PROCEDURE [dbo].[UpdateBidLadder]
@bidPosition int,
@bidPrice float,
@bidVolume bigint

AS
UPDATE bidLadder 
SET bidPrice = @bidPrice, bidVolume = @bidVolume
WHERE bidPosition = @bidPosition;
