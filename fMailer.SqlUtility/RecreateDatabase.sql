USE [master]
GO

Print('Starting database recreating...');

BEGIN TRY
    USE [master]    
    IF EXISTS (SELECT name FROM sys.databases WHERE name = N'fMailer')
    BEGIN
        ALTER DATABASE [fMailer] SET SINGLE_USER WITH ROLLBACK IMMEDIATE 
        DROP DATABASE [fMailer]
        Print('Existed DB was successfully dropped.');
    END
    ELSE
    BEGIN
        Print('DB does not exist. Drop skipped.');
    END
END TRY
BEGIN CATCH
    Print('Can not drop DB, maybe it does not exist. Drop skipped.');
END CATCH
GO

BEGIN TRY
    Print('Creating new empty DB...');
    BEGIN TRY
        CREATE DATABASE [fMailer] ON  PRIMARY 
        ( NAME = N'fMailer', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\fMailer.mdf' , SIZE = 4048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
         LOG ON 
        ( NAME = N'fMailer_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\fMailer_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
    END TRY
    BEGIN CATCH
        CREATE DATABASE [fMailer] ON  PRIMARY 
        ( NAME = N'fMailer', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\fMailer.mdf' , SIZE = 4048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
         LOG ON 
        ( NAME = N'fMailer_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\fMailer_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
    END CATCH

    ALTER DATABASE [fMailer] SET COMPATIBILITY_LEVEL = 100

    IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
    BEGIN
        EXEC [fMailer].[dbo].[sp_fulltext_database] @action = 'enable'
    END

    ALTER DATABASE [fMailer] SET ANSI_NULL_DEFAULT OFF 
    ALTER DATABASE [fMailer] SET ANSI_NULLS OFF 
    ALTER DATABASE [fMailer] SET ANSI_PADDING OFF 
    ALTER DATABASE [fMailer] SET ANSI_WARNINGS OFF 
    ALTER DATABASE [fMailer] SET ARITHABORT OFF 
    ALTER DATABASE [fMailer] SET AUTO_CLOSE OFF 
    ALTER DATABASE [fMailer] SET AUTO_CREATE_STATISTICS ON 
    ALTER DATABASE [fMailer] SET AUTO_SHRINK OFF 
    ALTER DATABASE [fMailer] SET AUTO_UPDATE_STATISTICS ON 
    ALTER DATABASE [fMailer] SET CURSOR_CLOSE_ON_COMMIT OFF 
    ALTER DATABASE [fMailer] SET CURSOR_DEFAULT  GLOBAL 
    ALTER DATABASE [fMailer] SET CONCAT_NULL_YIELDS_NULL OFF 
    ALTER DATABASE [fMailer] SET NUMERIC_ROUNDABORT OFF 
    ALTER DATABASE [fMailer] SET QUOTED_IDENTIFIER OFF 
    ALTER DATABASE [fMailer] SET RECURSIVE_TRIGGERS OFF 
    ALTER DATABASE [fMailer] SET DISABLE_BROKER 
    ALTER DATABASE [fMailer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
    ALTER DATABASE [fMailer] SET DATE_CORRELATION_OPTIMIZATION OFF 
    ALTER DATABASE [fMailer] SET TRUSTWORTHY OFF 
    ALTER DATABASE [fMailer] SET ALLOW_SNAPSHOT_ISOLATION OFF 
    ALTER DATABASE [fMailer] SET PARAMETERIZATION SIMPLE 
    ALTER DATABASE [fMailer] SET READ_COMMITTED_SNAPSHOT OFF 
    ALTER DATABASE [fMailer] SET HONOR_BROKER_PRIORITY OFF 
    ALTER DATABASE [fMailer] SET READ_WRITE 
    ALTER DATABASE [fMailer] SET RECOVERY SIMPLE 
    ALTER DATABASE [fMailer] SET MULTI_USER 
    ALTER DATABASE [fMailer] SET PAGE_VERIFY CHECKSUM  
    ALTER DATABASE [fMailer] SET DB_CHAINING OFF
    
    Print('DB was successfully recreated!');
END TRY
BEGIN CATCH
    Print('Error on creating DB. DB can not be created.');
END CATCH
GO