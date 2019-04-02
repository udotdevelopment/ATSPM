
-- Drop Function [dbo].[InsertValues]

CREATE FUNCTION [dbo].[InsertValues] (@TableId int)
Returns varchar(100)
With EXECUTE as Caller
AS

Begin
	DECLARE @InsertValues varchar(1000)

  SELECT 	  

	@InsertValues = [InsertValues]
  FROM [dbo].[ToBeProcessededTables]
  WHERE TableId = @TableId 
	
Return @InsertValues
END;

--SELECT [dbo].[TableName] (1)


GO


