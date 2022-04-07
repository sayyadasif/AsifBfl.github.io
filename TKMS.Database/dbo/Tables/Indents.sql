CREATE TABLE [dbo].[Indents] (
    [IndentId]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [SchemeCodeId]       BIGINT        NOT NULL,
    [C5CodeId]           BIGINT        NOT NULL,
    [CardTypeId]         BIGINT        NOT NULL,
    [BfilBranchTypeId]   BIGINT        NOT NULL,
    [BfilRegionId]       BIGINT        NOT NULL,
    [BfilBranchId]       BIGINT        NOT NULL,
    [IblBranchId]        BIGINT        CONSTRAINT [DF_Indents_IblBranchId] DEFAULT ((0)) NOT NULL,
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
    [IsActive]           BIT           CONSTRAINT [DF_Indents_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]        DATETIME      CONSTRAINT [DF_Indents_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [UpdatedDate]        DATETIME      CONSTRAINT [DF_Indents_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]          BIGINT        NOT NULL,
    [IsDeleted]          BIT           CONSTRAINT [DF_Indents_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Indents] PRIMARY KEY CLUSTERED ([IndentId] ASC)
);














GO
CREATE   TRIGGER [dbo].[IndentAuditTrigger] ON [dbo].[Indents]
AFTER INSERT, UPDATE
AS  
   INSERT INTO [dbo].[IndentAudits]
           ([IndentId]
           ,[SchemeCodeId]
           ,[C5CodeId]
           ,[CardTypeId]
           ,[BfilBranchTypeId]
           ,[BfilRegionId]           
		   ,[BfilBranchId]
           ,[IblBranchId]
           ,[DispatchAddressId]
           ,[IndentStatusId]
           ,[IndentNo]
           ,[IndentDate]
           ,[NoOfKit]
           ,[ContactName]
           ,[ContactNo]
           ,[RejectionReasonId]
           ,[HoApproveStatusId]
           ,[HoApproveBy]
           ,[HoApproveDate]
           ,[CpuApproveStatusId]
           ,[CpuApproveBy]
           ,[CpuApproveDate]
           ,[Remarks]
           ,[IsActive]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdatedDate]
           ,[UpdatedBy]
           ,[IsDeleted]) 
	SELECT [IndentId]
           ,[SchemeCodeId]
           ,[C5CodeId]
           ,[CardTypeId]
           ,[BfilBranchTypeId]
           ,[BfilRegionId]           
		   ,[BfilBranchId]
           ,[IblBranchId]
           ,[DispatchAddressId]
           ,[IndentStatusId]
           ,[IndentNo]
           ,[IndentDate]
           ,[NoOfKit]
           ,[ContactName]
           ,[ContactNo]
           ,[RejectionReasonId]
           ,[HoApproveStatusId]
           ,[HoApproveBy]
           ,[HoApproveDate]
           ,[CpuApproveStatusId]
           ,[CpuApproveBy]
           ,[CpuApproveDate]
           ,[Remarks]
           ,[IsActive]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdatedDate]
           ,[UpdatedBy]
           ,[IsDeleted]
	FROM INSERTED