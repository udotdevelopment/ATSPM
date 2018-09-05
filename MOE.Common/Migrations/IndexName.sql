--  DROP FUNCTION [dbo].[IndexName]

CREATE FUNCTION [dbo].[IndexName] (@TableId int, @IndexNumber int)
Returns varchar(50)
With EXECUTE as Caller
AS

Begin
DECLARE @IndexName nvarchar(50)

SELECT @IndexName = IndexName
FROM [ToBeProcessedTableIndexes]
WHERE TableId = @TableId 
AND IndexId = @IndexNumber 

Return @IndexName
END;

GO


