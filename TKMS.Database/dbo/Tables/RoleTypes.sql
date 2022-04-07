CREATE TABLE [dbo].[RoleTypes] (
    [RoleTypeId]        BIGINT       NOT NULL,
    [RoleTypeName]      VARCHAR (50) NOT NULL,
    [RoleTypeKey]       VARCHAR (50) NULL,
    [SortOrder]         INT          CONSTRAINT [DF_RoleTypes_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsAllowUserCreate] BIT          CONSTRAINT [DF_RoleTypes_IsAllowUserCreate] DEFAULT ((0)) NOT NULL,
    [IsActive]          BIT          CONSTRAINT [DF_RoleTypes_IsActive] DEFAULT ((0)) NOT NULL,
    [CreatedDate]       DATETIME     CONSTRAINT [DF_RoleTypes_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         BIGINT       NOT NULL,
    [UpdatedDate]       DATETIME     CONSTRAINT [DF_RoleTypes_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]         BIGINT       NOT NULL,
    [IsDeleted]         BIT          CONSTRAINT [DF_RoleTypes_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RoleTypes] PRIMARY KEY CLUSTERED ([RoleTypeId] ASC)
);



