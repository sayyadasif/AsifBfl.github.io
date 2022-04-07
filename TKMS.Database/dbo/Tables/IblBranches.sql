CREATE TABLE [dbo].[IblBranches] (
    [IblBranchId]   BIGINT       IDENTITY (1, 1) NOT NULL,
    [IblBranchName] VARCHAR (50) NOT NULL,
    [IblBranchCode] VARCHAR (20) NOT NULL,
    [SortOrder]     INT          CONSTRAINT [DF_IblBranches_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsActive]      BIT          CONSTRAINT [DF_IblBranches_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]   DATETIME     CONSTRAINT [DF_IblBranches_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     BIGINT       NOT NULL,
    [UpdatedDate]   DATETIME     CONSTRAINT [DF_IblBranches_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]     BIGINT       NOT NULL,
    [IsDeleted]     BIT          CONSTRAINT [DF_IblBranches_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_IblBranches] PRIMARY KEY CLUSTERED ([IblBranchId] ASC)
);

