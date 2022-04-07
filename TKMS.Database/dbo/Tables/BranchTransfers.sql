CREATE TABLE [dbo].[BranchTransfers] (
    [BranchTransferId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [KitId]            BIGINT        NOT NULL,
    [FromBranchId]     BIGINT        NOT NULL,
    [ToBranchId]       BIGINT        NOT NULL,
    [TransferById]     BIGINT        NOT NULL,
    [TransferDate]     DATETIME      NOT NULL,
    [ReceivedById]     BIGINT        NULL,
    [ReceivedDate]     DATETIME      NULL,
    [Remarks]          VARCHAR (500) NULL,
    [IsActive]         BIT           CONSTRAINT [DF_BranchTransfers_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]      DATETIME      CONSTRAINT [DF_BranchTransfers_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        BIGINT        NOT NULL,
    [UpdatedDate]      DATETIME      CONSTRAINT [DF_BranchTransfers_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]        BIGINT        NOT NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_BranchTransfers_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BranchTransfers] PRIMARY KEY CLUSTERED ([BranchTransferId] ASC)
);



