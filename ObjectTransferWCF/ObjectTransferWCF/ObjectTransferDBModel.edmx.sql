
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 07/16/2014 11:20:43
-- Generated from EDMX file: D:\Own\GitHub\TransferAccObj\ObjectTransferWCF\ObjectTransferWCF\ObjectTransferDBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ObjectTransferDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[LogsEntity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LogsEntity];
GO
IF OBJECT_ID(N'[dbo].[TasksEntity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TasksEntity];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'LogsEntity'
CREATE TABLE [dbo].[LogsEntity] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] nvarchar(max)  NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [StackTrace] nvarchar(max)  NULL
);
GO

-- Creating table 'TasksEntity'
CREATE TABLE [dbo].[TasksEntity] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Task] nvarchar(max)  NOT NULL,
    [Parameters] nvarchar(max)  NULL,
    [Date] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [NeedRecall] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'LogsEntity'
ALTER TABLE [dbo].[LogsEntity]
ADD CONSTRAINT [PK_LogsEntity]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TasksEntity'
ALTER TABLE [dbo].[TasksEntity]
ADD CONSTRAINT [PK_TasksEntity]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------