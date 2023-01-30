CREATE PROCEDURE [dbo].[UpdateAskLadder]
@askPosition int,
@askPrice float,
@askVolume bigint

AS
UPDATE askLadder 
SET askPrice = @askPrice, askVolume = @askVolume
WHERE askPosition = @askPosition;