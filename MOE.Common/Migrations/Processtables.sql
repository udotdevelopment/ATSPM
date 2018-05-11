-- DROP PROCEDURE [dbo].[ProcessTables]

CREATE PROCEDURE [dbo].[ProcessTables]
@ProcessingYear int,
@ProcessingMonth int,
@MainCounter int,
@StopDropping int,
@Verbose bit

AS

DECLARE @NumberOfTables int
DECLARE @Status int
DECLARE @Counter int
DECLARE @TableName nvarchar(100)
DECLARE @PartitionNumber int
DECLARE @NewStagingTableName nvarchar(200)
DECLARE @TensMonthSpace varchar(1)
DECLARE @TensPartitionSpace varchar(1)
DECLARE @TableHasIndexRemoved int
DECLARE @TableHasBeenDropped int
DECLARE @FileGroup nvarchar(100)
DECLARE @PartitionDate nvarchar(20)
DECLARE @IndexName nvarchar(100)
DECLARE @Dummy int
DECLARE @SQLSTATEMENT     nvarchar(4000)
DECLARE @TableCompressionType varchar(10)
DECLARE @MyNote nvarchar (100)
DECLARE @FileGroupNeedsShrink int

Begin

IF (@ProcessingMonth  < 10)
	SET @TensMonthSpace = '0'
ELSE
	SET @TensMonthSpace = ''

SET @PartitionDate = CAST(@ProcessingYear AS nvarchar(4)) + '-' + CAST (@ProcessingMonth AS nvarchar(2)) + '-15'
SET @PartitionNumber = $PARTITION.[PF_MOEPARTITION_Month](@PartitionDate)
IF (@PartitionNumber < 10)
	SET @TensPartitionSpace = '0'
ELSE
	SET @TensPartitionSpace = ''

SET @Counter = 1

SET @NumberOfTables = 0
SELECT @NumberOfTables = COUNT(*)
FROM [dbo].[ToBeProcessededTables]

While (@Counter <= @NumberOfTables )
BEGIN
	SET @IndexName = [dbo].[IndexName](@Counter , 1)
	SET @TableName = [dbo].[TableName] (@Counter )
	SET @FileGroup = [dbo].[FileGroup] (@TableName , @IndexName , @PartitionNumber)

	SELECT @Verbose = Verbose
	FROM [dbo].[ToBeProcessededTables] 
	WHERE TableId = @Counter

	SET @MyNote = 'MainCounter is ' + Cast (@MainCounter as varchar (3)) + '.  StopDropping is ' + Cast (@StopDropping AS varchar (3)) + '.  Table Counter is ' + Cast (@Counter AS varchar(3)) + ' .  '   
	IF (@Verbose <>0)
		BEGIN
			EXEC	@Status  = [dbo].[VerboseStatus]
				@PartitionedTableName = @MyNote ,
				@PartitionName = @FileGroup ,
				@PartitionYear = @ProcessingYear  ,
				@PartitionMonth = @ProcessingMonth  ,
				@SQLStatementOrMessage = N'Inside While Loop',
				@FunctionOrProcedure = N'ProcessTables',
				@Notes = @MyNote 
		END

	SET @TableName =  [dbo].[TableName] (@Counter )
	SET @IndexName =  [dbo].[IndexName] (@Counter ,1)
	SET @NewStagingTableName = 'Staging_' + @TableName + '_Part-'  + @TensPartitionSpace + CAST (@PartitionNumber AS nvarchar(4))
		+ '-' + CAST (@ProcessingYear AS nvarchar (4)) + '-' + @TensMonthSpace +  CAST (@ProcessingMonth AS nvarchar (2)) 

	SET @TableHasIndexRemoved = 0
	SELECT @TableHasIndexRemoved = [IndexRemoved]
	FROM [dbo].[TablePartitionProcesseds]
	WHERE [SwapTableName] = @NewStagingTableName  
		AND [PartitionBeginYear] = @ProcessingYear  
		AND [PartitionBeginMonth] = @ProcessingMonth 

	IF (@TableHasIndexRemoved = 0)
	BEGIN
		BEGIN Transaction
			EXEC @Status = [dbo].[CreateTable]        @StagingTableName = @NewStagingTableName, @TableNumber = @Counter, @PartitionNumber = @PartitionNumber, @PartitionYear = @ProcessingYear , @PartitionMonth = @ProcessingMonth ,  @Verbose = @Verbose
			EXEC @Status = [dbo].[CreateIndexes]      @StagingTableName = @NewStagingTableName, @TableNumber = @Counter, @PartitionNumber = @PartitionNumber, @PartitionYear = @ProcessingYear , @PartitionMonth = @ProcessingMonth ,  @Verbose = @Verbose
			EXEC @Status = [dbo].[CreateConstraints]  @StagingTableName = @NewStagingTableName, @TableNumber = @Counter, @PartitionNumber = @PartitionNumber, @PartitionYear = @ProcessingYear , @PartitionMonth = @ProcessingMonth ,  @Verbose = @Verbose 
			EXEC @Status = [dbo].[DoTheSwapTable]     @StagingTableName = @NewStagingTableName, @TableNumber = @Counter, @PartitionNumber = @PartitionNumber, @PartitionYear = @ProcessingYear , @PartitionMonth = @ProcessingMonth ,  @Verbose = @Verbose  
		COMMIT

		EXEC @Status = [dbo].[PreserveData] @StagingTableName = @NewStagingTableName, @TableNumber = @Counter, @PartitionNumber = @PartitionNumber, @PartitionYear = @ProcessingYear , @PartitionMonth = @ProcessingMonth ,  @Verbose = @Verbose
		EXEC @Status = [dbo].[DropIndexes]  @StagingTableName = @NewStagingTableName, @TableNumber = @Counter, @PartitionNumber = @PartitionNumber, @PartitionYear = @ProcessingYear , @PartitionMonth = @ProcessingMonth ,  @Verbose = @Verbose
	END

	EXEC @Status = [dbo].[DropOrCompressTable] @StagingTableName = @NewStagingTableName, @MainCounter = @MainCounter, @StopDropping = @StopDropping, @PartitionNumber = @PartitionNumber, @PartitionYear = @ProcessingYear, @PartitionMonth = @ProcessingMonth, @Verbose =@Verbose 

	SET @IndexName = [dbo].[IndexName](@Counter , 1)
	SET @TableName = [dbo].[TableName] (@Counter )
	SET @FileGroup = [dbo].[FileGroup] (@TableName , @IndexName , @PartitionNumber)
	
	IF (@Verbose <>0)
		BEGIN
			EXEC	@Status  = [dbo].[VerboseStatus]
					@PartitionedTableName = @NewStagingTableName ,
					@PartitionName = @FileGroup ,
					@PartitionYear = @ProcessingYear ,
					@PartitionMonth = @ProcessingMonth ,
					@SQLStatementOrMessage = @SQLSTATEMENT ,
					@FunctionOrProcedure = N'ProcessTables',
					@Notes = N'Step After Compression'
		END
	
	SET @Counter = @Counter + 1
END

SET @FileGroupNeedsShrink = 0
SELECT top 1 @FileGroupNeedsShrink = [FileGroupNeedsShrink]
FROM [dbo].[ShrinkFileGroups]
Where FilegroupName = @FileGroup

-- IF (@FileGroupNeedsShrink > 0)
-- BEGIN
	SET @SQLSTATEMENT = 'DBCC SHRINKFILE (N''' + @FileGroup + ''' , 1)'

	IF (@Verbose <>0)
	BEGIN
		EXEC	@Status  = [dbo].[VerboseStatus]
				@PartitionedTableName = @NewStagingTableName ,
				@PartitionName = @FileGroup ,
				@PartitionYear = @ProcessingYear ,
				@PartitionMonth = @ProcessingMonth ,
				@SQLStatementOrMessage = @SQLSTATEMENT ,
				@FunctionOrProcedure = N'ProcessTables',
				@Notes = N'Step Shrink'

	END
	EXEC sp_executesql @SQLSTATEMENT
-- END

SET @Status = 1
Return @Status
END;
