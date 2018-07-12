--  DROP PROCEDURE [dbo].[DropIndexes]

CREATE PROCEDURE [dbo].[DropIndexes]
@StagingTableName nvarchar(200) ,
@TableNumber int ,
@PartitionNumber int ,
@PartitionYear int,
@PartitionMonth int,
@Verbose int

AS

DECLARE @NumberOfTables int
DECLARE @Status int
DECLARE @FileGroup nvarchar(100)
DECLARE @PartitionDate nvarchar(20)
DECLARE @TableName varchar (100)
DECLARE @IndexName varchar(100)
DECLARE @MainIndexName varchar (50)
DECLARE @SQLSTATEMENT     nvarchar(4000)
DECLARE @IndexCounter int
DECLARE @NumberOfIndexes int
DECLARE @ClusterOrNo Varchar (20)
DECLARE @TextForIndex nvarchar (200)
DECLARE @MyNotes  varchar (25)
DECLARE @DoesIndexExisits int
DECLARE @CountOfFileGroupNeedShrinkRows int

Begin

SELECT @NumberOfIndexes = Count(*)
FROM [dbo].[ToBeProcessedTableIndexes]
WHERE TableId = @TableNumber 

SET @IndexCounter = 1

While (@IndexCounter <= @NumberOfIndexes )
	BEGIN
		SET @MainIndexName  = [dbo].[IndexName](@TableNumber, @IndexCounter )
		SET @TableName = [dbo].[TableName] (@TableNumber)
		SET @FileGroup = [dbo].[FileGroup] (@TableName , @MainIndexName , @PartitionNumber)

		SET @IndexName = @StagingTableName + '_' + @MainIndexName 
		SET @ClusterOrNo = [dbo].[IndexNameClustered] (@TableNumber, @IndexCounter )

		SELECT @DoesIndexExisits = Count (*) 
		FROM sys.indexes 
		WHERE name= @IndexName AND object_id = OBJECT_ID(@StagingTableName)

		IF (@DoesIndexExisits > 0)
		BEGIN
			SET @SQLSTATEMENT = 'DROP INDEX [' + @IndexName + '] ON [dbo].[' + @StagingTableName + '] WITH ( ONLINE = OFF )'
			IF (@Verbose <>0)
				BEGIN
					SET @MyNotes = N'Table ' + Cast (@TableNumber AS varchar(2)) + ' Index ' + Cast (@IndexCounter AS varchar(2))
					EXEC	@Status  = [dbo].[VerboseStatus]
							@PartitionedTableName = @StagingTableName ,
							@PartitionName = @FileGroup ,
							@PartitionYear = @PartitionYear ,
							@PartitionMonth = @PartitionMonth ,
							@SQLStatementOrMessage = @SQLSTATEMENT ,
							@FunctionOrProcedure = N'Drop Indexes',
							@Notes = @MyNotes 
				END

			EXEC sp_executesql @SQLSTATEMENT

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
				,getdate()
				,1 )
			END
		END


		UPDATE [dbo].[ShrinkFileGroups] 
		SET	[FileGroupNeedsShrink] = 1
		WHERE FileGroupName = @FileGroup 

		SET @IndexCounter = @IndexCounter + 1
	END
SET @Status = 1
Return @Status
END

/***

DECLARE	@return_value int

EXEC	@return_value = [dbo].[DropIndexes]
		@StagingTableName = N'Staging_Controller_Event_Log_Part-03-2014-02',
		@TableNumber = 1,
		@PArtitionNumber = 3,
		@PartitionYear = 2014,
		@PartitionMonth = 2,
		@Verbose = 1

SELECT	'Return Value' = @return_value

DECLARE	@return_value int

EXEC	@return_value = [dbo].[DropIndexes]
		@StagingTableName = N'Staging_Speed_Events_Part-03-2014-02',
		@TableNumber = 2,
		@PArtitionNumber = 3,
		@PartitionYear = 2014,
		@PartitionMonth = 2,
		@Verbose = 1

SELECT	'Return Value' = @return_value



***/



