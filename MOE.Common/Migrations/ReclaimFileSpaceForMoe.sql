
DECLARE @Counter int = 1
DECLARE @StartTime datetime2(7) = getdate()
DECLARE @StopCounter int
DECLARE @StopDropping int = 0
DECLARE @MonthsToDrop int = 0
DEclare @NeedToCompress int = 0
DECLARE @NeedToDrop INT = 0
DECLARE @NeedToShrink int = 0
DECLARE @IndexHasBeenRemoved int = 0
DECLARE @Status int = 0
DECLARE @Verbose int 
DECLARE @CurentYear int
DECLARE @CurentMonth int
DECLARE @MinimumDate datetime2(7)

SET @Verbose = 0
SELECT @Verbose = Verbose 
FROM [dbo].[ToBeProcessededTables] 
WHERE TableId = 1

SET @CurentYear = year(@StartTime)
SET @CurentMonth = month(@StartTime)

SET @StopCounter = [dbo].[StopCounter] (@CurentYear , @CurentMonth ,@Verbose )
SET @StopDropping = [dbo].[StopDropping] (@CurentYear , @CurentMonth ,@Verbose )

SELECT @MinimumDate  = Min ([Timestamp])
  FROM [dbo].[Controller_Event_Log]

SET @CurentYear = year(@MinimumDate )
SET @CurentMonth = month(@MinimumDate )


IF (@Verbose <>0)
BEGIN
	EXEC	@Status  = [dbo].[VerboseStatus]
			@PartitionedTableName = N'All Tables',
			@PartitionName = N'Start of Reclaim File Space First',
			@PartitionYear = @StopCounter ,
			@PartitionMonth = @StopDropping  ,
			@SQLStatementOrMessage = N'Start of Reclaim File Space',
			@FunctionOrProcedure = N'Main - Reclaim File Space',
			@Notes = N'Step 1'
END

While (@Counter < @StopCounter )
BEGIN
	EXEC	@Status = [dbo].[ProcessTables]
			@ProcessingYear = @CurentYear,
			@ProcessingMonth = @CurentMonth,
			@MainCounter = @Counter ,
			@StopDropping = @StopDropping,
			@Verbose = @Verbose 

	IF (@Verbose <>0)
BEGIN
	EXEC	@Status  = [dbo].[VerboseStatus]
			@PartitionedTableName = N'All Tables',
			@PartitionName = N'All Partition Names',
			@PartitionYear = @Counter ,
			@PartitionMonth = @Verbose ,
			@SQLStatementOrMessage = N'After EXEC ProcessTables',
			@FunctionOrProcedure = N'Main - Reclaim File Space',
			@Notes = N'Step 2' 
END
	SET @CurentMonth = @CurentMonth + 1
	IF (@CurentMonth > 12)
	BEGIN
		SET @CurentMonth = 1
		SET @CurentYear = @CurentYear + 1
	END

	SET @Counter = @Counter + 1

END



