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
