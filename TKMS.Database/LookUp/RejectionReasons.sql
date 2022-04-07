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