-- DROP Function StopCounter

CREATE FUNCTION [dbo].[StopCounter]  (@CurrentYear int, @CurentMonth int , @Verbose int)
Returns INT
With EXECUTE as Caller
AS
BEGIN
DECLARE @MonthToKeep int
DECLARE @MonthsToKeepIndex int
DECLARE @MonthsToKeepData int
DECLARE @SelectedDeleteOrMove int
DECLARE @TableName nvarchar(50)
DECLARE @StopCounter int
DECLARE @StartYear int
DECLARE @StartMonth int
DECLARE @StartTime dateTime2(7)

DECLARE	@EndMonth int
DECLARE	@EndYear int
DECLARE @DiffYear int
DECLARE @DiffMonth int

Set @StopCounter = 120

Select  
	@MonthsToKeepIndex = [MonthsToKeepIndex]
	,@MonthsToKeepData = [MonthsToKeepData]
	,@SelectedDeleteOrMove = [SelectedDeleteOrMove] 
FROM [dbo].[ApplicationSettings]
Where ApplicationId = 3

IF (@MonthsToKeepData IS NULL)
	SET @MonthsToKeepData = 120  -- 10 years as a default
IF (@MonthsToKeepIndex IS NULL) 
	SET @MonthsToKeepIndex = 120 -- 10 years as a default
IF (@SelectedDeleteOrMove IS NULL) 
	SET @SelectedDeleteOrMove = 0 -- 0 Do Nothing, 1 is Delete, 2 is move

SELECT @StartTime = Min ([Timestamp])   FROM [dbo].[Controller_Event_Log]

SET @StartYear = year (@StartTime )
SET @StartMonth = month (@StartTime )

SET @DiffYear = @CurrentYear - @StartYear 
SET @DiffMonth = @CurentMonth -@StartMonth 

SET @StopCounter = (@DiffYear *12) + @DiffMonth - @MonthsToKeepIndex 
RETURN @StopCounter

END;

-- SELECT [dbo].[StopCounter] (2018, 4,  1)