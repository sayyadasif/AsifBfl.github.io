CREATE TABLE [dbo].[CardTypes] (
    [CardTypeId]   BIGINT       IDENTITY (1, 1) NOT NULL,
    [CardTypeName] VARCHAR (50) NOT NULL,
    [SortOrder]    INT          CONSTRAINT [DF_CardTypes_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsActive]     BIT          CONSTRAINT [DF_CardTypes_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]  DATETIME     CONSTRAINT [DF_CardTypes_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]    BIGINT       NOT NULL,
    [UpdatedDate]  DATETIME     CONSTRAINT [DF_CardTypes_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]    BIGINT       NOT NULL,
    [IsDeleted]    BIT          CONSTRAINT [DF_CardTypes_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CardTypes] PRIMARY KEY CLUSTERED ([CardTypeId] ASC)
);



