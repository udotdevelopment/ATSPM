--DROP PROCEDURE [dbo].[VerboseStatus]

CREATE PROCEDURE [dbo].[VerboseStatus]

@PartitionedTableName varchar (100),
@PartitionName varchar (100),
@PartitionYear int,
@PartitionMonth int,
@SQLStatementOrMessage nvarchar(4000),
@FunctionOrProcedure varchar (100),
@Notes nvarchar (4000)

AS

DECLARE @Status int

Begin

SET @Status = 1
Insert INTO [dbo].[StatusOfProcessedTables] 
	(PartitionedTableName 
	,TimeEntered  
	,PartitionName
	,PartitionYear
	,PartitionMonth
	,SQLStatementOrMessage
	,FunctionOrProcedure
	,Notes
	)
Values 
	(@PartitionedTableName 
	,getdate() 
	,@PartitionName 
	,@PartitionYear 
	,@PartitionMonth 
	,@SQLStatementOrMessage 
	,@FunctionOrProcedure 
	,@Notes 
	)

	
Return @Status
END;