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