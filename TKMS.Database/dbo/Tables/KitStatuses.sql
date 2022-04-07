CREATE TABLE [dbo].[KitStatuses] (
    [KitStatusId] BIGINT       NOT NULL,
    [StatusName]  VARCHAR (50) NOT NULL,
    [StatusKey]   VARCHAR (50) NULL,
    [SortOrder]   INT          CONSTRAINT [DF_KitStatuses_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsActive]    BIT          CONSTRAINT [DF_KitStatuses_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate] DATETIME     CONSTRAINT [DF_KitStatuses_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   BIGINT       NOT NULL,
    [UpdatedDate] DATETIME     CONSTRAINT [DF_KitStatuses_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]   BIGINT       NOT NULL,
    [IsDeleted]   BIT          CONSTRAINT [DF_KitStatuses_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_KitStatuses] PRIMARY KEY CLUSTERED ([KitStatusId] ASC)
);



