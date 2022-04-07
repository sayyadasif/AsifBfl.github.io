CREATE TABLE [dbo].[IndentAudits] (
    [IndentAuditId]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [IndentId]           BIGINT        NOT NULL,
    [SchemeCodeId]       BIGINT        NOT NULL,
    [C5CodeId]           BIGINT        NOT NULL,
    [CardTypeId]         BIGINT        NOT NULL,
    [BfilBranchTypeId]   BIGINT        NOT NULL,
    [BfilRegionId]       BIGINT        NOT NULL,
    [BfilBranchId]       BIGINT        NOT NULL,
    [IblBranchId]        BIGINT        CONSTRAINT [DF_IndentAudits_IblBranchId] DEFAULT ((0)) NOT NULL,
    [DispatchAddressId]  BIGINT        NOT NULL,
    [IndentStatusId]     BIGINT        NOT NULL,
    [IndentNo]           VARCHAR (50)  NOT NULL,
    [IndentDate]         DATETIME      NOT NULL,
    [NoOfKit]            INT           NOT NULL,
    [ContactName]        VARCHAR (500) NOT NULL,
    [ContactNo]          VARCHAR (500) NOT NULL,
    [RejectionReasonId]  BIGINT        NULL,
    [HoApproveStatusId]  BIGINT        NULL,
    [HoApproveBy]        BIGINT        NULL,
    [HoApproveDate]      DATETIME      NULL,
    [CpuApproveStatusId] BIGINT        NULL,
    [CpuApproveBy]       BIGINT        NULL,
    [CpuApproveDate]     DATETIME      NULL,
    [Remarks]            VARCHAR (500) NULL,
    [IsActive]           BIT           CONSTRAINT [DF_IndentAudits_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]        DATETIME      CONSTRAINT [DF_IndentAudits_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [UpdatedDate]        DATETIME      CONSTRAINT [DF_IndentAudits_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]          BIGINT        NOT NULL,
    [IsDeleted]          BIT           CONSTRAINT [DF_IndentAudits_IsDeleted] DEFAULT ((0)) NOT NULL,
    [TimeStamp]          DATETIME      CONSTRAINT [DF_IndentAudits_TimeStamp] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_IndentAudits] PRIMARY KEY CLUSTERED ([IndentAuditId] ASC)
);











