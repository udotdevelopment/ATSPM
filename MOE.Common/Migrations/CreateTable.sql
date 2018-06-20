-- DROP PROCEDURE [dbo].[CreateTable]

CREATE PROCEDURE [dbo].[CreateTable]
@StagingTableName nvarchar(100) ,
@TableNumber int ,
@PartitionNumber int ,
@PartitionYear int,
@PartitionMonth int,
@Verbose int


AS

DECLARE @NumberOfTables int
DECLARE @Status int
DECLARE @Counter int
DECLARE @FileGroup nvarchar(100)
DECLARE @PartitionDate nvarchar(20)
DECLARE @IndexName varchar(50)
DECLARE @Dummy int
DECLARE @SQLSTATEMENT     nvarchar(4000)
DECLARE @TableCompressionType varchar(10)
DECLARE @TableName varchar (100)
DECLARE @CreateTableColumns nvarchar (500)
DECLARE @TableExists int
DECLARE @TablePartitionProcessedsEntry int
DECLARE @StartTime datetime2(7)
DECLARE @CountOfFileGroupNeedShrinkRows int = 0

BEGIN
	SELECT @TableExists = Count (*) FROM INFORMATION_SCHEMA.TABLES 
           WHERE TABLE_NAME = @StagingTableName
	
	IF (@TableExists > 0)
	BEGIN
  		SELECT @TablePartitionProcessedsEntry = count(*)
		FROM [dbo].[TablePartitionProcesseds]
		WHERE SwapTableName = @StagingTableName 
		
		IF (@TablePartitionProcessedsEntry = 0)
		BEGIN
			INSERT INTO [dbo].[TablePartitionProcesseds] (
						 [SwapTableName]
						,[PartitionNumber]
						,[PartitionBeginYear]
						,[PartitionBeginMonth]
						,[FileGroupName]
						,[IndexRemoved]
						,[SwappedTableRemoved]
						,[TimeIndexdropped]
						,[TimeSwappedTableDropped]	)
			VALUES (
						  @StagingTableName 
						, @PartitionNumber
						, @PartitionYear  
						, @PartitionMonth  
						, @FileGroup 
						, 1
						, 0
						, getdate ()
						, getdate ())
		END
		IF (@TablePartitionProcessedsEntry > 0)
		BEGIN
			UPDATE [dbo].[TablePartitionProcesseds] 
			SET [IndexRemoved] = 1
				,[TimeIndexdropped] = getdate ()
				,[TimeSwappedTableDropped] = getdate()
			WHERE SwapTableName = @StagingTableName
				AND PartitionNumber = @PartitionNumber
				AND PartitionBeginYear = @PartitionYear  
				AND PartitionBeginMonth = @PartitionMonth  
				AND FileGroupName = @FileGroup 
		END
	END

	IF (@TableExists = 0)
	BEGIN
		SET @TableName = [dbo].[TableName] (@TableNumber)
		SET @IndexName = [dbo].[IndexName](@TableNumber, 1)
		SET @FileGroup = [dbo].[FileGroup] (@TableName, @IndexName , @PartitionNumber)

		SELECT @CreateTableColumns = [CreateColumns4Table]
		FROM [dbo].[ToBeProcessededTables] 
		Where [TableId] = @TableNumber 

		SET @SQLSTATEMENT = 'CREATE TABLE [dbo].[' + @StagingTableName +']('
			+ @CreateTableColumns + ') ON [' + @FileGroup + ']'
		IF (@Verbose <>0)
		BEGIN
			EXEC	@Status  = [dbo].[VerboseStatus]
					@PartitionedTableName = @StagingTableName,
					@PartitionName = @FileGroup ,
					@PartitionYear = @PartitionYear ,
					@PartitionMonth = @PartitionMonth ,
					@SQLStatementOrMessage = @SQLSTATEMENT ,
					@FunctionOrProcedure = N'Create Tables',
					@Notes = N'Step 1'
		END
		EXEC sp_executesql @SQLSTATEMENT

		Select @TableCompressionType = sp.data_compression_desc
		FROM sys.partitions SP
			INNER JOIN sys.tables ST ON
			st.object_id = sp.object_id
		WHERE name = @TableName 
			and sp.partition_number = @PartitionNumber
			AND  sp.index_id = 1
		SET @SQLSTATEMENT = 'ALTER TABLE [dbo].[' + @StagingTableName + ']' + ' REBUILD PARTITION = ALL
			WITH (DATA_COMPRESSION = ' + @TableCompressionType + ')'

		IF (@Verbose <>0)
			BEGIN
				EXEC	@Status  = [dbo].[VerboseStatus]
						@PartitionedTableName = @StagingTableName ,
						@PartitionName = @FileGroup ,
						@PartitionYear = @PartitionYear ,
						@PartitionMonth = @PartitionMonth ,
						@SQLStatementOrMessage = @SQLSTATEMENT ,
						@FunctionOrProcedure = N'Create Table',
						@Notes = N'Step 2 after compression'
			END


		SET @StartTime = getdate()
		INSERT INTO [dbo].[TablePartitionProcesseds] (
						 [SwapTableName]
						,[PartitionNumber]
						,[PartitionBeginYear]
						,[PartitionBeginMonth]
						,[FileGroupName]
						,[IndexRemoved]
						,[SwappedTableRemoved]
						,[TimeIndexdropped]
						,[TimeSwappedTableDropped]	)
		VALUES (
						  @StagingTableName 
						, @PartitionNumber
						, @PartitionYear  
						, @PartitionMonth  
						, @FileGroup 
						, 1
						, 0
						, @StartTime 
						, @StartTime )

		SELECT @CountOfFileGroupNeedShrinkRows = Count(*)
		FROM [dbo].[ShrinkFileGroups]
		WHERE FileGroupName = @FileGroup

		IF (@CountOfFileGroupNeedShrinkRows = 0)
		BEGIN
			INSERT INTO [dbo].[ShrinkFileGroups] (
				[FileGroupName]
				,[CreatedTimeStamp]
				,[FileGroupNeedsShrink])
			Values (
				@FileGroup
				,@StartTime 
				,0 )
		END
	END

SET @Status = 1
Return @Status

END

/***

DECLARE	@return_value int

EXEC	@return_value = [dbo].[CreateTable]
		@StagingTableName = N'Staging_Controller_Event_Log_Part-03-2014-02',
		@TableNumber = 1,
		@PArtitionNumber = 3,
		@PartitionYear = 2014,
		@PartitionMonth = 2,
		@Verbose = 1

SELECT	'Return Value' = @return_value



DECLARE	@return_value int

EXEC	@return_value = [dbo].[CreateTable]
		@StagingTableName = N'Staging_Speed_Events_Part-03-2014-02',
		@TableNumber = 2,
		@PArtitionNumber = 3,
		@PartitionYear = 2014,
		@PartitionMonth = 2,
		@Verbose = 1

SELECT	'Return Value' = @return_value


***/
