CREATE TABLE [dbo].[CourierStatuses] (
    [CourierStatusId] BIGINT       NOT NULL,
    [StatusName]      VARCHAR (50) NOT NULL,
    [SortOrder]       INT          CONSTRAINT [DF_CourierStatuses_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_CourierStatuses_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]     DATETIME     CONSTRAINT [DF_CourierStatuses_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       BIGINT       NOT NULL,
    [UpdatedDate]     DATETIME     CONSTRAINT [DF_CourierStatuses_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]       BIGINT       NOT NULL,
    [IsDeleted]       BIT          CONSTRAINT [DF_CourierStatuses_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CourierStatuses] PRIMARY KEY CLUSTERED ([CourierStatusId] ASC)
);



