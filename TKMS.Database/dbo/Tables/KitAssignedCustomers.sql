CREATE TABLE [dbo].[KitAssignedCustomers] (
    [KitAssignedCustomerId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [AccountNumber]         VARCHAR (50)  NOT NULL,
    [AssignedDate]          DATETIME      NOT NULL,
    [CustomerName]          VARCHAR (255) NOT NULL,
    [CreatedDate]           DATETIME      CONSTRAINT [DF_KitAssignedCustomers_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [ProcessedDate]         DATETIME      NULL,
    CONSTRAINT [PK_KitAssignedCustomers] PRIMARY KEY CLUSTERED ([KitAssignedCustomerId] ASC)
);



