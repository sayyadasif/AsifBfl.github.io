CREATE TABLE [dbo].[Roles] (
    [RoleId]            BIGINT       NOT NULL,
    [RoleTypeId]        BIGINT       NOT NULL,
    [RoleName]          VARCHAR (50) NOT NULL,
    [RoleKey]           VARCHAR (50) NOT NULL,
    [SortOrder]         INT          CONSTRAINT [DF_Roles_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsAllowUserCreate] BIT          CONSTRAINT [DF_Roles_IsAllowUserCreate] DEFAULT ((0)) NOT NULL,
    [IsActive]          BIT          CONSTRAINT [DF_Roles_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]       DATETIME     CONSTRAINT [DF_Roles_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         BIGINT       NOT NULL,
    [UpdatedDate]       DATETIME     CONSTRAINT [DF_Roles_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]         BIGINT       NOT NULL,
    [IsDeleted]         BIT          CONSTRAINT [DF_Roles_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);





