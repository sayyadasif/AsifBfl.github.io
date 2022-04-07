CREATE TABLE [dbo].[DispatchWayBills] (
    [DispatchWayBillId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [DispatchId]        BIGINT        NOT NULL,
    [WayBillNo]         VARCHAR (50)  NOT NULL,
    [DispatchDate]      DATETIME      NOT NULL,
    [DeliveryDate]      DATETIME      NULL,
    [CourierStatusId]   BIGINT        NOT NULL,
    [ReceiveBy]         VARCHAR (50)  NULL,
    [ReceiveStatusId]   BIGINT        NULL,
    [ReportUploadBy]    BIGINT        NULL,
    [ReportUploadDate]  DATETIME      NULL,
    [DispatchStatusId]  BIGINT        NOT NULL,
    [CourierPartner]    VARCHAR (50)  NULL,
    [Remarks]           VARCHAR (500) NULL,
    [IsActive]          BIT           CONSTRAINT [DF_DispatchWayBills_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]       DATETIME      CONSTRAINT [DF_DispatchWayBills_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         BIGINT        NOT NULL,
    [UpdatedDate]       DATETIME      CONSTRAINT [DF_DispatchWayBills_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]         BIGINT        NOT NULL,
    [IsDeleted]         BIT           CONSTRAINT [DF_DispatchWayBills_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DispatchWayBills] PRIMARY KEY CLUSTERED ([DispatchWayBillId] ASC)
);





