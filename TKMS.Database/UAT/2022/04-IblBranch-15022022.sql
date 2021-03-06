DROP TRIGGER [IndentAuditTrigger]
GO

ALTER TABLE [Branches]
ADD  [IblBranchId]  BIGINT       CONSTRAINT [DF_Branches_IBLBranchId] DEFAULT ((0)) NOT NULL
GO

ALTER TABLE [dbo].[Indents]
DROP COLUMN [BranchId];
GO

ALTER TABLE [Indents]
ADD  [IblBranchId]        BIGINT        CONSTRAINT [DF_Indents_IblBranchId] DEFAULT ((0)) NOT NULL
GO


ALTER TABLE [dbo].[IndentAudits]
DROP COLUMN [BranchId];
GO

ALTER TABLE [IndentAudits]
ADD  [IblBranchId]        BIGINT        CONSTRAINT [DF_IndentAudits_IblBranchId] DEFAULT ((0)) NOT NULL
GO

DELETE FROM [Settings] WHERE [SettingId] = 10
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
GO

CREATE TABLE [dbo].[IblBranches] (
    [IblBranchId]   BIGINT       IDENTITY (1, 1) NOT NULL,
    [IblBranchName] VARCHAR (50) NOT NULL,
    [IblBranchCode] VARCHAR (20) NOT NULL,
    [SortOrder]     INT          CONSTRAINT [DF_IblBranches_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsActive]      BIT          CONSTRAINT [DF_IblBranches_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]   DATETIME     CONSTRAINT [DF_IblBranches_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     BIGINT       NOT NULL,
    [UpdatedDate]   DATETIME     CONSTRAINT [DF_IblBranches_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]     BIGINT       NOT NULL,
    [IsDeleted]     BIT          CONSTRAINT [DF_IblBranches_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_IblBranches] PRIMARY KEY CLUSTERED ([IblBranchId] ASC)
);
GO

TRUNCATE TABLE [NotificationTypes]
GO

INSERT [dbo].[NotificationTypes] ([NotificationTypeId], [NotificationTypeName], [NotificationTemplate], [UrlTemplate], [Description], [VisibleToRoles], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'Receipt Confirmation', N'Please confirm Indent Received for Indent No: <span class=''reminder-indent-no''>{0}</span>, Dispatched from RO On: <span class=''reminder-date''>{1}</span>, Reference No: <span class=''reminder-ref-no''>{2}</span>', N'/Indent/DispatchedDetail/{0}', N'5 days after RO dispatches to Branch and Branch not marking it as "Received"', N'6,7', 1, CAST(N'2022-01-12T17:02:35.287' AS DateTime), 1, CAST(N'2022-01-12T17:02:35.287' AS DateTime), 1, 0)
GO
INSERT [dbo].[NotificationTypes] ([NotificationTypeId], [NotificationTypeName], [NotificationTemplate], [UrlTemplate], [Description], [VisibleToRoles], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'Receipt Confirmation', N'No confirmation on Indent Received by Branch for Indent No:<span class=''reminder-indent-no''>{0}</span>, Dispatched from RO On: <span class=''reminder-date''>{1}</span>, Reference No: <span class=''reminder-ref-no''>{2}</span>', N'/Indent/DispatchedDetail/{0}', N'5 days after RO dispatches to Branch and Branch not marking it as "Received"', N'5', 1, CAST(N'2022-01-12T17:04:28.747' AS DateTime), 1, CAST(N'2022-01-12T17:04:28.747' AS DateTime), 1, 0)
GO
INSERT [dbo].[NotificationTypes] ([NotificationTypeId], [NotificationTypeName], [NotificationTemplate], [UrlTemplate], [Description], [VisibleToRoles], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'Scan Kit Confirmation', N'Please scan kits for Indent No:<span class=''reminder-indent-no''>{0}</span>, Received On: <span class=''reminder-date''>{1}</span>', N'/Kit/KitDetail/{0}', N'5 Days after Branch Marks a Dispatch as "Received" and the Kits under it are not scanned (Received/Damaged).', N'6,7', 1, CAST(N'2022-01-12T17:04:54.857' AS DateTime), 1, CAST(N'2022-01-12T17:04:54.857' AS DateTime), 1, 0)
GO
INSERT [dbo].[NotificationTypes] ([NotificationTypeId], [NotificationTypeName], [NotificationTemplate], [UrlTemplate], [Description], [VisibleToRoles], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (4, N'Kit Return After Allocation', N'Kit not Assigned for {2} Days for Account No:<span class=''reminder-account-no''>{0}</span>, Allocated On: <span class=''reminder-date''>{1}</span>', N'/Kit/KitAllocated/{0}', N'Notification to start from 30-5 Days, if Kit is in Allocated state and not yet Assigned.', N'6,7', 1, CAST(N'2022-01-12T17:06:21.830' AS DateTime), 1, CAST(N'2022-01-12T17:06:21.830' AS DateTime), 1, 0)
GO
INSERT [dbo].[NotificationTypes] ([NotificationTypeId], [NotificationTypeName], [NotificationTemplate], [UrlTemplate], [Description], [VisibleToRoles], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (5, N'Kit Return After Allocation', N'Kit not Assigned for {2} Days for Account No:<span class=''reminder-account-no''>{0}</span>, Allocated On: <span class=''reminder-date''>{1}</span>', N'/Kit/KitAllocated/{0}', N'Notification to start from 30-5 Days, if Kit is in Allocated state and not yet Assigned.', N'9', 1, CAST(N'2022-01-12T17:06:40.440' AS DateTime), 1, CAST(N'2022-01-12T17:06:40.440' AS DateTime), 1, 0)
GO
