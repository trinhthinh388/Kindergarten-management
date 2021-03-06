USE [master]
GO
/****** Object:  Database [kindergarten]    Script Date: 03/01/2020 20:19:23 ******/
CREATE DATABASE [kindergarten]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'kindergarten', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\kindergarten.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'kindergarten_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\kindergarten_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [kindergarten] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [kindergarten].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [kindergarten] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [kindergarten] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [kindergarten] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [kindergarten] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [kindergarten] SET ARITHABORT OFF 
GO
ALTER DATABASE [kindergarten] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [kindergarten] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [kindergarten] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [kindergarten] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [kindergarten] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [kindergarten] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [kindergarten] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [kindergarten] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [kindergarten] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [kindergarten] SET  ENABLE_BROKER 
GO
ALTER DATABASE [kindergarten] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [kindergarten] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [kindergarten] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [kindergarten] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [kindergarten] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [kindergarten] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [kindergarten] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [kindergarten] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [kindergarten] SET  MULTI_USER 
GO
ALTER DATABASE [kindergarten] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [kindergarten] SET DB_CHAINING OFF 
GO
ALTER DATABASE [kindergarten] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [kindergarten] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [kindergarten] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [kindergarten] SET QUERY_STORE = OFF
GO
USE [kindergarten]
GO
/****** Object:  Table [dbo].[children]    Script Date: 03/01/2020 20:19:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[children](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](1000) NOT NULL,
	[nickname] [nvarchar](1000) NULL,
	[birthdate] [datetime] NOT NULL,
	[enrolldate] [datetime] NOT NULL,
	[sex] [bit] NOT NULL,
	[imageUrl] [nvarchar](1000) NULL,
	[id_condition] [int] NULL,
	[id_parent] [int] NOT NULL,
	[id_class] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[class]    Script Date: 03/01/2020 20:19:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[class](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_grade] [int] NOT NULL,
	[id_teacher] [int] NULL,
	[name] [nvarchar](1000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[condition]    Script Date: 03/01/2020 20:19:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[condition](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](1000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[grade]    Script Date: 03/01/2020 20:19:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[grade](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[parent]    Script Date: 03/01/2020 20:19:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[parent](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[FatherName] [nvarchar](1000) NOT NULL,
	[Mothername] [nvarchar](1000) NOT NULL,
	[address] [nvarchar](1000) NOT NULL,
	[phonenumber] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[regulation]    Script Date: 03/01/2020 20:19:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[regulation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[content] [nvarchar](1000) NOT NULL,
	[ValueInt] [int] NOT NULL,
	[ValueStr] [nvarchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[report]    Script Date: 03/01/2020 20:19:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[report](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[generateDate] [datetime] NOT NULL,
	[id_class] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[teacher]    Script Date: 03/01/2020 20:19:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[teacher](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](1000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 03/01/2020 20:19:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[position] [int] NOT NULL,
	[id_teacher] [int] NOT NULL,
	[username] [nvarchar](1000) NOT NULL,
	[password] [nvarchar](1000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[children]  WITH CHECK ADD FOREIGN KEY([id_class])
REFERENCES [dbo].[class] ([id])
GO
ALTER TABLE [dbo].[children]  WITH CHECK ADD FOREIGN KEY([id_condition])
REFERENCES [dbo].[condition] ([id])
GO
ALTER TABLE [dbo].[children]  WITH CHECK ADD FOREIGN KEY([id_parent])
REFERENCES [dbo].[parent] ([id])
GO
ALTER TABLE [dbo].[class]  WITH CHECK ADD FOREIGN KEY([id_grade])
REFERENCES [dbo].[grade] ([id])
GO
ALTER TABLE [dbo].[class]  WITH CHECK ADD FOREIGN KEY([id_teacher])
REFERENCES [dbo].[teacher] ([id])
GO
ALTER TABLE [dbo].[report]  WITH CHECK ADD FOREIGN KEY([id_class])
REFERENCES [dbo].[class] ([id])
GO
ALTER TABLE [dbo].[users]  WITH CHECK ADD FOREIGN KEY([id_teacher])
REFERENCES [dbo].[teacher] ([id])
GO
USE [master]
GO
ALTER DATABASE [kindergarten] SET  READ_WRITE 
GO
