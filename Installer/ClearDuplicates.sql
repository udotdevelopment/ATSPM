-- Inline SQL script
WITH cte1 AS (
    SELECT 
        [ApproachId], 
        [BinStartTime],
        ROW_NUMBER() OVER (
            PARTITION BY 
        [ApproachId], 
        [BinStartTime]
            ORDER BY 
        [ApproachId], 
        [BinStartTime]
        ) row_num
     FROM 
        [ApproachSpeedAggregations]
)
DELETE FROM cte1
WHERE row_num > 1;
go
WITH cte2 AS (
    SELECT 
        [ApproachId], 
        [BinStartTime],
        ROW_NUMBER() OVER (
            PARTITION BY 
        [ApproachId], 
        [BinStartTime]
            ORDER BY 
        [ApproachId], 
        [BinStartTime]
        ) row_num
     FROM 
        [ApproachCycleAggregations]
)
DELETE FROM cte2
WHERE row_num > 1;
go

WITH cte3 AS (
    SELECT 
        [ApproachId], 
        [BinStartTime],
        ROW_NUMBER() OVER (
            PARTITION BY 
        [ApproachId], 
        [BinStartTime]
            ORDER BY 
        [ApproachId], 
        [BinStartTime]
        ) row_num
     FROM 
        [ApproachPcdAggregations]
)
DELETE FROM cte3
WHERE row_num > 1;
go

WITH cte4 AS (
    SELECT 
        [ApproachId], 
        [BinStartTime],
        ROW_NUMBER() OVER (
            PARTITION BY 
        [ApproachId], 
        [BinStartTime]
            ORDER BY 
        [ApproachId], 
        [BinStartTime]
        ) row_num
     FROM 
        [ApproachEventCountAggregations]
)
DELETE FROM cte4
WHERE row_num > 1;
go

WITH cte5 AS (
    SELECT 
        [ApproachId], 
        [BinStartTime],
        ROW_NUMBER() OVER (
            PARTITION BY 
        [ApproachId], 
        [BinStartTime]
            ORDER BY 
        [ApproachId], 
        [BinStartTime]
        ) row_num
     FROM 
        [ApproachSplitFailAggregations]
)
DELETE FROM cte5
WHERE row_num > 1;
go

WITH cte6 AS (
    SELECT 
        [ApproachId], 
        [BinStartTime],
        ROW_NUMBER() OVER (
            PARTITION BY 
        [ApproachId], 
        [BinStartTime]
            ORDER BY 
        [ApproachId], 
        [BinStartTime]
        ) row_num
     FROM 
        [ApproachYellowRedActivationAggregations]
)
DELETE FROM cte6
WHERE row_num > 1;
go

