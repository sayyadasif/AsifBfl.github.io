CREATE TABLE [dbo].[SchemeCodes] (
    [SchemeCodeId]   BIGINT       IDENTITY (1, 1) NOT NULL,
    [SchemeCodeName] VARCHAR (50) NOT NULL,
    [SortOrder]      INT          CONSTRAINT [DF_SchemeCodes_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsActive]       BIT          CONSTRAINT [DF_SchemeCodes_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]    DATETIME     CONSTRAINT [DF_SchemeCodes_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      BIGINT       NOT NULL,
    [UpdatedDate]    DATETIME     CONSTRAINT [DF_SchemeCodes_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]      BIGINT       NOT NULL,
    [IsDeleted]      BIT          CONSTRAINT [DF_SchemeCodes_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SchemeCodes] PRIMARY KEY CLUSTERED ([SchemeCodeId] ASC)
);

