
--  DROP FUNCTION [dbo].[IndexNameClustered]

CREATE FUNCTION [dbo].[IndexNameClustered] (@TableId int, @IndexNumber int)
Returns nvarchar(200)
With EXECUTE as Caller
AS

Begin
DECLARE @ClusteredText nvarchar(200)

SELECT @ClusteredText   = ClusteredText
FROM [dbo].[ToBeProcessedTableIndexes]
WHERE TableId = @TableId 
AND IndexId = @IndexNumber 


Return @ClusteredText
END;
GO


