CREATE DATABASE Calaculator;

CREATE TABLE [dbo].[Calculations] (
    [ID]       INT        IDENTITY (1, 1) NOT NULL,
    [operand1] FLOAT (53) NULL,
    [operator] NCHAR (10) NULL,
    [operand2] FLOAT (53) NULL,
    [result]   FLOAT (53) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);
