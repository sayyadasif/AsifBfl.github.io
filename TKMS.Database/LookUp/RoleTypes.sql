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

