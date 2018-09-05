
-- DROP PROCEDURE [dbo].[ProcessTables]

CREATE PROCEDURE [dbo].[ProcessTables]
@ProcessingYear int,
@ProcessingMonth int,
@MainCounter int,
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
DECLARE @LowerTimeBoundary datetime2(7)
DECLARE @SwappedTableRemoved int
DECLARE @MainTable nvarchar(200)
DECLARE @LowerYear int
DECLARE @LowerMonth int
DECLARE @LowerBoundaryString nvarchar(24)
DECLARE @DatabaseFileName nvarchar (40)
DECLARE @PhysicalFileName nvarchar (100)
DECLARE @DatabaseName nvarchar (50)

Begin
IF (@ProcessingMonth  < 10)
	SET @TensMonthSpace = '0'
ELSE
	SET @TensMonthSpace = ''
SET @PartitionDate = CAST(@ProcessingYear AS nvarchar(4)) + '-' + CAST (@ProcessingMonth AS nvarchar(2)) + '-15'
SET @PartitionNumber = $PARTITION.[PF_MOE_Month](@PartitionDate)
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
	SET @DatabaseFileName = [dbo].[DatabaseFileName] (@TableName , @IndexName , @PartitionNumber)
	SET @PhysicalFileName = [dbo].[PhysicalFileName] (@TableName , @IndexName , @PartitionNumber)
	SELECT @Verbose = Verbose, @DatabaseName = DatabaseName 
	FROM [dbo].[ToBeProcessededTables] 
	WHERE TableId = @Counter
	IF (@Verbose <>0)
		BEGIN
			SET @MyNote = 'MainCounter is ' + Cast (@MainCounter as varchar (3)) +  '.  Table Counter is ' + Cast (@Counter AS varchar(3)) + ' .  '   
			EXEC	@Status  = [dbo].[VerboseStatus]
						@PartitionedTableName = @MyNote ,
						@PartitionName = @FileGroup ,
						@PartitionYear = @ProcessingYear  ,
						@PartitionMonth = @ProcessingMonth  ,
						@SQLStatementOrMessage = N'Inside While Loop for number of tables',
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
	END
	SET @Counter = @Counter + 1
END
SET @MainTable = [dbo].[TableName] (1)
SET @IndexName = [dbo].[indexName] (1 , 1)
SET @LowerTimeBoundary = [dbo].[LowerBoundary](@MainTable , @IndexName , @PartitionNumber ) 
SET @LowerYear = year (@LowerTimeBoundary)
SET @LowerMonth = month (@LowerTimeBoundary)
IF (@LowerMonth  < 10)
	SET @TensMonthSpace = '0'
ELSE
	SET @TensMonthSpace = ''
SET @LowerBoundaryString =  CAST (@LowerYear AS nvarchar (4)) + '-' + @TensMonthSpace 
		+  CAST (@LowerMonth AS nvarchar (2)) + '-01T00:00:00.000' 
SET @SQLSTATEMENT = 'ALTER PARTITION FUNCTION PF_MOE_Month () MERGE RANGE (N''' + @LowerBoundaryString + ''')'
	IF (@Verbose <>0)
	BEGIN
		EXEC	@Status  = [dbo].[VerboseStatus]
					@PartitionedTableName = @NewStagingTableName ,
					@PartitionName = @FileGroup ,
					@PartitionYear = @ProcessingYear ,
					@PartitionMonth = @ProcessingMonth ,
					@SQLStatementOrMessage = @SQLSTATEMENT ,
					@FunctionOrProcedure = N'ProcessTables',
					@Notes = N'Merging Partition Function' 
	END
	EXEC sp_executesql @SQLSTATEMENT
SET @Counter = 1
While (@Counter <= @NumberOfTables )
BEGIN
	SET @TableName =  [dbo].[TableName] (@Counter )
	SET @NewStagingTableName = 'Staging_' + @TableName + '_Part-'  + @TensPartitionSpace + CAST (@PartitionNumber AS nvarchar(4))
		+ '-' + CAST (@ProcessingYear AS nvarchar (4)) + '-' + @TensMonthSpace +  CAST (@ProcessingMonth AS nvarchar (2)) 
	SET @SwappedTableRemoved = 0
	SELECT @SwappedTableRemoved = [SwappedTableRemoved]
	FROM [dbo].[TablePartitionProcesseds]
	WHERE [SwapTableName] = @NewStagingTableName  
		AND [PartitionBeginYear] = @ProcessingYear  
		AND [PartitionBeginMonth] = @ProcessingMonth 
	IF (@SwappedTableRemoved = 0)
	BEGIN
		EXEC @Status = [dbo].[PreserveData] @StagingTableName = @NewStagingTableName, @TableNumber = @Counter, @PartitionNumber = @PartitionNumber, @PartitionYear = @ProcessingYear , @PartitionMonth = @ProcessingMonth ,  @Verbose = @Verbose
	END
	SET @Counter = @Counter + 1
END
SET @SQLSTATEMENT = 'ALTER DATABASE ' + @DatabaseName + ' REMOVE File [' + @DatabaseFileName + ']'
	IF (@Verbose <>0)
	BEGIN
		SET @MyNote = N'Remove file and file group: ' + @DatabaseFileName + ', ' + @PhysicalFileName 	
		EXEC	@Status  = [dbo].[VerboseStatus]
					@PartitionedTableName = @NewStagingTableName ,
					@PartitionName = @FileGroup ,
					@PartitionYear = @ProcessingYear ,
					@PartitionMonth = @ProcessingMonth ,
					@SQLStatementOrMessage = @SQLSTATEMENT ,
					@FunctionOrProcedure = N'ProcessTables',
					@Notes = @MyNote 
	END
EXEC sp_executesql @SQLSTATEMENT
SET @SQLSTATEMENT = 'ALTER DATABASE ' + @DatabaseName + ' REMOVE Filegroup [' + @FileGroup + ']'
	IF (@Verbose <>0)
	BEGIN
		EXEC	@Status  = [dbo].[VerboseStatus]
					@PartitionedTableName = @NewStagingTableName ,
					@PartitionName = @FileGroup ,
					@PartitionYear = @ProcessingYear ,
					@PartitionMonth = @ProcessingMonth ,
					@SQLStatementOrMessage = @SQLSTATEMENT ,
					@FunctionOrProcedure = N'ProcessTables',
					@Notes = N'Remove Filegroup'
	END
EXEC sp_executesql @SQLSTATEMENT

SET @Status = 1
Return @Status
END;
GO
