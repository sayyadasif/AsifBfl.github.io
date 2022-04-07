CREATE TABLE [dbo].[DispatchStatuses] (
    [DispatchStatusId] BIGINT       NOT NULL,
    [StatusName]       VARCHAR (50) NOT NULL,
    [StatusKey]        VARCHAR (50) NULL,
    [SortOrder]        INT          CONSTRAINT [DF_DispatchStatuses_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsActive]         BIT          CONSTRAINT [DF_DispatchStatuses_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]      DATETIME     CONSTRAINT [DF_DispatchStatuses_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        BIGINT       NOT NULL,
    [UpdatedDate]      DATETIME     CONSTRAINT [DF_DispatchStatuses_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]        BIGINT       NOT NULL,
    [IsDeleted]        BIT          CONSTRAINT [DF_DispatchStatuses_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DispatchStatuses] PRIMARY KEY CLUSTERED ([DispatchStatusId] ASC)
);



