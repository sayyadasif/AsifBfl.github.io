CREATE TABLE [dbo].[Branches] (
    [BranchId]     BIGINT       IDENTITY (1, 1) NOT NULL,
    [BranchName]   VARCHAR (50) NOT NULL,
    [BranchCode]   VARCHAR (20) NOT NULL,
    [RegionId]     BIGINT       NOT NULL,
    [BranchTypeId] BIGINT       NOT NULL,
    [IblBranchId]  BIGINT       CONSTRAINT [DF_Branches_IBLBranchId] DEFAULT ((0)) NOT NULL,
    [SchemeCodeId] BIGINT       CONSTRAINT [DF_Branches_SchemeCodeId] DEFAULT ((0)) NOT NULL,
    [C5CodeId]     BIGINT       CONSTRAINT [DF_Branches_C5CodeId] DEFAULT ((0)) NOT NULL,
    [CardTypeId]   BIGINT       CONSTRAINT [DF_Branches_CardTypeId] DEFAULT ((0)) NOT NULL,
    [AddressId]    BIGINT       NOT NULL,
    [IfscCode]     VARCHAR (11) CONSTRAINT [DF_Branches_IfscCode] DEFAULT ('') NOT NULL,
    [SortOrder]    INT          CONSTRAINT [DF_Branches_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsActive]     BIT          CONSTRAINT [DF_Branches_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]  DATETIME     CONSTRAINT [DF_Branches_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]    BIGINT       NOT NULL,
    [UpdatedDate]  DATETIME     CONSTRAINT [DF_Branches_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]    BIGINT       NOT NULL,
    [IsDeleted]    BIT          CONSTRAINT [DF_Branches_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Branches] PRIMARY KEY CLUSTERED ([BranchId] ASC)
);













