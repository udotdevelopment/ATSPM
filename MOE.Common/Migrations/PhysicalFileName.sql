--  DROP FUNCTION [dbo].[PhysicalFileName]

CREATE FUNCTION [dbo].[PhysicalFileName](@TableName varchar(40), @FirstIndexName varchar(100), @PartitionNumber int)
Returns varchar(40)
With EXECUTE as Caller
AS
BEGIN
		DECLARE @PhysicalFileName VARCHAR(40);
		SELECT 
		@PhysicalFileName = dbf.physical_name 

		FROM sys.partitions                  AS p
		JOIN sys.indexes                     AS i
			ON i.object_id = p.object_id
			AND i.index_id = p.index_id
		JOIN sys.data_spaces                 AS ds
			ON ds.data_space_id = i.data_space_id
		JOIN sys.partition_schemes           AS ps
			ON ps.data_space_id = ds.data_space_id
		JOIN sys.destination_data_spaces     AS dds2
			ON dds2.partition_scheme_id = ps.data_space_id 
			AND dds2.destination_id = p.partition_number
		JOIN sys.filegroups                  AS fg
			ON fg.data_space_id = dds2.data_space_id
		LEFT JOIN sys.partition_range_values AS prv_left
			ON ps.function_id = prv_left.function_id
			AND prv_left.boundary_id = p.partition_number - 1
		LEFT JOIN sys.partition_range_values AS prv_right
			ON ps.function_id = prv_right.function_id
			AND prv_right.boundary_id = p.partition_number
		JOIN sys.database_files AS dbf
			ON dbf.data_space_id = fg.data_space_id 
		Where OBJECT_NAME(p.object_id) = @TableName 
			AND i.name = @FirstIndexName 
			AND p.partition_number = @PartitionNumber;

		RETURN  @PhysicalFileName;
END;


--SELECT   dbo.[PhysicalFileName] ('Controller_event_log', 'IX_Clustered_Controller_Event_Log_Temp', 5)


GO


