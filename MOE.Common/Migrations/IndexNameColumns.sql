
--  DROP FUNCTION [dbo].[IndexNameColumns]

CREATE FUNCTION [dbo].[IndexNameColumns] (@TableId int, @IndexNumber int)
Returns nvarchar(200)
With EXECUTE as Caller
AS

Begin
DECLARE @TextForIndex nvarchar(200)

SELECT @TextForIndex  = TextForIndex
FROM [dbo].[ToBeProcessedTableIndexes]
WHERE TableId = @TableId 
AND IndexId = @IndexNumber 

Return @TextForIndex
END;
GO


