CREATE FUNCTION dbo.GetDestCountFunc(@TripId int)
RETURNS INT
AS
BEGIN 
	DECLARE @count int;
	SELECT @count = COUNT(*) FROM DestinationCityInTrips WHERE TripId = @TripId;
	RETURN @count;

END






CREATE TRIGGER AddDestination
ON DestinationCityInTrips
AFTER INSERT
AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @count int;
		SELECT @count = dbo.GetDestCountFunc((SELECT TOP 1 inserted.TripId from inserted));
		UPDATE DestinationCityInTrips
		SET OrderNumber = @count
		WHERE Id = (SELECT TOP 1 inserted.Id from inserted);
	END


CREATE TRIGGER DeleteDestination
On DestinationCityInTrips
AFTER DELETE 
AS 
	BEGIN
		SET NOCOUNT ON;
		UPDATE DestinationCityInTrips
		SET OrderNumber = OrderNumber - 1
		WHERE OrderNumber > (SELECT Top 1 OrderNumber FROM deleted);
	END



