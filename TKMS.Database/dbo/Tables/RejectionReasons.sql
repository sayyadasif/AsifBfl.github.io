CREATE TABLE [dbo].[RejectionReasons] (
    [RejectionReasonId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [Reason]            VARCHAR (50) NOT NULL,
    [SortOrder]         INT          CONSTRAINT [DF_RejectionReasons_SortOrder] DEFAULT ((1000)) NOT NULL,
    [IsActive]          BIT          CONSTRAINT [DF_RejectionReasons_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]       DATETIME     CONSTRAINT [DF_RejectionReasons_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         BIGINT       NOT NULL,
    [UpdatedDate]       DATETIME     CONSTRAINT [DF_RejectionReasons_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]         BIGINT       NOT NULL,
    [IsDeleted]         BIT          CONSTRAINT [DF_RejectionReasons_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RejectionReasons] PRIMARY KEY CLUSTERED ([RejectionReasonId] ASC)
);



