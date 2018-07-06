-- DROP FUNCTION [dbo].[TableName]

CREATE FUNCTION [dbo].[TableName] (@TableId int)
--(@TableName varchar(100), @StaggingName varchar(100), @FiLeGroupName varchar(100), @PartitionNumber int, @LowerBoundaryYears int, @LowerBoundaryMonths int, @Verbose int)
Returns varchar(100)
With EXECUTE as Caller
AS

Begin
--	DECLARE @Id int
	DECLARE @PartitionedTableName varchar(100)
--	DECLARE @Updatedtime datetime2(7)
--	DECLARE @PreserveData varchar(1000)
DECLARE @SQLSTATEMENT  nvarchar(4000)
DECLARE @DatabaseName  nvarchar(100)

SELECT 
--	@Id = [Id]
    @PartitionedTableName = [PartitionedTableName]
--    ,@UpdatedTime = [UpdatedTime]
--    ,@PreserveData = [PreserveData]
--    ,@TableId =[TableId]
  FROM [dbo].[ToBeProcessededTables]
  WHERE TableId = @TableId 

Return @PartitionedTableName
END;

-- SELECT [dbo].[TableName] (1)



