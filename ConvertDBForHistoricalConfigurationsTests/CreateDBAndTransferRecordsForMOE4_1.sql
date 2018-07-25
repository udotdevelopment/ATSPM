USE [master]
GO

/****** Object:  Database [MOE_Orig]    Script Date: 2/21/2018 10:58:41 AM ******/


USE master
IF EXISTS(select * from sys.databases where name='MOE_Orig')
DROP DATABASE [MOE_Orig]
GO

CREATE DATABASE [MOE_Orig] ON  PRIMARY 
( NAME = N'MOE_Orig', FILENAME = N'D:\MSSQL\Data\MOE_Orig.mdf' , SIZE = 3072KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MOE_Orig_log', FILENAME = N'D:\MSSQL\Data\MOE_Orig_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MOE_Orig] SET COMPATIBILITY_LEVEL = 100
GO
ALTER DATABASE [MOE_Orig] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MOE_Orig] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MOE_Orig] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MOE_Orig] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MOE_Orig] SET ARITHABORT OFF 
GO
ALTER DATABASE [MOE_Orig] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MOE_Orig] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [MOE_Orig] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MOE_Orig] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MOE_Orig] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MOE_Orig] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MOE_Orig] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MOE_Orig] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MOE_Orig] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MOE_Orig] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MOE_Orig] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MOE_Orig] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MOE_Orig] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MOE_Orig] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MOE_Orig] SET  READ_WRITE 
GO
ALTER DATABASE [MOE_Orig] SET RECOVERY FULL 
GO
ALTER DATABASE [MOE_Orig] SET  MULTI_USER 
GO
ALTER DATABASE [MOE_Orig] SET PAGE_VERIFY CHECKSUM  
GO
USE [MOE_Orig]
GO
IF NOT EXISTS (SELECT name FROM sys.filegroups WHERE is_default=1 AND name = N'PRIMARY') ALTER DATABASE [MOE_Orig] MODIFY FILEGROUP [PRIMARY] DEFAULT
GO

/*create the tables*/

/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Accordian]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accordian](
	[AccHeader] [nvarchar](150) NULL,
	[AccContent] [nvarchar](max) NULL,
	[AccOrder] [int] NULL,
	[Application] [nvarchar](50) NULL,
	[AccID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Accordian] PRIMARY KEY CLUSTERED 
(
	[AccID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ActionLogActions]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActionLogActions](
	[ActionLog_ActionLogID] [int] NOT NULL,
	[Action_ActionID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ActionLogActions] PRIMARY KEY CLUSTERED 
(
	[ActionLog_ActionLogID] ASC,
	[Action_ActionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ActionLogMetricTypes]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActionLogMetricTypes](
	[ActionLog_ActionLogID] [int] NOT NULL,
	[MetricType_MetricID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ActionLogMetricTypes] PRIMARY KEY CLUSTERED 
(
	[ActionLog_ActionLogID] ASC,
	[MetricType_MetricID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ActionLogs]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActionLogs](
	[ActionLogID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[AgencyID] [int] NOT NULL,
	[Comment] [nvarchar](255) NULL,
	[SignalID] [nvarchar](10) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_dbo.ActionLogs] PRIMARY KEY CLUSTERED 
(
	[ActionLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Actions]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actions](
	[ActionID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_dbo.Actions] PRIMARY KEY CLUSTERED 
(
	[ActionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Agencies]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agencies](
	[AgencyID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.Agencies] PRIMARY KEY CLUSTERED 
(
	[AgencyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Alert_Day_Types]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Alert_Day_Types](
	[DayTypeNumber] [int] NOT NULL,
	[DayTypeDesctiption] [nvarchar](50) NULL,
 CONSTRAINT [PK_Alert_Day_Types] PRIMARY KEY CLUSTERED 
(
	[DayTypeNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationEvents]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationEvents](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[ApplicationName] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[SeverityLevel] [int] NOT NULL,
	[Class] [nvarchar](50) NULL,
	[Function] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.ApplicationEvents] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Applications]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Applications](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Applications] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationSettings]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationSettings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[ConsecutiveCount] [int] NULL,
	[MinPhaseTerminations] [int] NULL,
	[PercentThreshold] [float] NULL,
	[MaxDegreeOfParallelism] [int] NULL,
	[ScanDayStartHour] [int] NULL,
	[ScanDayEndHour] [int] NULL,
	[PreviousDayPMPeakStart] [int] NULL,
	[PreviousDayPMPeakEnd] [int] NULL,
	[MinimumRecords] [int] NULL,
	[WeekdayOnly] [bit] NULL,
	[DefaultEmailAddress] [nvarchar](max) NULL,
	[FromEmailAddress] [nvarchar](max) NULL,
	[LowHitThreshold] [int] NULL,
	[EmailServer] [nvarchar](max) NULL,
	[MaximumPedestrianEvents] [int] NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationSettings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Approaches]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Approaches](
	[ApproachID] [int] IDENTITY(1,1) NOT NULL,
	[SignalID] [nvarchar](10) NOT NULL,
	[DirectionTypeID] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[MPH] [int] NULL,
	[ProtectedPhaseNumber] [int] NOT NULL,
	[IsProtectedPhaseOverlap] [bit] NOT NULL,
	[PermissivePhaseNumber] [int] NULL,
 CONSTRAINT [PK_dbo.Approaches] PRIMARY KEY CLUSTERED 
(
	[ApproachID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApproachRoute]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApproachRoute](
	[ApproachRouteId] [int] IDENTITY(1,1) NOT NULL,
	[RouteName] [varchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.ApproachRoute] PRIMARY KEY CLUSTERED 
(
	[ApproachRouteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ApproachRouteDetail]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApproachRouteDetail](
	[ApproachRouteId] [int] NOT NULL,
	[ApproachOrder] [int] NOT NULL,
	[RouteDetailID] [int] IDENTITY(1,1) NOT NULL,
	[ApproachID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ApproachRouteDetail] PRIMARY KEY CLUSTERED 
(
	[RouteDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Archived_Metrics]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Archived_Metrics](
	[Timestamp] [datetime] NOT NULL,
	[DetectorID] [varchar](50) NOT NULL,
	[Volume] [int] NULL,
	[speed] [int] NULL,
	[delay] [int] NULL,
	[AoR] [int] NULL,
	[BinSize] [int] NOT NULL,
	[SpeedHits] [int] NULL,
	[BinGreenTime] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[ReceiveAlerts] [bit] NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Controller_Event_Log]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Controller_Event_Log](
	[SignalID] [nvarchar](10) NULL,
	[Timestamp] [datetime2](7) NULL,
	[EventCode] [int] NULL,
	[EventParam] [int] NULL
) 

GO
ALTER TABLE [dbo].[Controller_Event_Log] SET (LOCK_ESCALATION = AUTO)
GO
/****** Object:  Table [dbo].[ControllerTypes]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ControllerTypes](
	[ControllerTypeID] [int] NOT NULL,
	[Description] [varchar](50) NULL,
	[SNMPPort] [bigint] NOT NULL,
	[FTPDirectory] [varchar](max) NULL,
	[ActiveFTP] [bit] NOT NULL,
	[UserName] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
 CONSTRAINT [PK_dbo.ControllerTypes] PRIMARY KEY CLUSTERED 
(
	[ControllerTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DetectionHardwares]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetectionHardwares](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.DetectionHardwares] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DetectionTypeDetector]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetectionTypeDetector](
	[ID] [int] NOT NULL,
	[DetectionTypeID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.DetectionTypeDetector] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[DetectionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DetectionTypeMetricTypes]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetectionTypeMetricTypes](
	[DetectionType_DetectionTypeID] [int] NOT NULL,
	[MetricType_MetricID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.DetectionTypeMetricTypes] PRIMARY KEY CLUSTERED 
(
	[DetectionType_DetectionTypeID] ASC,
	[MetricType_MetricID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DetectionTypes]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetectionTypes](
	[DetectionTypeID] [int] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.DetectionTypes] PRIMARY KEY CLUSTERED 
(
	[DetectionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DetectorComments]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetectorComments](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[ID] [int] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[CommentText] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.DetectorComments] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Detectors]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Detectors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DetectorID] [nvarchar](50) NOT NULL,
	[DetChannel] [int] NOT NULL,
	[DistanceFromStopBar] [int] NULL,
	[MinSpeedFilter] [int] NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateDisabled] [datetime] NULL,
	[LaneNumber] [int] NULL,
	[MovementTypeID] [int] NULL,
	[LaneTypeID] [int] NULL,
	[DecisionPoint] [int] NULL,
	[MovementDelay] [int] NULL,
	[ApproachID] [int] NOT NULL,
	[DetectionHardwareID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Detectors] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DirectionTypes]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DirectionTypes](
	[DirectionTypeID] [int] NOT NULL,
	[Description] [nvarchar](30) NULL,
	[Abbreviation] [nvarchar](5) NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_dbo.DirectionTypes] PRIMARY KEY CLUSTERED 
(
	[DirectionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DownloadAgreements]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DownloadAgreements](
	[DownloadAgreementID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[EmailAddress] [nvarchar](max) NOT NULL,
	[Acknowledged] [bit] NOT NULL,
	[AgreementDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.DownloadAgreements] PRIMARY KEY CLUSTERED 
(
	[DownloadAgreementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExternalLinks]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExternalLinks](
	[ExternalLinkID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ExternalLinks] PRIMARY KEY CLUSTERED 
(
	[ExternalLinkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FAQs]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FAQs](
	[FAQID] [int] IDENTITY(1,1) NOT NULL,
	[Header] [nvarchar](max) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[OrderNumber] [int] NOT NULL,
 CONSTRAINT [PK_dbo.FAQs] PRIMARY KEY CLUSTERED 
(
	[FAQID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[holddups]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[holddups](
	[SignalID] [nvarchar](10) NULL,
	[Timestamp] [datetime] NULL,
	[EventCode] [int] NULL,
	[EventParam] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[holdkey]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[holdkey](
	[Timestamp] [datetime] NULL,
	[SignalID] [nvarchar](10) NULL,
	[EventCode] [int] NULL,
	[EventParam] [int] NULL,
	[DupeCount] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LaneTypes]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LaneTypes](
	[LaneTypeID] [int] NOT NULL,
	[Description] [nvarchar](30) NOT NULL,
	[Abbreviation] [nvarchar](5) NOT NULL,
 CONSTRAINT [PK_dbo.LaneTypes] PRIMARY KEY CLUSTERED 
(
	[LaneTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LastUpdates]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LastUpdates](
	[UpdateID] [int] IDENTITY(1,1) NOT NULL,
	[SignalID] [nvarchar](10) NOT NULL,
	[LastUpdateTime] [datetime] NULL,
	[LastErrorTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.LastUpdates] PRIMARY KEY CLUSTERED 
(
	[UpdateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LogTable]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LogTable](
	[TSQLStatement] [varchar](max) NULL,
	[FRAGMENTATION] [float] NULL,
	[Executed] [datetime] NULL,
	[ExecutedBy] [varchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[MenuId] [int] NOT NULL,
	[MenuName] [nvarchar](50) NOT NULL,
	[ParentId] [int] NOT NULL,
	[Application] [nvarchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[Controller] [nvarchar](50) NOT NULL,
	[Action] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_dbo.Menu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MetricCommentMetricTypes]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetricCommentMetricTypes](
	[MetricComment_CommentID] [int] NOT NULL,
	[MetricType_MetricID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.MetricCommentMetricTypes] PRIMARY KEY CLUSTERED 
(
	[MetricComment_CommentID] ASC,
	[MetricType_MetricID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MetricComments]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetricComments](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[SignalID] [nvarchar](10) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[CommentText] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.MetricComments] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MetricsFilterTypes]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetricsFilterTypes](
	[FilterID] [int] IDENTITY(1,1) NOT NULL,
	[FilterName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.MetricsFilterTypes] PRIMARY KEY CLUSTERED 
(
	[FilterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MetricTypes]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetricTypes](
	[MetricID] [int] IDENTITY(1,1) NOT NULL,
	[ChartName] [nvarchar](max) NOT NULL,
	[Abbreviation] [nvarchar](max) NOT NULL,
	[ShowOnWebsite] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.MetricTypes] PRIMARY KEY CLUSTERED 
(
	[MetricID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MovementTypes]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovementTypes](
	[MovementTypeID] [int] NOT NULL,
	[Description] [nvarchar](30) NOT NULL,
	[Abbreviation] [nvarchar](5) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_dbo.MovementTypes] PRIMARY KEY CLUSTERED 
(
	[MovementTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Program_Message]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Program_Message](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Priority] [nvarchar](10) NULL,
	[Program] [nvarchar](50) NULL,
	[Message] [nvarchar](500) NULL,
	[Timestamp] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Program_Settings]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Program_Settings](
	[SettingName] [nvarchar](50) NOT NULL,
	[SettingValue] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Region]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Region](
	[ID] [int] NOT NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Route]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Route](
	[RouteID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Region] [int] NOT NULL,
	[Name] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Route] PRIMARY KEY CLUSTERED 
(
	[RouteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Route_Detectors]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Route_Detectors](
	[DetectorID] [nvarchar](50) NOT NULL,
	[RouteID] [int] NOT NULL,
	[RouteOrder] [int] NOT NULL,
	[Detectors_ID] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Signals]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Signals](
	[SignalID] [nvarchar](10) NOT NULL,
	[Latitude] [varchar](30) NOT NULL,
	[Longitude] [varchar](30) NOT NULL,
	[PrimaryName] [varchar](100) NOT NULL,
	[SecondaryName] [varchar](100) NOT NULL,
	[IPAddress] [varchar](50) NOT NULL,
	[RegionID] [int] NOT NULL,
	[ControllerTypeID] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_Signals] PRIMARY KEY CLUSTERED 
(
	[SignalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SignalWithDetections]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SignalWithDetections](
	[SignalID] [nvarchar](10) NOT NULL,
	[DetectionTypeID] [int] NOT NULL,
	[PrimaryName] [nvarchar](max) NULL,
	[Secondary_Name] [nvarchar](max) NULL,
	[Latitude] [nvarchar](max) NULL,
	[Longitude] [nvarchar](max) NULL,
	[Region] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SignalWithDetections] PRIMARY KEY CLUSTERED 
(
	[SignalID] ASC,
	[DetectionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Speed_Events]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Speed_Events](
	[DetectorID] [nvarchar](50) NOT NULL,
	[MPH] [int] NOT NULL,
	[KPH] [int] NOT NULL,
	[Timestamp] [datetime2](7) NOT NULL
)

GO
/****** Object:  Table [dbo].[Speed_Events1]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Speed_Events1](
	[DetectorID] [nvarchar](50) NOT NULL,
	[MPH] [int] NOT NULL,
	[KPH] [int] NOT NULL,
	[timestamp] [datetime2](7) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SPMWatchDogErrorEvents]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SPMWatchDogErrorEvents](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[SignalID] [nvarchar](10) NOT NULL,
	[DetectorID] [nvarchar](max) NULL,
	[Direction] [nvarchar](max) NOT NULL,
	[Phase] [int] NOT NULL,
	[ErrorCode] [int] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.SPMWatchDogErrorEvents] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[staging_Controller_Event_Log_1-1-2014]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


GO
/****** Object:  Table [dbo].[staging_Controller_Event_Log_Part_24_MOE_201511]    Script Date: 2/21/2018 10:10:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


GO
ALTER TABLE [dbo].[ApproachRouteDetail] ADD  DEFAULT ((0)) FOR [ApproachID]
GO
ALTER TABLE [dbo].[Detectors] ADD  DEFAULT ((0)) FOR [DetectionHardwareID]
GO
ALTER TABLE [dbo].[DirectionTypes] ADD  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[DownloadAgreements] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [AgreementDate]
GO
ALTER TABLE [dbo].[FAQs] ADD  DEFAULT ((0)) FOR [OrderNumber]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ('') FOR [Controller]
GO
ALTER TABLE [dbo].[Menu] ADD  DEFAULT ('') FOR [Action]
GO
ALTER TABLE [dbo].[MovementTypes] ADD  DEFAULT ((0)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[Program_Message] ADD  CONSTRAINT [DF_Program_Message_Timestamp]  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Signals] ADD  DEFAULT ('') FOR [PrimaryName]
GO
ALTER TABLE [dbo].[Signals] ADD  DEFAULT ('') FOR [SecondaryName]
GO
ALTER TABLE [dbo].[Signals] ADD  DEFAULT ('') FOR [IPAddress]
GO
ALTER TABLE [dbo].[Signals] ADD  DEFAULT ((0)) FOR [RegionID]
GO
ALTER TABLE [dbo].[Signals] ADD  DEFAULT ((0)) FOR [ControllerTypeID]
GO
ALTER TABLE [dbo].[Signals] ADD  DEFAULT ((0)) FOR [Enabled]
GO
ALTER TABLE [dbo].[ActionLogActions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ActionLogActions_dbo.ActionLogs_ActionLog_ActionLogID] FOREIGN KEY([ActionLog_ActionLogID])
REFERENCES [dbo].[ActionLogs] ([ActionLogID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActionLogActions] CHECK CONSTRAINT [FK_dbo.ActionLogActions_dbo.ActionLogs_ActionLog_ActionLogID]
GO
ALTER TABLE [dbo].[ActionLogActions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ActionLogActions_dbo.Actions_Action_ActionID] FOREIGN KEY([Action_ActionID])
REFERENCES [dbo].[Actions] ([ActionID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActionLogActions] CHECK CONSTRAINT [FK_dbo.ActionLogActions_dbo.Actions_Action_ActionID]
GO
ALTER TABLE [dbo].[ActionLogMetricTypes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ActionLogMetricTypes_dbo.ActionLogs_ActionLog_ActionLogID] FOREIGN KEY([ActionLog_ActionLogID])
REFERENCES [dbo].[ActionLogs] ([ActionLogID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActionLogMetricTypes] CHECK CONSTRAINT [FK_dbo.ActionLogMetricTypes_dbo.ActionLogs_ActionLog_ActionLogID]
GO
ALTER TABLE [dbo].[ActionLogMetricTypes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ActionLogMetricTypes_dbo.MetricTypes_MetricType_MetricID] FOREIGN KEY([MetricType_MetricID])
REFERENCES [dbo].[MetricTypes] ([MetricID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActionLogMetricTypes] CHECK CONSTRAINT [FK_dbo.ActionLogMetricTypes_dbo.MetricTypes_MetricType_MetricID]
GO
ALTER TABLE [dbo].[ActionLogs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ActionLogs_dbo.Agencies_AgencyID] FOREIGN KEY([AgencyID])
REFERENCES [dbo].[Agencies] ([AgencyID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActionLogs] CHECK CONSTRAINT [FK_dbo.ActionLogs_dbo.Agencies_AgencyID]
GO
ALTER TABLE [dbo].[ActionLogs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ActionLogs_dbo.Signals_SignalID] FOREIGN KEY([SignalID])
REFERENCES [dbo].[Signals] ([SignalID])
GO
ALTER TABLE [dbo].[ActionLogs] CHECK CONSTRAINT [FK_dbo.ActionLogs_dbo.Signals_SignalID]
GO
ALTER TABLE [dbo].[ApplicationSettings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationSettings_dbo.Applications_ApplicationID] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Applications] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationSettings] CHECK CONSTRAINT [FK_dbo.ApplicationSettings_dbo.Applications_ApplicationID]
GO
ALTER TABLE [dbo].[Approaches]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Approaches_dbo.DirectionTypes_DirectionTypeID] FOREIGN KEY([DirectionTypeID])
REFERENCES [dbo].[DirectionTypes] ([DirectionTypeID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Approaches] CHECK CONSTRAINT [FK_dbo.Approaches_dbo.DirectionTypes_DirectionTypeID]
GO
ALTER TABLE [dbo].[Approaches]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Approaches_dbo.Signals_SignalID] FOREIGN KEY([SignalID])
REFERENCES [dbo].[Signals] ([SignalID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Approaches] CHECK CONSTRAINT [FK_dbo.Approaches_dbo.Signals_SignalID]
GO
ALTER TABLE [dbo].[ApproachRouteDetail]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApproachRouteDetail_dbo.Approaches_ApproachID] FOREIGN KEY([ApproachID])
REFERENCES [dbo].[Approaches] ([ApproachID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApproachRouteDetail] CHECK CONSTRAINT [FK_dbo.ApproachRouteDetail_dbo.Approaches_ApproachID]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[DetectionTypeDetector]  WITH CHECK ADD  CONSTRAINT [FK_dbo.DetectionTypeDetector_dbo.DetectionTypes_DetectionTypeID] FOREIGN KEY([DetectionTypeID])
REFERENCES [dbo].[DetectionTypes] ([DetectionTypeID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DetectionTypeDetector] CHECK CONSTRAINT [FK_dbo.DetectionTypeDetector_dbo.DetectionTypes_DetectionTypeID]
GO
ALTER TABLE [dbo].[DetectionTypeDetector]  WITH CHECK ADD  CONSTRAINT [FK_dbo.DetectionTypeDetector_dbo.Detectors_ID] FOREIGN KEY([ID])
REFERENCES [dbo].[Detectors] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DetectionTypeDetector] CHECK CONSTRAINT [FK_dbo.DetectionTypeDetector_dbo.Detectors_ID]
GO
ALTER TABLE [dbo].[DetectionTypeMetricTypes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.DetectionTypeMetricTypes_dbo.DetectionTypes_DetectionType_DetectionTypeID] FOREIGN KEY([DetectionType_DetectionTypeID])
REFERENCES [dbo].[DetectionTypes] ([DetectionTypeID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DetectionTypeMetricTypes] CHECK CONSTRAINT [FK_dbo.DetectionTypeMetricTypes_dbo.DetectionTypes_DetectionType_DetectionTypeID]
GO
ALTER TABLE [dbo].[DetectionTypeMetricTypes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.DetectionTypeMetricTypes_dbo.MetricTypes_MetricType_MetricID] FOREIGN KEY([MetricType_MetricID])
REFERENCES [dbo].[MetricTypes] ([MetricID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DetectionTypeMetricTypes] CHECK CONSTRAINT [FK_dbo.DetectionTypeMetricTypes_dbo.MetricTypes_MetricType_MetricID]
GO
ALTER TABLE [dbo].[DetectorComments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.DetectorComments_dbo.Detectors_ID] FOREIGN KEY([ID])
REFERENCES [dbo].[Detectors] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DetectorComments] CHECK CONSTRAINT [FK_dbo.DetectorComments_dbo.Detectors_ID]
GO
ALTER TABLE [dbo].[Detectors]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Detectors_dbo.Approaches_ApproachID] FOREIGN KEY([ApproachID])
REFERENCES [dbo].[Approaches] ([ApproachID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Detectors] CHECK CONSTRAINT [FK_dbo.Detectors_dbo.Approaches_ApproachID]
GO
ALTER TABLE [dbo].[Detectors]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Detectors_dbo.DetectionHardwares_DetectionHardwareID] FOREIGN KEY([DetectionHardwareID])
REFERENCES [dbo].[DetectionHardwares] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Detectors] CHECK CONSTRAINT [FK_dbo.Detectors_dbo.DetectionHardwares_DetectionHardwareID]
GO
ALTER TABLE [dbo].[Detectors]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Detectors_dbo.LaneTypes_LaneTypeID] FOREIGN KEY([LaneTypeID])
REFERENCES [dbo].[LaneTypes] ([LaneTypeID])
GO
ALTER TABLE [dbo].[Detectors] CHECK CONSTRAINT [FK_dbo.Detectors_dbo.LaneTypes_LaneTypeID]
GO
ALTER TABLE [dbo].[Detectors]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Detectors_dbo.MovementTypes_MovementTypeID] FOREIGN KEY([MovementTypeID])
REFERENCES [dbo].[MovementTypes] ([MovementTypeID])
GO
ALTER TABLE [dbo].[Detectors] CHECK CONSTRAINT [FK_dbo.Detectors_dbo.MovementTypes_MovementTypeID]
GO
ALTER TABLE [dbo].[MetricCommentMetricTypes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.MetricCommentMetricTypes_dbo.MetricComments_MetricComment_CommentID] FOREIGN KEY([MetricComment_CommentID])
REFERENCES [dbo].[MetricComments] ([CommentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MetricCommentMetricTypes] CHECK CONSTRAINT [FK_dbo.MetricCommentMetricTypes_dbo.MetricComments_MetricComment_CommentID]
GO
ALTER TABLE [dbo].[MetricCommentMetricTypes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.MetricCommentMetricTypes_dbo.MetricTypes_MetricType_MetricID] FOREIGN KEY([MetricType_MetricID])
REFERENCES [dbo].[MetricTypes] ([MetricID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MetricCommentMetricTypes] CHECK CONSTRAINT [FK_dbo.MetricCommentMetricTypes_dbo.MetricTypes_MetricType_MetricID]
GO
ALTER TABLE [dbo].[MetricComments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.MetricComments_dbo.Signals_SignalID] FOREIGN KEY([SignalID])
REFERENCES [dbo].[Signals] ([SignalID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MetricComments] CHECK CONSTRAINT [FK_dbo.MetricComments_dbo.Signals_SignalID]
GO
ALTER TABLE [dbo].[Route_Detectors]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Route_Detectors_dbo.Detectors_Detectors_ID] FOREIGN KEY([Detectors_ID])
REFERENCES [dbo].[Detectors] ([ID])
GO
ALTER TABLE [dbo].[Route_Detectors] CHECK CONSTRAINT [FK_dbo.Route_Detectors_dbo.Detectors_Detectors_ID]
GO
ALTER TABLE [dbo].[Signals]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Signals_dbo.ControllerTypes_ControllerTypeID] FOREIGN KEY([ControllerTypeID])
REFERENCES [dbo].[ControllerTypes] ([ControllerTypeID])
GO
ALTER TABLE [dbo].[Signals] CHECK CONSTRAINT [FK_dbo.Signals_dbo.ControllerTypes_ControllerTypeID]
GO
ALTER TABLE [dbo].[Signals]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Signals_dbo.Region_RegionID] FOREIGN KEY([RegionID])
REFERENCES [dbo].[Region] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Signals] CHECK CONSTRAINT [FK_dbo.Signals_dbo.Region_RegionID]
GO
ALTER TABLE [dbo].[SPMWatchDogErrorEvents]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SPMWatchDogErrorEvents_dbo.Signals_SignalID] FOREIGN KEY([SignalID])
REFERENCES [dbo].[Signals] ([SignalID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SPMWatchDogErrorEvents] CHECK CONSTRAINT [FK_dbo.SPMWatchDogErrorEvents_dbo.Signals_SignalID]
GO


/*Transfer records*/

USE [MOE_ORIG]


insert into __MigrationHistory select * from spmserver.MOE.dbo.__MigrationHistory

insert into ControllerTypes select* from spmserver.MOE.dbo.ControllerTypes

insert into DirectionTypes select* from spmserver.MOE.dbo.DirectionTypes

insert into DetectionHardwares select * from spmserver.MOE.dbo.DetectionHardwares

insert into DetectionTypes select * from spmserver.MOE.dbo.DetectionTypes

insert into LaneTypes select * from spmserver.MOE.dbo.LaneTypes

SET IDENTITY_INSERT dbo.MetricTypes ON

insert into MetricTypes([MetricID]
      ,[ChartName]
      ,[Abbreviation]
      ,[ShowOnWebsite])
	   select [MetricID]
      ,[ChartName]
      ,[Abbreviation]
      ,[ShowOnWebsite] from spmserver.MOE.dbo.MetricTypes

SET IDENTITY_INSERT dbo.MetricTypes OFF

insert into MovementTypes select * from spmserver.MOE.dbo.MovementTypes

insert into DetectionTypeMetricTypes select * from spmserver.MOE.dbo.DetectionTypeMetricTypes


insert into Region (id, Description) select ID, Description from spmserver.MOE.dbo.Region


Insert Into Signals(SignalID, PrimaryName, SecondaryName, IPAddress, ControllerTypeID, Enabled, Latitude, Longitude, RegionID)
Select SignalID, PrimaryName, SecondaryName, IPAddress, ControllerTypeID, Enabled, Latitude, Longitude, RegionID From [spmserver].[moe].dbo.Signals

Go

SET IDENTITY_INSERT dbo.Approaches ON
Go
Insert into Approaches(ApproachID,SignalId, DirectionTypeID, Description, MPH, ProtectedPhaseNumber, IsProtectedPhaseOverlap, PermissivePhaseNumber)
Select ApproachID,SignalId, DirectionTypeID, Description, MPH, ProtectedPhaseNumber, IsProtectedPhaseOverlap, PermissivePhaseNumber From [spmserver].[moe].dbo.Approaches


SET IDENTITY_INSERT dbo.Approaches OFF
Go


SET IDENTITY_INSERT dbo.Detectors ON
Go

Insert Into Detectors (ID, DetectorID, DetChannel, DistanceFromStopBar, MinSpeedFilter, DateAdded, DateDisabled, LaneNumber, MovementTypeID, LaneTypeID, DecisionPoint, MovementDelay, ApproachID, DetectionHardwareID)
Select * From [spmserver].[moe].dbo.Detectors

Go

SET IDENTITY_INSERT dbo.Detectors OFF
Go

insert into Accordian select AccHeader, AccContent, AccOrder, Application from spmserver.MOE.dbo.Accordian

SET IDENTITY_INSERT dbo.Agencies ON
insert into Agencies([AgencyID]
      ,[Description])
	   select [AgencyID]
      ,[Description] from spmserver.MOE.dbo.Agencies

SET IDENTITY_INSERT dbo.Agencies OFF

SET IDENTITY_INSERT ActionLogs ON 
insert into ActionLogs(ActionLogID, DATE, AgencyID, Comment, signalID, Name) select ActionLogID, DATE, AgencyID, Comment, signalID, Name from spmserver.MOE.dbo.ActionLogs
SET IDENTITY_INSERT ActionLogs OFF

insert into ActionLogMetricTypes select * from spmserver.MOE.dbo.ActionLogMetricTypes



insert into Alert_Day_Types select * from spmserver.MOE.dbo.Alert_Day_Types


SET IDENTITY_INSERT ApproachRoute ON 
insert into ApproachRoute(ApproachRouteId, RouteName) select ApproachRouteId, RouteName from spmserver.MOE.dbo.ApproachRoute
SET IDENTITY_INSERT ApproachRoute Off


SET IDENTITY_INSERT ApproachRouteDetail ON 
insert into ApproachRouteDetail([ApproachRouteId]
      ,[ApproachOrder]
      ,[RouteDetailID]
      ,[ApproachID]) 
	  select [ApproachRouteId]
      ,[ApproachOrder]
      ,[RouteDetailID]
      ,[ApproachID] from spmserver.MOE.dbo.ApproachRouteDetail
SET IDENTITY_INSERT ApproachRouteDetail OFF 



insert into Archived_Metrics select top 10000 * from spmserver.MOE.dbo.Archived_Metrics



insert into Controller_Event_Log select  top 10000 * from spmserver.MOE.dbo.Controller_Event_Log



insert into DetectorComments select ID,Timestamp, CommentText from spmserver.MOE.dbo.DetectorComments
insert into DetectionTypeDetector select ID,DetectionTypeID from spmserver.MOE.dbo.DetectionTypeDetector

insert into DownloadAgreements select CompanyName, Address, PhoneNumber, EmailAddress, Acknowledged, AgreementDate from spmserver.MOE.dbo.DownloadAgreements

insert into Menu(MenuId, MenuName, ParentId, Application, DisplayOrder) select MenuId, MenuName, ParentId, Application, DisplayOrder from spmserver.MOE.dbo.Menu

insert into Program_Message select top 10000 Priority, Program, Message, Timestamp from spmserver.MOE.dbo.Program_Message


insert into Program_Settings select top 10000 * from spmserver.MOE.dbo.Program_Settings


SET IDENTITY_INSERT Route ON
insert into Route (RouteID, Description, Region, Name) select  RouteID, Description, Region, Name from spmserver.MOE.dbo.Route
SET IDENTITY_INSERT Route OFF


insert into Route_Detectors (DetectorID, RouteID, RouteOrder) select DetectorID, RouteID, RouteOrder from spmserver.MOE.dbo.Route_Detectors


insert into Speed_Events select  top 10000 * from spmserver.MOE.dbo.Speed_Events
