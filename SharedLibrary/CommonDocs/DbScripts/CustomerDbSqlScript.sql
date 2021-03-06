USE [master]
GO
/****** Object:  Database [CustomerDb]    Script Date: 12/19/2021 2:41:06 PM ******/
CREATE DATABASE [CustomerDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CustomerDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER2012\MSSQL\DATA\CustomerDb.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CustomerDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER2012\MSSQL\DATA\CustomerDb_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [CustomerDb] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CustomerDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CustomerDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CustomerDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CustomerDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CustomerDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CustomerDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [CustomerDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CustomerDb] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [CustomerDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CustomerDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CustomerDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CustomerDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CustomerDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CustomerDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CustomerDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CustomerDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CustomerDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CustomerDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CustomerDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CustomerDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CustomerDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CustomerDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CustomerDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CustomerDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CustomerDb] SET RECOVERY FULL 
GO
ALTER DATABASE [CustomerDb] SET  MULTI_USER 
GO
ALTER DATABASE [CustomerDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CustomerDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CustomerDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CustomerDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'CustomerDb', N'ON'
GO
USE [CustomerDb]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 12/19/2021 2:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Address] [varchar](300) NULL,
	[ContactNo] [varchar](25) NULL,
	[Email] [varchar](50) NOT NULL,
	[Salary] [decimal](18, 0) NOT NULL,
	[PanNo] [varchar](50) NULL,
	[EmployeeTypeId] [int] NOT NULL,
	[Employer] [varchar](100) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmployeeType]    Script Date: 12/19/2021 2:41:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmployeeType](
	[Id] [int] NOT NULL,
	[TypeName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_EmployeeType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

GO
INSERT [dbo].[Customer] ([Id], [FirstName], [LastName], [DateOfBirth], [Address], [ContactNo], [Email], [Salary], [PanNo], [EmployeeTypeId], [Employer]) VALUES (1, N'Mike', N'Smith', CAST(0x00007C0600000000 AS DateTime), N'123 Abc Road, Toronton, ON M2H2W9', N'804-504-3946', N'user@example.com', CAST(3565 AS Decimal(18, 0)), N'536474854', 1, N'Cognizant Technology Inc.')
GO
INSERT [dbo].[Customer] ([Id], [FirstName], [LastName], [DateOfBirth], [Address], [ContactNo], [Email], [Salary], [PanNo], [EmployeeTypeId], [Employer]) VALUES (2, N'Mike', N'Smith', CAST(0x00007C0600000000 AS DateTime), N'123 Abc Road, Toronton, ON M2H2W9', N'804-504-3946', N'user1@example.com', CAST(3565 AS Decimal(18, 0)), N'536474854', 1, N'Cognizant Technology Inc.')
GO
INSERT [dbo].[Customer] ([Id], [FirstName], [LastName], [DateOfBirth], [Address], [ContactNo], [Email], [Salary], [PanNo], [EmployeeTypeId], [Employer]) VALUES (3, N'Yuke', N'Guo', CAST(0x00008A2C00000000 AS DateTime), N'123 ABC Road, Toronto, ON M2H 2W9 Canada', N'(836)384-5635', N'yuke@test.com', CAST(100000 AS Decimal(18, 0)), N'2637383', 1, N'Google')
GO
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
INSERT [dbo].[EmployeeType] ([Id], [TypeName]) VALUES (1, N'salaried')
GO
INSERT [dbo].[EmployeeType] ([Id], [TypeName]) VALUES (2, N'self-employed')
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Customer]    Script Date: 12/19/2021 2:41:06 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Customer] ON [dbo].[Customer]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_EmployeeType] FOREIGN KEY([EmployeeTypeId])
REFERENCES [dbo].[EmployeeType] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_EmployeeType]
GO
USE [master]
GO
ALTER DATABASE [CustomerDb] SET  READ_WRITE 
GO
