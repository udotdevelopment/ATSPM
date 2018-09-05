
-- DROP PROCEDURE[dbo].[PreserveData]

CREATE PROCEDURE [dbo].[PreserveData]
@StagingTableName nvarchar(200) ,
@TableNumber int ,
@PartitionNumber int ,
@PartitionYear int,
@PartitionMonth int,
@Verbose int

AS

DECLARE @Status int
DECLARE @FileGroup nvarchar(100)
DECLARE @Dummy int
DECLARE @SQLSTATEMENT     nvarchar(4000)
DECLARE @MainTable nvarchar(200)
DECLARE @PreserveDataWhere nvarchar (700)
DECLARE @PreserveDataSelect nvarchar (400)
DECLARE @InsertValues nvarchar (500)
DECLARE @DatabaseName nvarchar (40)
DECLARE @ArchivePath nvarchar (200)
DECLARE @PartitionName nvarchar (200)
DECLARE @PhysicalFileName nvarchar (200)
DECLARE @FirstIndexName nvarchar (200)
declare @NumberOfSwappedTablesProcessed int

Begin
	SELECT 
		@MainTable = [PartitionedTableName]
		,@PreserveDataSelect = [PreserveDataSelect]
		,@PreserveDataWhere = [PreserveDataWhere] 
		,@InsertValues = [InsertValues] 
		,@DatabaseName = [DataBaseName] 
	FROM [dbo].[ToBeProcessededTables]
	WHERE TableId = @TableNumber  

	SELECT @ArchivePath = ArchivePath
	FROM ApplicationSettings
	WHERE ApplicationID = 3

	SET @FirstIndexName = [DBO].[IndexName] (1,1)
	SET @PhysicalFileName = [DBO].[PhysicalFileName] (@MainTable, @FirstIndexName, @PartitionNumber)
	SET @SQLSTATEMENT = 'INSERT INTO [dbo].[' + @MainTable + ']  ' +@PreserveDataSelect 
		+ '	FROM [dbo].[' + @StagingTableName  +']  ' + @PreserveDataWhere  

	IF (@Verbose = 1)
	BEGIN
		SET @PartitionName = 'Partition Number is ' + Convert( varchar(4), @PartitionNumber) 
		EXEC [dbo].[VerboseStatus]
			@PartitionedTableName = @StagingTableName,
			@PartitionName = @PartitionName,
			@PartitionYear = @PartitionYear,
			@PartitionMonth = @PartitionMonth, 
			@SQLStatementOrMessage = @SQLSTATEMENT ,
			@FunctionOrProcedure = 'PreserveData ',
			@Notes = 'End of Procedure'
	END
	EXEC sp_executesql @SQLSTATEMENT

	SET @SQLSTATEMENT = 'bcp [' +@DatabaseName +'].[dbo].[' + @StagingTableName 
		+ '] OUT ' + @ArchivePath + @StagingTableName 
		+ '-n.txt -T -n -o ' + @ArchivePath + 'status.txt'

	IF (@Verbose = 1)
	BEGIN
		EXEC [dbo].[VerboseStatus]
			@PartitionedTableName = @StagingTableName,
			@PartitionName = 'EXEC master..xp_cmdshell',
			@PartitionYear = @PartitionYear,
			@PartitionMonth = @PartitionMonth, 
			@SQLStatementOrMessage = @SQLSTATEMENT ,
			@FunctionOrProcedure = 'PreserveData ',
			@Notes = 'Before bcp Command, plan about ten hours.'
	END
	EXEC master..xp_cmdshell @SQLSTATEMENT

	SET @SQLSTATEMENT = 'DROP TABLE [dbo].[' + @StagingTableName  + ']'
	IF (@Verbose = 1)
	BEGIN
		EXEC [dbo].[VerboseStatus]
			@PartitionedTableName = @StagingTableName,
			@PartitionName = @PartitionName,
			@PartitionYear = @PartitionYear,
			@PartitionMonth = @PartitionMonth, 
			@SQLStatementOrMessage = @SQLSTATEMENT ,
			@FunctionOrProcedure = 'PreserveData ',
			@Notes = 'Dropping the stagging table'
		END
	EXEC sp_executesql @SQLSTATEMENT 
	
	UPDATE [dbo].[TablePartitionProcesseds] 
		SET [SwappedTableRemoved] = 1
				,[TimeIndexdropped] = getdate ()
				,[TimeSwappedTableDropped] = getdate()
				,[PhysicalFileName] = @PhysicalFileName
		WHERE SwapTableName = @StagingTableName
				AND PartitionNumber = @PartitionNumber
				AND PartitionBeginYear = @PartitionYear  
				AND PartitionBeginMonth = @PartitionMonth  

	SET @Status = 1
	Return @Status
END
GO


