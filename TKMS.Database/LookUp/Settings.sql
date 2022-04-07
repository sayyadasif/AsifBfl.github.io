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
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingKey], [SettingValue], [SettingDesc], [IsEditable], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (10, N'Allow OTP Authentication', N'AllowOTPAuth', N'1', N'Enable/Disable OTP Authentication for User. Enter "1" for "Yes" and "0" for "No".', 1, 1, CAST(N'2021-11-29T11:30:09.830' AS DateTime), 1, CAST(N'2021-11-29T11:30:09.830' AS DateTime), 1, 0)
GO
SET IDENTITY_INSERT [dbo].[Settings] OFF
GO