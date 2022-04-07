CREATE TABLE [dbo].[Dispatches] (
    [DispatchId]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [IndentId]           BIGINT        NOT NULL,
    [Vendor]             VARCHAR (50)  NOT NULL,
    [DispatchQty]        INT           NOT NULL,
    [AccountStart]       VARCHAR (50)  NOT NULL,
    [AccountEnd]         VARCHAR (50)  NOT NULL,
    [SchemeType]         VARCHAR (50)  NOT NULL,
    [DispatchDate]       DATETIME      NOT NULL,
    [ReferenceNo]        VARCHAR (50)  NOT NULL,
    [DispatchStatusId]   BIGINT        NOT NULL,
    [BranchDispatchDate] DATETIME      NULL,
    [BranchReceiveDate]  DATETIME      NULL,
    [Remarks]            VARCHAR (500) NULL,
    [IsActive]           BIT           CONSTRAINT [DF_Dispatches_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]        DATETIME      CONSTRAINT [DF_Dispatches_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [UpdatedDate]        DATETIME      CONSTRAINT [DF_Dispatches_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]          BIGINT        NOT NULL,
    [IsDeleted]          BIT           CONSTRAINT [DF_Dispatches_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Dispatches] PRIMARY KEY CLUSTERED ([DispatchId] ASC)
);










GO

CREATE TRIGGER [dbo].[DispatchAuditTrigger] ON [dbo].[Dispatches]
AFTER INSERT, UPDATE
AS  
   INSERT INTO [dbo].[DispatchAudits]
           ([DispatchId]
		   ,[IndentId]
		   ,[Vendor]
		   ,[DispatchQty]
		   ,[AccountStart]
		   ,[AccountEnd]
		   ,[SchemeType]
		   ,[DispatchDate]
		   ,[ReferenceNo]
		   ,[DispatchStatusId]
		   ,[BranchDispatchDate]
		   ,[BranchReceiveDate]
		   ,[Remarks]
		   ,[IsActive]
		   ,[CreatedDate]
		   ,[CreatedBy]
		   ,[UpdatedDate]
		   ,[UpdatedBy]
		   ,[IsDeleted]) 
	SELECT  [DispatchId]
		   ,[IndentId]
		   ,[Vendor]
		   ,[DispatchQty]
		   ,[AccountStart]
		   ,[AccountEnd]
		   ,[SchemeType]
		   ,[DispatchDate]
		   ,[ReferenceNo]
		   ,[DispatchStatusId]
		   ,[BranchDispatchDate]
		   ,[BranchReceiveDate]
		   ,[Remarks]
		   ,[IsActive]
		   ,[CreatedDate]
		   ,[CreatedBy]
		   ,[UpdatedDate]
		   ,[UpdatedBy]
		   ,[IsDeleted]
	FROM INSERTED