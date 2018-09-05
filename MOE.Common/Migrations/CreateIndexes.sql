
--  DROP PROCEDURE [dbo].[CreateIndexes]

CREATE PROCEDURE [dbo].[CreateIndexes]
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
		SET @TextForIndex = [dbo].[IndexNameColumns] (@TableNumber, @IndexCounter )

		SET @SQLSTATEMENT = 'CREATE ' + @ClusterOrNo + ' INDEX [' + @IndexName + '] ON [dbo].[' + @StagingTableName + ']' + @TextForIndex + ' ON [' + @FileGroup + ']'

		IF (@Verbose <>0)
			BEGIN
				SET @MyNotes = N'Table ' + Cast (@TableNumber AS varchar(2)) + ' Index ' + Cast (@IndexCounter AS varchar(2))
				EXEC	@Status  = [dbo].[VerboseStatus]
						@PartitionedTableName = @StagingTableName ,
						@PartitionName = @FileGroup ,
						@PartitionYear = @PartitionYear ,
						@PartitionMonth = @PartitionMonth ,
						@SQLStatementOrMessage = @SQLSTATEMENT ,
						@FunctionOrProcedure = N'Create Indexes',
						@Notes = @MyNotes 
			END
		EXEC sp_executesql @SQLSTATEMENT

		SET @IndexCounter = @IndexCounter + 1
END

SET @Status = 1
Return @Status
END
GO


