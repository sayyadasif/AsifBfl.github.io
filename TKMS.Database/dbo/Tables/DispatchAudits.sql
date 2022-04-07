CREATE TABLE [dbo].[DispatchAudits] (
    [DispatchAuditId]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [DispatchId]         BIGINT        NOT NULL,
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
    [IsActive]           BIT           CONSTRAINT [DF_DispatchAudits_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]        DATETIME      CONSTRAINT [DF_DispatchAudits_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [UpdatedDate]        DATETIME      CONSTRAINT [DF_DispatchAudits_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]          BIGINT        NOT NULL,
    [IsDeleted]          BIT           CONSTRAINT [DF_DispatchAudits_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DispatchAudits] PRIMARY KEY CLUSTERED ([DispatchAuditId] ASC)
);

