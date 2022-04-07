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
