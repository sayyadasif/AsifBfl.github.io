CREATE TABLE [dbo].[KitAudits] (
    [KitAuditId]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [KitId]                 BIGINT        NOT NULL,
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
    [IsActive]              BIT           CONSTRAINT [DF_KitAudits_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]           DATETIME      CONSTRAINT [DF_KitAudits_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]             BIGINT        NOT NULL,
    [UpdatedDate]           DATETIME      CONSTRAINT [DF_KitAudits_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]             BIGINT        NOT NULL,
    [IsDeleted]             BIT           CONSTRAINT [DF_KitAudits_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_KitAudits] PRIMARY KEY CLUSTERED ([KitAuditId] ASC)
);















