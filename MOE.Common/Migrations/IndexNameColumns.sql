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

/***
 SELECT [dbo].[IndexNameColumns](1, 1)
 SELECT [dbo].[IndexNameColumns](1, 2)

 SELECT [dbo].[IndexNameColumns] (2, 1)
 SELECT [dbo].[IndexNameColumns] (2, 2)
 SELECT [dbo].[IndexNameColumns] (2, 3)
 ***/

/***
--Out of time to finish this function.  For now just a simple table lookup and return.  Here is some ideas on how to continue! 

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
