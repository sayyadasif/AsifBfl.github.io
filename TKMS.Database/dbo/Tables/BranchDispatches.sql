CREATE TABLE [dbo].[BranchDispatches] (
    [BranchDispatchId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [DispatchId]       BIGINT        NOT NULL,
    [DispatchMode]     VARCHAR (50)  NOT NULL,
    [StaffId]          VARCHAR (50)  NULL,
    [StaffName]        VARCHAR (50)  NULL,
    [StaffContactNo]   VARCHAR (50)  NULL,
    [CourierName]      VARCHAR (50)  NULL,
    [WayBillNo]        VARCHAR (50)  NULL,
    [Remarks]          VARCHAR (500) NULL,
    [IsActive]         BIT           CONSTRAINT [DF_BranchDispatches_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]      DATETIME      CONSTRAINT [DF_BranchDispatches_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        BIGINT        NOT NULL,
    [UpdatedDate]      DATETIME      CONSTRAINT [DF_BranchDispatches_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]        BIGINT        NOT NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_BranchDispatches_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BranchDispatches] PRIMARY KEY CLUSTERED ([BranchDispatchId] ASC)
);

