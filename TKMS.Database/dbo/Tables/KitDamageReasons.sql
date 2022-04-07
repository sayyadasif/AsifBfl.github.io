CREATE TABLE [dbo].[KitDamageReasons] (
    [KitDamageReasonId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [Reason]            VARCHAR (50) NOT NULL,
    [SortOrder]         INT          CONSTRAINT [DF_KitDamageReasons_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsActive]          BIT          CONSTRAINT [DF_KitDamageReasons_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]       DATETIME     CONSTRAINT [DF_KitDamageReasons_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         BIGINT       NOT NULL,
    [UpdatedDate]       DATETIME     CONSTRAINT [DF_KitDamageReasons_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]         BIGINT       NOT NULL,
    [IsDeleted]         BIT          CONSTRAINT [DF_KitDamageReasons_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_KitDamageReasons] PRIMARY KEY CLUSTERED ([KitDamageReasonId] ASC)
);

