USE [msdb]
GO

/****** Object:  Job [Reclaim Data Space]    Script Date: 8/29/2018 10:35:09 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [Database Maintenance]    Script Date: 8/29/2018 10:35:09 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'Database Maintenance' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'Database Maintenance'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'Reclaim Data Space', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'This Script Reclaims file space.  It keeps only the latest months of data.  For the older months as defined in the ApplicationSettings table, it creats a flat file of the month, then it only keeps the data for the signals, as listed in DatabaseArchiveExcludedSignals table.  it then drops the table, file and filegroup for this month from the database.', 
		@category_name=N'Database Maintenance', 
		@owner_login_name=N'SPM', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ReclaimFIleSpace]    Script Date: 8/29/2018 10:35:09 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ReclaimFIleSpace', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @Counter int = 1
DECLARE @EnableDatbaseArchive int 
DECLARE @StartTime datetime2(7) = getdate()
DECLARE @StopCounter int
DECLARE @MonthsToDrop int = 0
DEclare @NeedToCompress int = 0
DECLARE @NeedToDrop INT = 0
DECLARE @NeedToShrink int = 0
DECLARE @IndexHasBeenRemoved int = 0
DECLARE @Status int = 0
DECLARE @Verbose int 
DECLARE @CurentYear int
DECLARE @CurentMonth int
DECLARE @ProcessingYear int
DECLARE @ProcessingMonth Int
DECLARE @MinimumDate datetime2(7)
DECLARE @TimeFork int
Declare @Notes nvarchar(100) 

Select  @EnableDatbaseArchive = [EnableDatbaseArchive]
FROM [dbo].[ApplicationSettings]
Where ApplicationId = 3
IF (@EnableDatbaseArchive IS NULL)
	SET @EnableDatbaseArchive = 0
IF (@EnableDatbaseArchive = 1)
BEGIN
	SET @Verbose = 0
	SELECT @Verbose = Verbose 
	FROM [dbo].[ToBeProcessededTables] 
	WHERE TableId = 1
	
	Select @MinimumDate = Min (Timestamp)
	from [dbo].[Controller_Event_Log]
	SET @CurentYear  = year(@StartTime)
	SET @CurentMonth = month(@StartTime)

	SELECT @TimeFork = Count (*) 
	FROM [dbo].[TablePartitionProcesseds]
	WHERE SwapTableName like ''%Controller_event_log%''

	IF @TimeFork = 0 
	BEGIN
		SELECT @StartTime = Min ([Timestamp])
		FROM [dbo].[Controller_Event_Log] 
		SET @ProcessingYear = year (@StartTime )
		SET @ProcessingMonth = month (@StartTime )
	END

	IF @TimeFork > 0  
	BEGIN
		SELECT @ProcessingYear  = max (PartitionBeginYear) 
		FROM [dbo].[TablePartitionProcesseds]
		WHERE SwapTableName like ''%Controller_event_log%''
				
		SELECT @ProcessingMonth  =  MAX (PartitionBeginMonth )
		FROM [dbo].[TablePartitionProcesseds]
		WHERE SwapTableName like ''%Controller_event_log%''
		AND PartitionBeginYear = @ProcessingYear   
		SET @ProcessingMonth = @ProcessingMonth + 1
	END
	IF (@ProcessingMonth  > 12)
	Begin 
		SET @ProcessingMonth = 1
		SET @ProcessingYear = @ProcessingYear  + 1
	END
	SET @StopCounter = [dbo].[StopCounter] (@CurentYear , @CurentMonth ,@Verbose )
	IF (@Verbose <>0)
	BEGIN
		SET @Notes = N''Step 1 The begining of the story!  Counter is '' + CONVERT(nvarchar(4), @Counter) + ''.  Stop Counter is '' + Convert(nvarchar(4), @StopCounter);
		EXEC	@Status  = [VerboseStatus]
					@PartitionedTableName = N''All Tables'',
					@PartitionName = N''Start of Reclaim File Space First'',
					@PartitionYear = @ProcessingYear ,
					@PartitionMonth = @ProcessingMonth ,
					@SQLStatementOrMessage = N''Start of Reclaim File Space'',
					@FunctionOrProcedure = N''Main - Reclaim File Space'',
					@Notes = @Notes 
	END
	While (@Counter < @StopCounter )
	BEGIN
		EXEC	@Status = [dbo].[ProcessTables]
					@ProcessingYear = @ProcessingYear,
					@ProcessingMonth = @ProcessingMonth,
					@MainCounter = @Counter,
					@Verbose = @Verbose 
		IF (@Verbose <>0)
		BEGIN
			Set @Notes = N''Step 2  Shall this be done again?  Counter is '' + CONVERT(nvarchar(4), @Counter) + ''.  Stop Counter is '' + Convert(nvarchar(4), @StopCounter)
			EXEC	@Status  = [dbo].[VerboseStatus]
						@PartitionedTableName = N''All Tables'',
						@PartitionName = N''All Partition Names'',
						@PartitionYear = @Counter ,
						@PartitionMonth = @StopCounter  ,
						@SQLStatementOrMessage = N''After EXEC ProcessTables'',
						@FunctionOrProcedure = N''Main - Reclaim File Space Bottom of main Loop'',
						@Notes = @Notes 
		END
		SET @ProcessingMonth  = @ProcessingMonth  + 1
		IF (@ProcessingMonth  > 12)
		BEGIN
			SET @ProcessingMonth  = 1
			SET @ProcessingYear  = @ProcessingYear  + 1
		END
		SET @Counter = @Counter + 1
	END
END
IF (@EnableDatbaseArchive = 0)
BEGIN
	EXEC	@Status  = [dbo].[VerboseStatus]
						@PartitionedTableName = N''All Tables'',
						@PartitionName = N''All Partition Names'',
						@PartitionYear = 0 ,
						@PartitionMonth = 0  ,
						@SQLStatementOrMessage = N''Flag <EnableDatbaseArchive> NOT SET.  Nothing to DO. '',
						@FunctionOrProcedure = N''Main - Reclaim File Space Bottom of main Loop'',
						@Notes = N''Table <ApplicationSettings>''
END', 
		@database_name=N'MOE', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'ReclaimFileSpaceMonthlyOnFirstFriday', 
		@enabled=1, 
		@freq_type=32, 
		@freq_interval=6, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=1, 
		@freq_recurrence_factor=1, 
		@active_start_date=20180827, 
		@active_end_date=99991231, 
		@active_start_time=210000, 
		@active_end_time=235959, 
		@schedule_uid=N'5a814858-8513-4058-865c-b5cac34653fb'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO
