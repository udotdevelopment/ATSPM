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
DECLARE @PreserveDataWhere nvarchar (500)
DECLARE @PreserveDataSelect nvarchar (500)
DECLARE @InsertValues nvarchar (500)

Begin
	SELECT 
		@MainTable = [PartitionedTableName]
		,@PreserveDataSelect = [PreserveDataSelect]
		,@PreserveDataWhere = [PreserveDataWhere] 
		,@InsertValues = [InsertValues] 
	FROM [dbo].[ToBeProcessededTables]
	WHERE TableId = @TableNumber  

	SET @SQLSTATEMENT = 'INSERT INTO [dbo].[' + @MainTable + ']  ' +@PreserveDataSelect 
		+ '	FROM [dbo].[' + @StagingTableName  +']  ' + @PreserveDataWhere  

	IF (@Verbose = 1)
	BEGIN

		EXEC [dbo].[VerboseStatus]

		@PartitionedTableName = @StagingTableName,
		@PartitionName = 'Need to Work this out',
		@PartitionYear = @PartitionYear,
		@PartitionMonth = @PartitionMonth, 
		@SQLStatementOrMessage = @SQLSTATEMENT ,
		@FunctionOrProcedure = 'PreserveData ',
		@Notes = 'End of Procedure'

	END

	EXEC sp_executesql @SQLSTATEMENT

SET @Status = 1

Return @Status

END

/***
DECLARE	@return_value int

EXEC	@return_value = [dbo].[PreserveData]
		@StagingTableName = N'Staging_ControllerEvent_Log_Part-03-2014-02',
		@TableNumber = 1,
		@PArtitionNumber = 3,
		@PartitionYear = 2014,
		@PartitionMonth = 2,
		@Verbose = 1

SELECT	'Return Value' = @return_value



DECLARE	@return_value int

EXEC	@return_value = [dbo].[PreserveData]
		@StagingTableName = N'Staging_Speed_Events_Part-03-2014-02',
		@TableNumber = 2,
		@PArtitionNumber = 3,
		@PartitionYear = 2014,
		@PartitionMonth = 2,
		@Verbose = 1

SELECT	'Return Value' = @return_value

***/

