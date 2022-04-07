CREATE TABLE [dbo].[BranchTypes] (
    [BranchTypeId]   BIGINT       NOT NULL,
    [BranchTypeName] VARCHAR (50) NOT NULL,
    [BranchTypeKey]  VARCHAR (50) NOT NULL,
    [SortOrder]      INT          CONSTRAINT [DF_BranchTypes_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsAllowIndent]  BIT          CONSTRAINT [DF_BranchTypes_IsActive1] DEFAULT ((0)) NOT NULL,
    [IsActive]       BIT          CONSTRAINT [DF_BranchTypes_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]    DATETIME     CONSTRAINT [DF_BranchTypes_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      BIGINT       NOT NULL,
    [UpdatedDate]    DATETIME     CONSTRAINT [DF_BranchTypes_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]      BIGINT       NOT NULL,
    [IsDeleted]      BIT          CONSTRAINT [DF_BranchTypes_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BranchTypes] PRIMARY KEY CLUSTERED ([BranchTypeId] ASC)
);





