CREATE TABLE [dbo].[Kits] (
    [KitId]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [IndentId]              BIGINT        NOT NULL,
    [DispatchId]            BIGINT        NOT NULL,
    [BranchId]              BIGINT        NOT NULL,
    [CifNo]                 VARCHAR (50)  NOT NULL,
    [AccountNo]             VARCHAR (50)  NOT NULL,
    [KitStatusId]           BIGINT        NOT NULL,
    [AllocatedToId]         BIGINT        NULL,
    [AllocatedDate]         DATETIME      NULL,
    [AllocatedById]         BIGINT        NULL,
    [KitDamageReasonId]     BIGINT        NULL,
    [DamageRemarks]         VARCHAR (500) NULL,
    [IsDestructionApproved] BIT           NULL,
    [DestructionById]       BIGINT        NULL,
    [DestructionRemarks]    VARCHAR (500) NULL,
    [AssignedDate]          DATETIME      NULL,
    [CustomerName]          VARCHAR (50)  NULL,
    [Remarks]               VARCHAR (500) NULL,
    [IsActive]              BIT           CONSTRAINT [DF_Kits_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]           DATETIME      CONSTRAINT [DF_Kits_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]             BIGINT        NOT NULL,
    [UpdatedDate]           DATETIME      CONSTRAINT [DF_Kits_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]             BIGINT        NOT NULL,
    [IsDeleted]             BIT           CONSTRAINT [DF_Kits_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Kits] PRIMARY KEY CLUSTERED ([KitId] ASC)
);






















GO

CREATE TRIGGER [dbo].[KitAuditTrigger] ON [dbo].[Kits]
AFTER INSERT, UPDATE
AS  
   INSERT INTO [dbo].[KitAudits]
           ([KitId]
		   ,[IndentId]
		   ,[DispatchId]
		   ,[BranchId]
		   ,[CifNo]
		   ,[AccountNo]
		   ,[KitStatusId]
		   ,[AllocatedToId]
		   ,[AllocatedDate]
		   ,[AllocatedById]
		   ,[KitDamageReasonId]
		   ,[DamageRemarks]
		   ,[IsDestructionApproved]
           ,[DestructionById]
           ,[DestructionRemarks]
		   ,[AssignedDate]
		   ,[CustomerName]
		   ,[Remarks]
		   ,[IsActive]
		   ,[CreatedDate]
		   ,[CreatedBy]
		   ,[UpdatedDate]
		   ,[UpdatedBy]
		   ,[IsDeleted]) 
	SELECT  [KitId]
		   ,[IndentId]
		   ,[DispatchId]
		   ,[BranchId]
		   ,[CifNo]
		   ,[AccountNo]
		   ,[KitStatusId]
		   ,[AllocatedToId]
		   ,[AllocatedDate]
		   ,[AllocatedById]
		   ,[KitDamageReasonId]
		   ,[DamageRemarks]
		   ,[IsDestructionApproved]
           ,[DestructionById]
           ,[DestructionRemarks]
		   ,[AssignedDate]
		   ,[CustomerName]
		   ,[Remarks]
		   ,[IsActive]
		   ,[CreatedDate]
		   ,[CreatedBy]
		   ,[UpdatedDate]
		   ,[UpdatedBy]
		   ,[IsDeleted]
	FROM INSERTED