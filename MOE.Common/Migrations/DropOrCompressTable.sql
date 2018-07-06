--  DROP PROCEDURE [dbo].[DropOrCompressTable]

CREATE PROCEDURE [dbo].[DropOrCompressTable]
@StagingTableName nvarchar(200) ,
@MainCounter int ,
@StopDropping int,
@PartitionNumber int,
@PartitionYear int,
@PartitionMonth int,
@Verbose int

AS

DECLARE @Status int
DECLARE @FileGroup nvarchar(100)
DECLARE @TableName varchar (100)
DECLARE @IndexName varchar(100)
DECLARE @MainIndexName varchar (50)
DECLARE @SQLSTATEMENT     nvarchar(4000)
DECLARE @IndexCounter int
DECLARE @TableExists int
DECLARE @ClusterOrNo Varchar (20)
DECLARE @MyNotes  varchar (50)
DECLARE @DoesTableExisits int
DECLARE @IsThereAnEntryInTablePartitionProcesseds int
DECLARE @SwappedTableRemoved int
DECLARE @Action varchar (25) = N'Start'
DECLARE @HowToRecord varchar (25)
DECLARE @TableCompressionType varchar (10)
DECLARE @CountOfFileGroupNeedShrinkRows int
DECLARE @myTime datetime2 (7)
DECLARE @TimeIndexdropped datetime2 (7)
DECLARE @TimeSwappedTableDropped datetime2 (7)

Begin

	SELECT @TableExists = Count (*) 
	FROM INFORMATION_SCHEMA.TABLES 
	WHERE TABLE_NAME = @StagingTableName

	Select @IsThereAnEntryInTablePartitionProcesseds = Count(*)
	FROM [dbo].[TablePartitionProcesseds] 
	WHERE SwapTableName = @StagingTableName 
		AND PartitionBeginYear = @PartitionYear
		AND PartitionBeginMonth = @PartitionMonth 

	IF (@TableExists > 0)
	BEGIN
		IF (@MainCounter <= @StopDropping )
		BEGIN
			SET @Action = N'Drop '
			SET @SwappedTableRemoved = 1
			SET @SQLSTATEMENT = 'DROP TABLE [dbo].[' + @StagingTableName  + ']'
			EXEC sp_executesql @SQLSTATEMENT 

			SELECT @CountOfFileGroupNeedShrinkRows = Count(*)
			FROM [dbo].[ShrinkFileGroups]
			WHERE FileGroupNeedsShrink = @FileGroup

			IF (@CountOfFileGroupNeedShrinkRows = 0)
			BEGIN
				INSERT INTO [dbo].[ShrinkFileGroups] (
					[FileGroupName]
					,[CreatedTimeStamp]
					,[FileGroupNeedsShrink])
				Values (
					@FileGroup
					,getdate()
					,1 )
			END

			UPDATE [dbo].[ShrinkFileGroups] 
			SET	[FileGroupNeedsShrink] = 1
			WHERE FileGroupName = @FileGroup 

		End

		IF (@MainCounter > @StopDropping )
		BEGIN
			SET @Action = N'Compress '
			SET @SwappedTableRemoved = 0
			Select @TableCompressionType = sp.data_compression_desc
			FROM sys.partitions SP
				INNER JOIN sys.tables ST ON
				st.object_id = sp.object_id
			WHERE name = @StagingTableName  
			and sp.partition_number = @PartitionNumber
			AND  sp.index_id = 1
	
			IF (@TableCompressionType <> 'PAGE')
			BEGIN
				SET @SQLSTATEMENT = 'ALTER TABLE [dbo].[' + @StagingTableName + ']' + ' REBUILD PARTITION = ALL
						WITH (DATA_COMPRESSION = PAGE )'
				EXEC sp_executesql @SQLSTATEMENT
			END
		END
	END
	IF (@TableExists = 0)
		SET @SwappedTableRemoved = 1

	SET @myTime = getdate ()
	IF (@IsThereAnEntryInTablePartitionProcesseds = 0)
	BEGIN
		SET @HowToRecord = N'Insert '
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
						, @SwappedTableRemoved 
						, @myTime 
						, @myTime 
				)
	END	
	

	IF (@IsThereAnEntryInTablePartitionProcesseds >0)
	BEGIN
	SELECT @TimeIndexdropped = TimeIndexdropped
		, @TimeSwappedTableDropped = TimeSwappedTableDropped 
	FROM [dbo].[TablePartitionProcesseds]
	WHERE SwapTableName = @StagingTableName  
		AND PartitionNumber = @PartitionNumber
		AND PartitionBeginYear = @PartitionYear  
		AND PartitionBeginMonth = @PartitionMonth 

		SET @myTime = @TimeSwappedTableDropped 
		IF (@TimeIndexdropped = @TimeSwappedTableDropped )
			SET @myTime = Getdate ()

		SET @HowToRecord = N'UPDATE '

		UPDATE [dbo].[TablePartitionProcesseds] 
			SET [SwappedTableRemoved] = 1 
		    , [TimeSwappedTableDropped] = @myTime
		WHERE SwapTableName = @StagingTableName  
			AND PartitionNumber = @PartitionNumber
			AND PartitionBeginYear = @PartitionYear  
			AND PartitionBeginMonth = @PartitionMonth  
	END

	IF (@Verbose <>0)
	BEGIN
		SET @MyNotes = N'Action: ' + @Action + 'How to record this event: ' + @HowToRecord    
		EXEC	@Status  = [dbo].[VerboseStatus]
							@PartitionedTableName = @StagingTableName ,
							@PartitionName = 'Unknown here '  ,
							@PartitionYear = @PartitionYear ,
							@PartitionMonth = @PartitionMonth ,
							@SQLStatementOrMessage = @SQLSTATEMENT ,
							@FunctionOrProcedure = N'DropOrCompressTable',
							@Notes = @MyNotes 
	END

SET @Status = 1
Return @Status
END


