TRUNCATE TABLE [DispatchStatuses]


INSERT [dbo].[DispatchStatuses] ([DispatchStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, N'Dispatched', N'Dispatched', 1, 1, CAST(N'2021-11-29T11:20:42.353' AS DateTime), 1, CAST(N'2021-11-29T11:20:42.353' AS DateTime), 1, 0)
GO
INSERT [dbo].[DispatchStatuses] ([DispatchStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (2, N'Received At Ro', N'ReceivedAtRo', 2, 1, CAST(N'2021-11-29T11:17:02.193' AS DateTime), 1, CAST(N'2021-11-29T11:17:02.193' AS DateTime), 1, 0)
GO
INSERT [dbo].[DispatchStatuses] ([DispatchStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (3, N'Dispatch To Branch', N'DispatchToBranch', 3, 1, CAST(N'2021-11-29T11:17:15.770' AS DateTime), 1, CAST(N'2021-11-29T11:17:15.770' AS DateTime), 1, 0)
GO
INSERT [dbo].[DispatchStatuses] ([DispatchStatusId], [StatusName], [StatusKey], [SortOrder], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (4, N'Received', N'Received', 4, 1, CAST(N'2021-11-29T11:20:50.710' AS DateTime), 1, CAST(N'2021-11-29T11:20:50.710' AS DateTime), 1, 0)
GO