CREATE TABLE [dbo].[C5Codes] (
    [C5CodeId]    BIGINT       IDENTITY (1, 1) NOT NULL,
    [CardTypeId]  BIGINT       CONSTRAINT [DF_C5Codes_CardTypeId] DEFAULT ((0)) NOT NULL,
    [C5CodeName]  VARCHAR (50) NOT NULL,
    [SortOrder]   INT          CONSTRAINT [DF_C5Codes_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsActive]    BIT          CONSTRAINT [DF_C5Codes_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate] DATETIME     CONSTRAINT [DF_C5Codes_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   BIGINT       NOT NULL,
    [UpdatedDate] DATETIME     CONSTRAINT [DF_C5Codes_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]   BIGINT       NOT NULL,
    [IsDeleted]   BIT          CONSTRAINT [DF_C5Codes_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_C5Codes] PRIMARY KEY CLUSTERED ([C5CodeId] ASC)
);





