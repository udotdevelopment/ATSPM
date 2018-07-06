--  DROP FUNCTION [dbo].[IndexName]

CREATE FUNCTION [dbo].[IndexName] (@TableId int, @IndexNumber int)
Returns varchar(50)
With EXECUTE as Caller
AS

Begin
--DECLARE @FirstIndexNameController nvarchar(50)
--DECLARE @SecondIndexNameController nvarchar(50)
--DECLARE @FirstIndexNameSpeed nvarchar(50)
--DECLARE @SecondIndexNameSpeed nvarchar(50)
--DECLARE @ThirdIndexNameSpeed nvarchar(50)
DECLARE @IndexName nvarchar(50)


SELECT @IndexName = IndexName
FROM [ToBeProcessedTableIndexes]
WHERE TableId = @TableId 
AND IndexId = @IndexNumber 

/***

	SET @FirstIndexNameController  = 'IX_Clustered_Controller_Event_Log_Temp'
	SET @SecondIndexNameController = 'IX_Controller_Event_Log'

	SET @FirstIndexNameSpeed = 'IX_Clustered_Speed_Events'
	SET @SecondIndexNameSpeed = 'IX_ByDetID'
	SET @ThirdIndexNameSpeed = 'ByTimestampByDetID'

IF (@TableId = 1)
BEGIN
	IF (@IndexNumber = 1)
		SET @IndexName = @FirstIndexNameController 
	Else
		SET @IndexName = @SecondIndexNameController
END

ELSE IF (@TableId = 2)
BEGIN
	IF (@IndexNumber = 1)
		SET @IndexName = @FirstIndexNameSpeed
	ELSE IF (@IndexNumber = 2)
		SET @IndexName = @SecondIndexNameSpeed
	ELSE
		SET @IndexName = @ThirdIndexNameSpeed
END


***/

Return @IndexName
END;


-- SELECT [dbo].[IndexName](1, 1)
-- SELECT [dbo].[IndexName](1, 2)

-- SELECT [dbo].[IndexName] (2, 1)
-- SELECT [dbo].[IndexName] (2, 2)
-- SELECT [dbo].[IndexName] (2, 3)

/***
--Out of time to finish this function.  For now just a simple return.  Here is some ideas on how to continue! 

SELECT --count(*)
     TableName = t.name,
     IndexName = ind.name,
     IndexId = ind.index_id,
     ColumnId = ic.index_column_id,
     ColumnName = col.name,
     ind.*,
     ic.*,
     col.* 
FROM 
     sys.indexes ind 
INNER JOIN 
     sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 
INNER JOIN 
     sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id 
INNER JOIN 
     sys.tables t ON ind.object_id = t.object_id 
WHERE 
     ind.is_primary_key = 0 
     AND ind.is_unique = 0 
     AND ind.is_unique_constraint = 0 
     AND t.is_ms_shipped = 0 
--	 AND t.name = 'Speed_Events'
ORDER BY 
     t.name, ind.name, ind.index_id, ic.index_column_id;


SELECT t.name AS table_name,
SCHEMA_NAME(schema_id) AS schema_name,
c.name AS column_name
FROM sys.tables AS t
INNER JOIN sys.columns c ON t.OBJECT_ID = c.OBJECT_ID
--WHERE t.name = 'Speed_Events'
ORDER BY schema_name, table_name; 


SELECT c.name,
       c.max_length,
       c.precision,
       c.scale,
       c.is_nullable,
       t.name
  FROM sys.columns c
  JOIN sys.types t
    ON c.user_type_id = t.user_type_id
	--where c.name = 'Speed_events'



	SELECT   distinct
    c.name 'Column Name',
    t.Name 'Data type',
    c.max_length 'Max Length',
    c.precision ,
    c.scale ,
    c.is_nullable,
    ISNULL(i.is_primary_key, 0) 'Primary Key'
FROM    
    sys.columns c
INNER JOIN 
    sys.types t ON c.user_type_id = t.user_type_id
LEFT OUTER JOIN 
    sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
LEFT OUTER JOIN 
    sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
WHERE
    c.object_id = OBJECT_ID('Speed_Events')

***/
