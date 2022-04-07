CREATE TABLE [dbo].[NotificationTypes] (
    [NotificationTypeId]   BIGINT        NOT NULL,
    [NotificationTypeName] VARCHAR (100) NOT NULL,
    [NotificationTemplate] VARCHAR (MAX) NOT NULL,
    [UrlTemplate]          VARCHAR (100) NULL,
    [Description]          VARCHAR (MAX) NULL,
    [VisibleToRoles]       VARCHAR (50)  NULL,
    [IsActive]             BIT           CONSTRAINT [DF_NotificationTypes_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]          DATETIME      CONSTRAINT [DF_NotificationTypes_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]            BIGINT        NOT NULL,
    [UpdatedDate]          DATETIME      CONSTRAINT [DF_NotificationTypes_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]            BIGINT        NOT NULL,
    [IsDeleted]            BIT           CONSTRAINT [DF_NotificationTypes_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_NotificationTypes] PRIMARY KEY CLUSTERED ([NotificationTypeId] ASC)
);

