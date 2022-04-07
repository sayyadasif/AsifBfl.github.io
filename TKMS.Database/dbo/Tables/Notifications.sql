CREATE TABLE [dbo].[Notifications] (
    [NotificationId]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [NotificationTypeId] BIGINT        NOT NULL,
    [BranchId]           BIGINT        NOT NULL,
    [Title]              VARCHAR (100) NOT NULL,
    [Description]        VARCHAR (MAX) NOT NULL,
    [DispatchId]         BIGINT        NULL,
    [KitId]              BIGINT        NULL,
    [RedirectUrl]        VARCHAR (100) NULL,
    [VisibleToRoles]     VARCHAR (50)  NULL,
    [IsActive]           BIT           CONSTRAINT [DF_Notifications_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]        DATETIME      CONSTRAINT [DF_Notifications_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [UpdatedDate]        DATETIME      CONSTRAINT [DF_Notifications_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]          BIGINT        NOT NULL,
    [IsDeleted]          BIT           CONSTRAINT [DF_Notifications_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([NotificationId] ASC)
);





