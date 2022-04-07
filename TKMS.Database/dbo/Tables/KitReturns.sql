CREATE TABLE [dbo].[KitReturns] (
    [KitReturnId]      BIGINT   IDENTITY (1, 1) NOT NULL,
    [KitId]            BIGINT   NOT NULL,
    [ReturnBy]         BIGINT   NOT NULL,
    [ReturnDate]       DATETIME NOT NULL,
    [ReturnAcceptedBy] BIGINT   NOT NULL,
    [IsActive]         BIT      CONSTRAINT [DF_KitReturns_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]      DATETIME CONSTRAINT [DF_KitReturns_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        BIGINT   NOT NULL,
    [UpdatedDate]      DATETIME CONSTRAINT [DF_KitReturns_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]        BIGINT   NOT NULL,
    [IsDeleted]        BIT      CONSTRAINT [DF_KitReturns_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_KitReturns] PRIMARY KEY CLUSTERED ([KitReturnId] ASC)
);

