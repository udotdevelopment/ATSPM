--  DROP PROCEDURE[dbo].[DoTheSwapTable]

CREATE PROCEDURE [dbo].[DoTheSwapTable]
@StagingTableName nvarchar(200) ,
@TableNumber int ,
@PartitionNumber int ,
@PartitionYear int,
@PartitionMonth int,
@Verbose int

AS

DECLARE @NumberOfTables int
DECLARE @Status int
DECLARE @Counter int
DECLARE @LowerBoundary datetime2(7)
DECLARE @LowerYear int
DECLARE @LowerMonth  int
DECLARE @UpperBoundary datetime2(7)
DECLARE @UpperYear int
DECLARE @UpperMonth int
DECLARE @FileGroup nvarchar(100)
DECLARE @PartitionDate nvarchar(20)
DECLARE @IndexName varchar(50)
DECLARE @Dummy int
DECLARE @SQLSTATEMENT     nvarchar(4000)
DECLARE @CHK_Constrants  varchar(100)
DECLARE @MainTable nvarchar(200)

Begin

SET @MainTable = [dbo].[TableName] (@TableNumber)

SET @SQLSTATEMENT = 'ALTER TABLE [dbo].[' + @MainTable + ']
	SWITCH PARTITION ' + CAST (@PartitionNumber AS nvarchar(4)) + 
	' TO [dbo].[' + @StagingTableName + ']'
EXEC sp_executesql @SQLSTATEMENT


IF (@Verbose <>0)
	BEGIN
		
		EXEC	@Status  = [dbo].[VerboseStatus]
				@PartitionedTableName = @StagingTableName,
				@PartitionName = N'Need to add code for this',
				@PartitionYear = @PartitionYear ,
				@PartitionMonth = @PartitionMonth ,
				@SQLStatementOrMessage = @SQLSTATEMENT ,
				@FunctionOrProcedure = N'Do the Swap Table',
				@Notes = N'End of the Procedure'

	END

SET @Status = 1
Return @Status

END

/***


DECLARE	@return_value int

EXEC	@return_value = [dbo].[DoTheSwapTable]
		@StagingTableName = N'Staging_Controller_Event_Log_Part-03-2014-02',
		@TableNumber = 1,
		@PArtitionNumber = 3,
		@PartitionYear = 2014,
		@PartitionMonth = 2,
		@Verbose = 1

SELECT	'Return Value' = @return_value


DECLARE	@return_value int

EXEC	@return_value = [dbo].[DoTheSwapTable]
		@StagingTableName = N'Staging_Speed_Events_Part-03-2014-02',
		@TableNumber = 2,
		@PArtitionNumber = 3,
		@PartitionYear = 2014,
		@PartitionMonth = 2,
		@Verbose = 1

SELECT	'Return Value' = @return_value


***/
