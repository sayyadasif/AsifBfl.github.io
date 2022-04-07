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