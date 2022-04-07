CREATE TABLE [dbo].[Settings] (
    [SettingId]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [SettingName]  VARCHAR (100) NOT NULL,
    [SettingKey]   VARCHAR (50)  NOT NULL,
    [SettingValue] VARCHAR (MAX) NOT NULL,
    [SettingDesc]  VARCHAR (MAX) NULL,
    [IsEditable]   BIT           CONSTRAINT [DF_Settings_IsEditable] DEFAULT ((1)) NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Settings_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]  DATETIME      CONSTRAINT [DF_Settings_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]    BIGINT        NOT NULL,
    [UpdatedDate]  DATETIME      CONSTRAINT [DF_Settings_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]    BIGINT        NOT NULL,
    [IsDeleted]    BIT           CONSTRAINT [DF_Settings_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED ([SettingId] ASC)
);



