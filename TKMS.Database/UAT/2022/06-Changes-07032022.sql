SET IDENTITY_INSERT [dbo].[Settings] ON 
GO
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingKey], [SettingValue], [SettingDesc], [IsEditable], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (10, N'Allow OTP Authentication', N'AllowOTPAuth', N'1', N'Enable/Disable OTP Authentication for User. Enter "1" for "Yes" and "0" for "No".', 1, 1, CAST(N'2021-11-29T11:30:09.830' AS DateTime), 1, CAST(N'2021-11-29T11:30:09.830' AS DateTime), 1, 0)
GO
SET IDENTITY_INSERT [dbo].[Settings] OFF
GO

ALTER TABLE [IWorksKits]
ALTER COLUMN [ResponseMessage] [varchar](max) NULL
GO