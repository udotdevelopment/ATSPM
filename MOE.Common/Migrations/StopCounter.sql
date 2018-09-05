-- DROP Function [dbo].[StopCounter]

CREATE FUNCTION [dbo].[StopCounter] (@CurrentYear int, @CurentMonth int , @Verbose int)
Returns INT
With EXECUTE as Caller
AS

BEGIN
DECLARE @MonthToKeep int
DECLARE @MonthsToKeepData int
DECLARE @TableName nvarchar(50)
DECLARE @StopCounter int
DECLARE @StartYear int
DECLARE @StartMonth int
DECLARE @StartTime dateTime2(7)

DECLARE	@EndMonth int
DECLARE	@EndYear int
DECLARE @DiffYear int
DECLARE @DiffMonth int
DECLARE @TimeFork int

Set @StopCounter = 120

Select 	@MonthsToKeepData = [MonthsToKeepData]
FROM [dbo].[ApplicationSettings]
Where ApplicationId = 3

IF (@MonthsToKeepData IS NULL)
	SET @MonthsToKeepData = 120  -- 10 years as a default

SELECT @TimeFork = Count (*) 
FROM [MOETestPartition].[dbo].[TablePartitionProcesseds]
WHERE SwapTableName like '%Controller_event_log%'

IF @TimeFork = 0 
BEGIN
	SELECT @StartTime = Min ([Timestamp])
	FROM [dbo].[Controller_Event_Log] 
	SET @StartYear = year (@StartTime )
	SET @StartMonth = month (@StartTime )
END

IF @TimeFork > 0  
BEGIN
	SELECT @StartYear = max (PartitionBeginYear) 
	FROM [dbo].[TablePartitionProcesseds]
	WHERE SwapTableName like '%Controller_event_log%'
				
	SELECT @StartMonth =  MAX (PartitionBeginMonth )
	FROM [dbo].[TablePartitionProcesseds]
	WHERE SwapTableName like '%Controller_event_log%'
	AND PartitionBeginYear = @StartYear  
	SET @CurentMonth = @CurentMonth + 1
END

SET @DiffYear = @CurrentYear - @StartYear 
SET @DiffMonth = @CurentMonth -@StartMonth 

SET @StopCounter = (@DiffYear *12) + @DiffMonth - @MonthsToKeepData  
RETURN @StopCounter

END

-- SELECT [dbo].[StopCounter] (2018, 4,  1)
GO


