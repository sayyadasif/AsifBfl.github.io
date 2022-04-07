TRUNCATE TABLE [BranchTypes]

INSERT [dbo].[BranchTypes] ([BranchTypeId], [BranchTypeName], [BranchTypeKey], [SortOrder], [IsAllowIndent], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'HO', N'HO', 1, 0, 1, CAST(N'2021-11-27T09:12:09.237' AS DateTime), 1, CAST(N'2021-11-27T09:12:09.237' AS DateTime), 1, 0)
GO
INSERT [dbo].[BranchTypes] ([BranchTypeId], [BranchTypeName], [BranchTypeKey], [SortOrder], [IsAllowIndent], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'BFIL RO', N'RO', 2, 1, 1, CAST(N'2021-11-27T09:12:09.237' AS DateTime), 1, CAST(N'2021-11-27T09:12:09.237' AS DateTime), 1, 0)
GO
INSERT [dbo].[BranchTypes] ([BranchTypeId], [BranchTypeName], [BranchTypeKey], [SortOrder], [IsAllowIndent], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'BFIL Branch', N'Branch', 3, 1, 1, CAST(N'2021-11-27T09:12:18.843' AS DateTime), 1, CAST(N'2021-11-27T09:12:18.843' AS DateTime), 1, 0)
GO


TRUNCATE TABLE [C5Codes]

SET IDENTITY_INSERT [dbo].[C5Codes] ON 
GO
INSERT [dbo].[C5Codes] ([C5CodeId], [C5CodeName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'CBM', 1, 1, CAST(N'2021-11-29T11:13:49.030' AS DateTime), 1, CAST(N'2021-11-29T11:13:49.030' AS DateTime), 1, 0)
GO
INSERT [dbo].[C5Codes] ([C5CodeId], [C5CodeName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'CM', 2, 1, CAST(N'2021-11-29T11:13:57.007' AS DateTime), 1, CAST(N'2021-11-29T11:13:57.007' AS DateTime), 1, 0)
GO
INSERT [dbo].[C5Codes] ([C5CodeId], [C5CodeName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'BV', 3, 1, CAST(N'2021-11-29T11:14:04.183' AS DateTime), 1, CAST(N'2021-11-29T11:14:04.183' AS DateTime), 1, 0)
GO
SET IDENTITY_INSERT [dbo].[C5Codes] OFF
GO

TRUNCATE TABLE [CardTypes]

SET IDENTITY_INSERT [dbo].[CardTypes] ON 
GO
INSERT [dbo].[CardTypes] ([CardTypeId], [CardTypeName], [CardTypeKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'Titanium Debit Card', N'Titanium', 1, 1, CAST(N'2021-11-29T11:14:43.403' AS DateTime), 1, CAST(N'2021-11-29T11:14:43.403' AS DateTime), 1, 0)
GO
INSERT [dbo].[CardTypes] ([CardTypeId], [CardTypeName], [CardTypeKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'Rupay', N'Rupay', 2, 1, CAST(N'2021-11-29T11:14:55.610' AS DateTime), 1, CAST(N'2021-11-29T11:14:55.610' AS DateTime), 1, 0)
GO
INSERT [dbo].[CardTypes] ([CardTypeId], [CardTypeName], [CardTypeKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'Visa', N'Visa', 3, 1, CAST(N'2021-11-29T11:15:03.870' AS DateTime), 1, CAST(N'2021-11-29T11:15:03.870' AS DateTime), 1, 0)
GO
SET IDENTITY_INSERT [dbo].[CardTypes] OFF
GO

TRUNCATE TABLE [CourierStatuses]

SET IDENTITY_INSERT [dbo].[CourierStatuses] ON 
GO
INSERT [dbo].[CourierStatuses] ([CourierStatusId], [StatusName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'No records', 1, 1, CAST(N'2021-12-11T09:36:22.020' AS DateTime), 1, CAST(N'2021-12-11T09:36:22.020' AS DateTime), 1, 0)
GO
INSERT [dbo].[CourierStatuses] ([CourierStatusId], [StatusName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'RTO', 2, 1, CAST(N'2021-12-11T09:36:29.527' AS DateTime), 1, CAST(N'2021-12-11T09:36:29.527' AS DateTime), 1, 0)
GO
INSERT [dbo].[CourierStatuses] ([CourierStatusId], [StatusName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'SHIPMENT DELIVERED', 3, 1, CAST(N'2021-12-11T09:36:36.963' AS DateTime), 1, CAST(N'2021-12-11T09:36:36.963' AS DateTime), 1, 0)
GO
INSERT [dbo].[CourierStatuses] ([CourierStatusId], [StatusName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (4, N'Speed Post', 4, 1, CAST(N'2021-12-11T09:36:43.637' AS DateTime), 1, CAST(N'2021-12-11T09:36:43.637' AS DateTime), 1, 0)
GO
INSERT [dbo].[CourierStatuses] ([CourierStatusId], [StatusName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (5, N'C''NEE SHIFTED FROM THE GIVEN ADDRESS', 5, 1, CAST(N'2021-12-11T09:36:51.893' AS DateTime), 1, CAST(N'2021-12-11T09:36:51.893' AS DateTime), 1, 0)
GO
INSERT [dbo].[CourierStatuses] ([CourierStatusId], [StatusName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (6, N'INTRANSIT', 6, 1, CAST(N'2021-12-11T09:36:59.770' AS DateTime), 1, CAST(N'2021-12-11T09:36:59.770' AS DateTime), 1, 0)
GO
INSERT [dbo].[CourierStatuses] ([CourierStatusId], [StatusName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (7, N'CONSIGNEE''S ADDRESS UNLOCATABLE/LANDMARK NEEDED', 7, 1, CAST(N'2021-12-11T09:37:08.620' AS DateTime), 1, CAST(N'2021-12-11T09:37:08.620' AS DateTime), 1, 0)
GO
INSERT [dbo].[CourierStatuses] ([CourierStatusId], [StatusName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (8, N'NEW REORD ADDED', 8, 1, CAST(N'2021-12-11T09:37:17.250' AS DateTime), 1, CAST(N'2021-12-11T09:37:17.250' AS DateTime), 1, 0)
GO
SET IDENTITY_INSERT [dbo].[CourierStatuses] OFF
GO


TRUNCATE TABLE [DispatchStatuses]


INSERT [dbo].[DispatchStatuses] ([DispatchStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'Dispatched', N'Dispatched', 1, 1, CAST(N'2021-11-29T11:20:42.353' AS DateTime), 1, CAST(N'2021-11-29T11:20:42.353' AS DateTime), 1, 0)
GO
INSERT [dbo].[DispatchStatuses] ([DispatchStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'Received At Ro', N'ReceivedAtRo', 2, 1, CAST(N'2021-11-29T11:17:02.193' AS DateTime), 1, CAST(N'2021-11-29T11:17:02.193' AS DateTime), 1, 0)
GO
INSERT [dbo].[DispatchStatuses] ([DispatchStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'Dispatch To Branch', N'DispatchToBranch', 3, 1, CAST(N'2021-11-29T11:17:15.770' AS DateTime), 1, CAST(N'2021-11-29T11:17:15.770' AS DateTime), 1, 0)
GO
INSERT [dbo].[DispatchStatuses] ([DispatchStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (4, N'Received', N'Received', 4, 1, CAST(N'2021-11-29T11:20:50.710' AS DateTime), 1, CAST(N'2021-11-29T11:20:50.710' AS DateTime), 1, 0)
GO

TRUNCATE TABLE [IndentStatuses]

INSERT [dbo].[IndentStatuses] ([IndentStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'Pending Approval', N'PendingApproval', 1, 1, CAST(N'2021-11-29T11:15:40.210' AS DateTime), 1, CAST(N'2021-11-29T11:15:40.210' AS DateTime), 1, 0)
GO
INSERT [dbo].[IndentStatuses] ([IndentStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'Approved', N'Approved', 2, 1, CAST(N'2021-11-29T11:15:52.243' AS DateTime), 1, CAST(N'2021-11-29T11:15:52.243' AS DateTime), 1, 0)
GO
INSERT [dbo].[IndentStatuses] ([IndentStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'Rejected', N'Rejected', 3, 1, CAST(N'2021-11-29T11:16:02.097' AS DateTime), 1, CAST(N'2021-11-29T11:16:02.097' AS DateTime), 1, 0)
GO
INSERT [dbo].[IndentStatuses] ([IndentStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (4, N'Approved', N'IndentForDispatch', 4, 1, CAST(N'2021-11-29T11:16:21.497' AS DateTime), 1, CAST(N'2021-11-29T11:16:21.497' AS DateTime), 1, 0)
GO
INSERT [dbo].[IndentStatuses] ([IndentStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (5, N'Partial Dispatched', N'PartialDispatched', 5, 1, CAST(N'2021-11-29T11:16:38.133' AS DateTime), 1, CAST(N'2021-11-29T11:16:38.133' AS DateTime), 1, 0)
GO
INSERT [dbo].[IndentStatuses] ([IndentStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (6, N'Dispatched', N'Dispatched', 6, 1, CAST(N'2021-11-29T11:16:47.057' AS DateTime), 1, CAST(N'2021-11-29T11:16:47.057' AS DateTime), 1, 0)
GO
INSERT [dbo].[IndentStatuses] ([IndentStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (7, N'Indent Box Received', N'IndentBoxReceived', 7, 1, CAST(N'2021-12-07T17:52:03.367' AS DateTime), 1, CAST(N'2021-12-07T17:52:03.367' AS DateTime), 1, 0)
GO
INSERT [dbo].[IndentStatuses] ([IndentStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (8, N'Cancelled', N'Cancelled', 8, 1, CAST(N'2021-11-29T11:17:28.910' AS DateTime), 1, CAST(N'2021-11-29T11:17:28.910' AS DateTime), 1, 0)
GO


TRUNCATE TABLE [KitDamageReasons]


SET IDENTITY_INSERT [dbo].[KitDamageReasons] ON 
GO
INSERT [dbo].[KitDamageReasons] ([KitDamageReasonId], [Reason], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'Damaged', 1, 1, CAST(N'2021-12-17T10:29:04.210' AS DateTime), 1, CAST(N'2021-12-17T10:29:04.210' AS DateTime), 1, 0)
GO
INSERT [dbo].[KitDamageReasons] ([KitDamageReasonId], [Reason], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'Opened Kit', 2, 1, CAST(N'2021-12-17T10:29:13.570' AS DateTime), 1, CAST(N'2021-12-17T10:29:13.570' AS DateTime), 1, 0)
GO
INSERT [dbo].[KitDamageReasons] ([KitDamageReasonId], [Reason], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'Not Received', 3, 1, CAST(N'2021-12-17T10:29:24.187' AS DateTime), 1, CAST(N'2021-12-17T10:29:24.187' AS DateTime), 1, 0)
GO
INSERT [dbo].[KitDamageReasons] ([KitDamageReasonId], [Reason], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (4, N'Missing', 4, 1, CAST(N'2021-12-17T10:29:39.330' AS DateTime), 1, CAST(N'2021-12-17T10:29:39.330' AS DateTime), 1, 0)
GO
INSERT [dbo].[KitDamageReasons] ([KitDamageReasonId], [Reason], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (5, N'Staff Left', 5, 1, CAST(N'2021-12-17T10:29:39.330' AS DateTime), 1, CAST(N'2021-12-17T10:29:39.330' AS DateTime), 1, 0)
GO
SET IDENTITY_INSERT [dbo].[KitDamageReasons] OFF
GO


TRUNCATE TABLE [KitStatuses]

INSERT [dbo].[KitStatuses] ([KitStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'Dispatched', N'Dispatched', 1, 1, CAST(N'2021-11-29T11:18:22.837' AS DateTime), 1, CAST(N'2021-11-29T11:18:22.837' AS DateTime), 1, 0)
GO
INSERT [dbo].[KitStatuses] ([KitStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'Received', N'Received', 2, 1, CAST(N'2021-11-29T11:19:47.530' AS DateTime), 1, CAST(N'2021-11-29T11:19:47.530' AS DateTime), 1, 0)
GO
INSERT [dbo].[KitStatuses] ([KitStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'Allocated', N'Allocated', 3, 1, CAST(N'2021-11-29T11:19:18.820' AS DateTime), 1, CAST(N'2021-11-29T11:19:18.820' AS DateTime), 1, 0)
GO
INSERT [dbo].[KitStatuses] ([KitStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (4, N'Assigned', N'Assigned', 4, 1, CAST(N'2021-11-29T11:19:25.850' AS DateTime), 1, CAST(N'2021-11-29T11:19:25.850' AS DateTime), 1, 0)
GO
INSERT [dbo].[KitStatuses] ([KitStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (5, N'Damaged', N'Damaged', 5, 1, CAST(N'2021-11-29T11:19:05.527' AS DateTime), 1, CAST(N'2021-11-29T11:19:05.527' AS DateTime), 1, 0)
GO
INSERT [dbo].[KitStatuses] ([KitStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (6, N'Destruction', N'Destruction', 6, 1, CAST(N'2021-11-29T11:19:12.320' AS DateTime), 1, CAST(N'2021-11-29T11:19:12.320' AS DateTime), 1, 0)
GO
INSERT [dbo].[KitStatuses] ([KitStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (7, N'Transfer', N'Transfer', 7, 1, CAST(N'2021-11-29T11:19:12.320' AS DateTime), 1, CAST(N'2021-11-29T11:19:12.320' AS DateTime), 1, 0)
GO


TRUNCATE TABLE [NotificationTypes]
GO

INSERT [dbo].[NotificationTypes] ([NotificationTypeId], [NotificationTypeName], [NotificationTemplate], [UrlTemplate], [Description], [VisibleToRoles], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'Receipt Confirmation', N'Please confirm Indent Received for Indent No: <span class=''reminder-indent-no''>{0}</span>, Dispatched from RO On: <span class=''reminder-date''>{1}</span>, Reference No: <span class=''reminder-ref-no''>{2}</span>', N'/Indent/DispatchedDetail/{0}', N'5 days after RO dispatches to Branch and Branch not marking it as "Received"', N'6,7', 1, CAST(N'2022-01-12T17:02:35.287' AS DateTime), 1, CAST(N'2022-01-12T17:02:35.287' AS DateTime), 1, 0)
GO
INSERT [dbo].[NotificationTypes] ([NotificationTypeId], [NotificationTypeName], [NotificationTemplate], [UrlTemplate], [Description], [VisibleToRoles], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'Receipt Confirmation', N'No confirmation on Indent Received by Branch for Indent No:<span class=''reminder-indent-no''>{0}</span>, Dispatched from RO On: <span class=''reminder-date''>{1}</span>, Reference No: <span class=''reminder-ref-no''>{2}</span>', N'/Indent/DispatchedDetail/{0}', N'5 days after RO dispatches to Branch and Branch not marking it as "Received"', N'5', 1, CAST(N'2022-01-12T17:04:28.747' AS DateTime), 1, CAST(N'2022-01-12T17:04:28.747' AS DateTime), 1, 0)
GO
INSERT [dbo].[NotificationTypes] ([NotificationTypeId], [NotificationTypeName], [NotificationTemplate], [UrlTemplate], [Description], [VisibleToRoles], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'Scan Kit Confirmation', N'Please scan kits for Indent No:<span class=''reminder-indent-no''>{0}</span>, Received On: <span class=''reminder-date''>{1}</span>', N'/Kit/KitDetail/{0}', N'5 Days after Branch Marks a Dispatch as "Received" and the Kits under it are not scanned (Received/Damaged).', N'6,7', 1, CAST(N'2022-01-12T17:04:54.857' AS DateTime), 1, CAST(N'2022-01-12T17:04:54.857' AS DateTime), 1, 0)
GO
INSERT [dbo].[NotificationTypes] ([NotificationTypeId], [NotificationTypeName], [NotificationTemplate], [UrlTemplate], [Description], [VisibleToRoles], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (4, N'Kit Return After Allocation', N'Kit not Assigned for X Days for Account No:<span class=''reminder-account-no''>{0}</span>, Allocated On: <span class=''reminder-date''>{1}</span>', N'/Kit/KitAllocated/{0}', N'Notification to start from 30-5 Days, if Kit is in Allocated state and not yet Assigned.', N'6,7', 1, CAST(N'2022-01-12T17:06:21.830' AS DateTime), 1, CAST(N'2022-01-12T17:06:21.830' AS DateTime), 1, 0)
GO
INSERT [dbo].[NotificationTypes] ([NotificationTypeId], [NotificationTypeName], [NotificationTemplate], [UrlTemplate], [Description], [VisibleToRoles], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (5, N'Kit Return After Allocation', N'Kit not Assigned for X Days for Account No:<span class=''reminder-account-no''>{0}</span>, Allocated On: <span class=''reminder-date''>{1}</span>', N'/Kit/KitAllocated/{0}', N'Notification to start from 30-5 Days, if Kit is in Allocated state and not yet Assigned.', N'9', 1, CAST(N'2022-01-12T17:06:40.440' AS DateTime), 1, CAST(N'2022-01-12T17:06:40.440' AS DateTime), 1, 0)
GO


TRUNCATE TABLE [RejectionReasons]


SET IDENTITY_INSERT [dbo].[RejectionReasons] ON 
GO
INSERT [dbo].[RejectionReasons] ([RejectionReasonId], [Reason], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'Data insufficient', 1, 1, CAST(N'2021-11-29T11:11:36.143' AS DateTime), 1, CAST(N'2021-11-29T11:11:36.143' AS DateTime), 1, 0)
GO
INSERT [dbo].[RejectionReasons] ([RejectionReasonId], [Reason], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'Sufficient stock at branch', 2, 1, CAST(N'2021-11-29T11:11:46.237' AS DateTime), 1, CAST(N'2021-11-29T11:11:46.237' AS DateTime), 1, 0)
GO
INSERT [dbo].[RejectionReasons] ([RejectionReasonId], [Reason], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'High Quantity', 3, 1, CAST(N'2021-11-29T11:11:53.673' AS DateTime), 1, CAST(N'2021-11-29T11:11:53.673' AS DateTime), 1, 0)
GO
SET IDENTITY_INSERT [dbo].[RejectionReasons] OFF
GO

TRUNCATE TABLE [Roles]

INSERT [dbo].[Roles] ([RoleId], [RoleTypeId], [RoleName], [RoleKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, 1, N'HO Admin', 'HOAdmin', 1, 0, 1, CAST(N'2021-11-25T16:32:05.017' AS DateTime), 1, CAST(N'2021-11-25T16:32:05.017' AS DateTime), 1, 0)
GO									 														  
INSERT [dbo].[Roles] ([RoleId], [RoleTypeId], [RoleName], [RoleKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, 1, N'HO Indent Maker', 'HOIndentMaker', 2, 0, 1, CAST(N'2021-11-25T16:32:14.800' AS DateTime), 1, CAST(N'2021-11-25T16:32:14.800' AS DateTime), 1, 0)
GO									 														  
INSERT [dbo].[Roles] ([RoleId], [RoleTypeId], [RoleName], [RoleKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, 1, N'HO Approver', 'HOApprover', 3, 0, 1, CAST(N'2021-11-25T16:32:25.163' AS DateTime), 1, CAST(N'2021-11-25T16:32:25.163' AS DateTime), 1, 0)
GO									 														  
INSERT [dbo].[Roles] ([RoleId], [RoleTypeId], [RoleName], [RoleKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (4, 2, N'RO Indent Maker', 'ROIndentMaker', 4, 0, 1, CAST(N'2021-11-25T16:32:31.510' AS DateTime), 1, CAST(N'2021-11-25T16:32:31.510' AS DateTime), 1, 0)
GO									 														  
INSERT [dbo].[Roles] ([RoleId], [RoleTypeId], [RoleName], [RoleKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (5, 2, N'RO Kit Management', 'ROKitManagement', 5, 0, 1, CAST(N'2021-11-25T16:32:45.127' AS DateTime), 1, CAST(N'2021-11-25T16:32:45.127' AS DateTime), 1, 0)
GO								 															  
INSERT [dbo].[Roles] ([RoleId], [RoleTypeId], [RoleName], [RoleKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (6, 3, N'BM', 'BM', 6, 0, 1, CAST(N'2021-11-25T16:32:45.127' AS DateTime), 1, CAST(N'2021-11-25T16:32:45.127' AS DateTime), 1, 0)
GO								 															  
INSERT [dbo].[Roles] ([RoleId], [RoleTypeId], [RoleName], [RoleKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (7, 3, N'BCM', 'BCM', 7, 0, 1, CAST(N'2021-11-25T16:32:45.127' AS DateTime), 1, CAST(N'2021-11-25T16:32:45.127' AS DateTime), 1, 0)
GO								 															  
INSERT [dbo].[Roles] ([RoleId], [RoleTypeId], [RoleName], [RoleKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (8, 4, N'IBL CPU', 'IBLCPU', 8, 1, 1, CAST(N'2021-11-25T16:32:45.127' AS DateTime), 1, CAST(N'2021-11-25T16:32:45.127' AS DateTime), 1, 0)
GO								 															  
INSERT [dbo].[Roles] ([RoleId], [RoleTypeId], [RoleName], [RoleKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (9, 5, N'Staff', 'Staff', 9, 1, 1, CAST(N'2021-11-25T16:32:45.127' AS DateTime), 1, CAST(N'2021-11-25T16:32:45.127' AS DateTime), 1, 0)
GO																							  
INSERT [dbo].[Roles] ([RoleId], [RoleTypeId], [RoleName], [RoleKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (10, 6, N'System Admin', 'SystemAdmin', 10, 1, 1, CAST(N'2021-11-25T16:32:45.127' AS DateTime), 1, CAST(N'2021-11-25T16:32:45.127' AS DateTime), 1, 0)
GO


TRUNCATE TABLE [RoleTypes]

INSERT [dbo].[RoleTypes] ([RoleTypeId], [RoleTypeName], [RoleTypeKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'Head Office', N'HO', 1, 0, 1, CAST(N'2021-12-01T11:47:01.550' AS DateTime), 1, CAST(N'2021-12-01T11:47:01.550' AS DateTime), 1, 0)
GO
INSERT [dbo].[RoleTypes] ([RoleTypeId], [RoleTypeName], [RoleTypeKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'Region Office', N'RO', 2, 0, 1, CAST(N'2021-12-01T11:47:15.077' AS DateTime), 1, CAST(N'2021-12-01T11:47:15.077' AS DateTime), 1, 0)
GO
INSERT [dbo].[RoleTypes] ([RoleTypeId], [RoleTypeName], [RoleTypeKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'Branch', N'BRANCH', 3, 0, 1, CAST(N'2021-12-01T11:47:21.713' AS DateTime), 1, CAST(N'2021-12-01T11:47:21.713' AS DateTime), 1, 0)
GO
INSERT [dbo].[RoleTypes] ([RoleTypeId], [RoleTypeName], [RoleTypeKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (4, N'IBL CPU', N'CPU', 4, 1, 1, CAST(N'2021-12-01T11:47:29.930' AS DateTime), 1, CAST(N'2021-12-01T11:47:29.930' AS DateTime), 1, 0)
GO
INSERT [dbo].[RoleTypes] ([RoleTypeId], [RoleTypeName], [RoleTypeKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (5, N'Staff', N'STAFF', 5, 1, 1, CAST(N'2021-12-01T11:47:38.330' AS DateTime), 1, CAST(N'2021-12-01T11:47:38.330' AS DateTime), 1, 0)
GO
INSERT [dbo].[RoleTypes] ([RoleTypeId], [RoleTypeName], [RoleTypeKey], [SortOrder], [IsAllowUserCreate], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (6, N'System Admin', N'SA', 6, 1, 1, CAST(N'2021-12-01T11:47:38.330' AS DateTime), 1, CAST(N'2021-12-01T11:47:38.330' AS DateTime), 1, 0)
GO


TRUNCATE TABLE [SchemeCodes]

SET IDENTITY_INSERT [dbo].[SchemeCodes] ON 
GO
INSERT [dbo].[SchemeCodes] ([SchemeCodeId], [SchemeCodeName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'CABFT', 1, 1, CAST(N'2021-11-29T11:12:59.710' AS DateTime), 1, CAST(N'2021-11-29T11:12:59.710' AS DateTime), 1, 0)
GO
INSERT [dbo].[SchemeCodes] ([SchemeCodeId], [SchemeCodeName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'SBMIC', 2, 1, CAST(N'2021-11-29T11:13:14.007' AS DateTime), 1, CAST(N'2021-11-29T11:13:14.007' AS DateTime), 1, 0)
GO
INSERT [dbo].[SchemeCodes] ([SchemeCodeId], [SchemeCodeName], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'CABFI', 3, 1, CAST(N'2021-11-29T11:13:14.007' AS DateTime), 1, CAST(N'2021-11-29T11:13:14.007' AS DateTime), 1, 0)
GO
SET IDENTITY_INSERT [dbo].[SchemeCodes] OFF
GO

TRUNCATE TABLE [Settings]

SET IDENTITY_INSERT [dbo].[Settings] ON 
GO
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingKey], [SettingValue], [SettingDesc], [IsEditable], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'Branch Indents Per Day', N'BranchIndentsPerDay', N'1', N'On same day indenting more than 1 indent should not be allowed for one branch', 1, 1, CAST(N'2021-11-29T11:26:34.390' AS DateTime), 1, CAST(N'2021-11-29T11:26:34.390' AS DateTime), 1, 0)
GO
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingKey], [SettingValue], [SettingDesc], [IsEditable], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'Dispatch Confirmation', N'DispatchConfirmation', N'5', N'System to prompt for confirmation post 5 days of dispatch from respective RO', 1, 1, CAST(N'2021-11-29T11:26:48.353' AS DateTime), 1, CAST(N'2021-11-29T11:26:48.353' AS DateTime), 1, 0)
GO
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingKey], [SettingValue], [SettingDesc], [IsEditable], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'Scan Kit Confirmation ', N'ScanKitConfirmation', N'5', N'System should prompt the branch user to scan the individual kits from post 5 days of confirmation by branch every time the user logs into the portal', 1, 1, CAST(N'2021-11-29T11:26:59.447' AS DateTime), 1, CAST(N'2021-11-29T11:26:59.447' AS DateTime), 1, 0)
GO
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingKey], [SettingValue], [SettingDesc], [IsEditable], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (4, N'Kit Allocation Alert', N'KitAllocationAlert', N'25', N'Kit Alert can be shown from 25 days onwards', 1, 1, CAST(N'2021-11-29T11:30:09.830' AS DateTime), 1, CAST(N'2021-11-29T11:30:09.830' AS DateTime), 1, 0)
GO
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingKey], [SettingValue], [SettingDesc], [IsEditable], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (5, N'Kit Return After Allocation', N'KitRetunAfterAllocation', N'30', N'If the status of kit is “Allocated” for 30 days (configurable) and staff has not attempted account opening (status has not changed to assigned), system to prompt for returning the Kit to branch', 1, 1, CAST(N'2021-11-29T11:27:13.060' AS DateTime), 1, CAST(N'2021-11-29T11:27:13.060' AS DateTime), 1, 0)
GO
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingKey], [SettingValue], [SettingDesc], [IsEditable], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (6, N'Kit Allocation to Staff', N'KitAllocationStaff', N'25', N'At any time, maximum kit which can be allocated to a staff can be upto 25 ', 1, 1, CAST(N'2021-11-29T11:27:25.460' AS DateTime), 1, CAST(N'2021-11-29T11:27:25.460' AS DateTime), 1, 0)
GO
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingKey], [SettingValue], [SettingDesc], [IsEditable], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (7, N'Min Kit Allocation to Staff', N'MinKitAllocationStaff', N'10', N'Allocation should not be allowed if staff already has 10 allocated kits (configurable)', 1, 1, CAST(N'2021-11-29T11:27:41.420' AS DateTime), 1, CAST(N'2021-11-29T11:27:41.420' AS DateTime), 1, 0)
GO
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingKey], [SettingValue], [SettingDesc], [IsEditable], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (8, N'Indent Count', N'IndentCount', N'0', N'Manage Daily Indent Number based on it', 0, 1, CAST(N'2021-11-29T11:28:33.553' AS DateTime), 1, CAST(N'2021-11-29T11:28:33.553' AS DateTime), 1, 0)
GO
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingKey], [SettingValue], [SettingDesc], [IsEditable], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (9, N'Indent Date', N'IndentDate', N'', N'Last Indeneted date for Indent Number', 0, 1, CAST(N'2021-11-29T11:30:09.830' AS DateTime), 1, CAST(N'2021-11-29T11:30:09.830' AS DateTime), 1, 0)
GO
SET IDENTITY_INSERT [dbo].[Settings] OFF
GO

SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT INTO [dbo].[Users]
           ([UserId]
           ,[StaffId]
           ,[FullName]
           ,[BranchId]
           ,[RoleTypeId]
           ,[Email]
           ,[Password]
           ,[Salt]
           ,[MobileNo]
           ,[IsActive]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdatedDate]
           ,[UpdatedBy]
           ,[IsDeleted])
     VALUES
           (1
		   ,'Admin'
           ,'Administrator'
           ,0
           ,6
           ,'admin@test.com'
           ,'eYwgHqZ/DHGPfUMj8Swk/YSsiUPRGZq/WfSNv04VLfQ=' --Admin123
           ,'j0E1sK50Dw7aPQ=='
           ,'9999999999'
           ,1
           ,GETDATE()
           ,1
           ,GETDATE()
           ,1
           ,0)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO


SET IDENTITY_INSERT [dbo].[UserRoles] ON 
GO
INSERT INTO [dbo].[UserRoles]
           ([UserRoleId]
           ,[UserId]
           ,[RoleId])
     VALUES
           (1
		   ,1
           ,10)
GO
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO