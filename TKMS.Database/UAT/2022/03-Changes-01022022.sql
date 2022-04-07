ALTER TABLE Branches
ADD  [SchemeCodeId] BIGINT       CONSTRAINT [DF_Branches_SchemeCodeId] DEFAULT ((0)) NOT NULL
GO

ALTER TABLE Branches
ADD  [C5CodeId]     BIGINT       CONSTRAINT [DF_Branches_C5CodeId] DEFAULT ((0)) NOT NULL
GO

ALTER TABLE Branches
ADD  [CardTypeId]   BIGINT       CONSTRAINT [DF_Branches_CardTypeId] DEFAULT ((0)) NOT NULL
GO

ALTER TABLE [C5Codes]
ADD  [CardTypeId]  BIGINT       CONSTRAINT [DF_C5Codes_CardTypeId] DEFAULT ((0)) NOT NULL
GO

ALTER TABLE [dbo].[CardTypes]
DROP COLUMN [CardTypeKey];
GO

CREATE TABLE [dbo].[SchemeC5Codes] (
    [SchemeC5CodeId] BIGINT IDENTITY (1, 1) NOT NULL,
    [SchemeCodeId]   BIGINT NOT NULL,
    [C5CodeId]       BIGINT NOT NULL,
    CONSTRAINT [PK_SchemeC5Codes] PRIMARY KEY CLUSTERED ([SchemeC5CodeId] ASC)
);

GO
ALTER TABLE [dbo].[SchemeC5Codes]
    ADD CONSTRAINT [DF_SchemeC5Codes_SchemeCodeId] DEFAULT ((0)) FOR [SchemeCodeId];
GO

SET IDENTITY_INSERT [dbo].[Settings] ON 
GO
INSERT [dbo].[Settings] ([SettingId], [SettingName], [SettingKey], [SettingValue], [SettingDesc], [IsEditable], [IsActive], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (10, N'Indent Branch Required', N'IndentBranchRequired', N'1', N'Indent Branch Required while creating Indent. Value should be 1 or 0. Eg Yes="1" / No="0"', 1, 1, CAST(N'2021-11-29T11:30:09.830' AS DateTime), 1, CAST(N'2021-11-29T11:30:09.830' AS DateTime), 1, 0)
GO
SET IDENTITY_INSERT [dbo].[Settings] OFF
GO