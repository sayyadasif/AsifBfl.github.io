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