USE [master]
GO
/****** Object:  Database [PolicyDb]    Script Date: 1/3/2022 1:50:10 PM ******/
CREATE DATABASE [PolicyDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PolicyDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER2012\MSSQL\DATA\PolicyDb.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PolicyDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER2012\MSSQL\DATA\PolicyDb_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [PolicyDb] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PolicyDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PolicyDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PolicyDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PolicyDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PolicyDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PolicyDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [PolicyDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PolicyDb] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [PolicyDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PolicyDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PolicyDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PolicyDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PolicyDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PolicyDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PolicyDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PolicyDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PolicyDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PolicyDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PolicyDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PolicyDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PolicyDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PolicyDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PolicyDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PolicyDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PolicyDb] SET RECOVERY FULL 
GO
ALTER DATABASE [PolicyDb] SET  MULTI_USER 
GO
ALTER DATABASE [PolicyDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PolicyDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PolicyDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PolicyDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PolicyDb', N'ON'
GO
USE [PolicyDb]
GO
/****** Object:  Table [dbo].[Policy]    Script Date: 1/3/2022 1:50:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Policy](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PolicyId] [varchar](50) NOT NULL,
	[PolicyName] [varchar](100) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[DurationInYear] [int] NOT NULL,
	[ComapnyName] [varchar](100) NOT NULL,
	[InitialDeposit] [money] NOT NULL,
	[PolicyTypeId] [varchar](10) NOT NULL,
	[TermsPerYear] [int] NOT NULL,
	[TermAmount] [money] NOT NULL,
	[Interest] [float] NOT NULL,
 CONSTRAINT [PK_Policy] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PolicyType]    Script Date: 1/3/2022 1:50:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PolicyType](
	[Id] [varchar](10) NOT NULL,
	[TypeName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PolicyType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PolicyUserType]    Script Date: 1/3/2022 1:50:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PolicyUserType](
	[PolicyId] [int] NOT NULL,
	[UserTypeId] [int] NOT NULL,
 CONSTRAINT [PK_PolicyUserType] PRIMARY KEY CLUSTERED 
(
	[PolicyId] ASC,
	[UserTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserType]    Script Date: 1/3/2022 1:50:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserType](
	[Id] [int] NOT NULL,
	[TypeName] [varchar](50) NOT NULL,
	[IncomePerYear] [varchar](50) NOT NULL,
 CONSTRAINT [PK_UserType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Policy] ON 

INSERT [dbo].[Policy] ([Id], [PolicyId], [PolicyName], [StartDate], [DurationInYear], [ComapnyName], [InitialDeposit], [PolicyTypeId], [TermsPerYear], [TermAmount], [Interest]) VALUES (17, N'CP-2021-001', N'Test Policy #1', CAST(0x0000AE0D00000000 AS DateTime), 3, N'Test Company 1', 100.0000, N'CP', 2, 1100.0000, 12)
INSERT [dbo].[Policy] ([Id], [PolicyId], [PolicyName], [StartDate], [DurationInYear], [ComapnyName], [InitialDeposit], [PolicyTypeId], [TermsPerYear], [TermAmount], [Interest]) VALUES (26, N'LI-2021-001', N'Test Policy #2', CAST(0x0000AE0D00000000 AS DateTime), 3, N'Test Company #2', 1200.0000, N'LI', 2, 1200.0000, 11)
INSERT [dbo].[Policy] ([Id], [PolicyId], [PolicyName], [StartDate], [DurationInYear], [ComapnyName], [InitialDeposit], [PolicyTypeId], [TermsPerYear], [TermAmount], [Interest]) VALUES (28, N'LI-2022-002', N'Test Policy #3', CAST(0x0000AE1100000000 AS DateTime), 3, N'Test Company #3', 1200.0000, N'LI', 6, 500.0000, 14)
SET IDENTITY_INSERT [dbo].[Policy] OFF
INSERT [dbo].[PolicyType] ([Id], [TypeName]) VALUES (N'CP', N'Child Plans')
INSERT [dbo].[PolicyType] ([Id], [TypeName]) VALUES (N'HI', N'Health Insurance')
INSERT [dbo].[PolicyType] ([Id], [TypeName]) VALUES (N'LI', N'Life Insurance')
INSERT [dbo].[PolicyType] ([Id], [TypeName]) VALUES (N'RP', N'Retirement Plans')
INSERT [dbo].[PolicyType] ([Id], [TypeName]) VALUES (N'TI', N'Travel Insurance')
INSERT [dbo].[PolicyType] ([Id], [TypeName]) VALUES (N'VI', N'Vehicle Insurance')
INSERT [dbo].[PolicyUserType] ([PolicyId], [UserTypeId]) VALUES (17, 1)
INSERT [dbo].[PolicyUserType] ([PolicyId], [UserTypeId]) VALUES (17, 2)
INSERT [dbo].[PolicyUserType] ([PolicyId], [UserTypeId]) VALUES (17, 3)
INSERT [dbo].[PolicyUserType] ([PolicyId], [UserTypeId]) VALUES (17, 5)
INSERT [dbo].[PolicyUserType] ([PolicyId], [UserTypeId]) VALUES (26, 1)
INSERT [dbo].[PolicyUserType] ([PolicyId], [UserTypeId]) VALUES (26, 5)
INSERT [dbo].[PolicyUserType] ([PolicyId], [UserTypeId]) VALUES (28, 1)
INSERT [dbo].[PolicyUserType] ([PolicyId], [UserTypeId]) VALUES (28, 2)
INSERT [dbo].[PolicyUserType] ([PolicyId], [UserTypeId]) VALUES (28, 5)
INSERT [dbo].[UserType] ([Id], [TypeName], [IncomePerYear]) VALUES (1, N'A', N'5')
INSERT [dbo].[UserType] ([Id], [TypeName], [IncomePerYear]) VALUES (2, N'B', N'10')
INSERT [dbo].[UserType] ([Id], [TypeName], [IncomePerYear]) VALUES (3, N'C', N'15')
INSERT [dbo].[UserType] ([Id], [TypeName], [IncomePerYear]) VALUES (4, N'D', N'30')
INSERT [dbo].[UserType] ([Id], [TypeName], [IncomePerYear]) VALUES (5, N'E', N'More than 30')
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Policy]    Script Date: 1/3/2022 1:50:11 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Policy] ON [dbo].[Policy]
(
	[PolicyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Policy_1]    Script Date: 1/3/2022 1:50:11 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Policy_1] ON [dbo].[Policy]
(
	[PolicyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Policy]  WITH CHECK ADD  CONSTRAINT [FK_Policy_PolicyType] FOREIGN KEY([PolicyTypeId])
REFERENCES [dbo].[PolicyType] ([Id])
GO
ALTER TABLE [dbo].[Policy] CHECK CONSTRAINT [FK_Policy_PolicyType]
GO
ALTER TABLE [dbo].[PolicyUserType]  WITH CHECK ADD  CONSTRAINT [FK_PolicyUserType_Policy] FOREIGN KEY([PolicyId])
REFERENCES [dbo].[Policy] ([Id])
GO
ALTER TABLE [dbo].[PolicyUserType] CHECK CONSTRAINT [FK_PolicyUserType_Policy]
GO
ALTER TABLE [dbo].[PolicyUserType]  WITH CHECK ADD  CONSTRAINT [FK_PolicyUserType_UserType] FOREIGN KEY([UserTypeId])
REFERENCES [dbo].[UserType] ([Id])
GO
ALTER TABLE [dbo].[PolicyUserType] CHECK CONSTRAINT [FK_PolicyUserType_UserType]
GO
USE [master]
GO
ALTER DATABASE [PolicyDb] SET  READ_WRITE 
GO
