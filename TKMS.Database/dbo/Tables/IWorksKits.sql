CREATE TABLE [dbo].[IWorksKits] (
    [IWorksKitId]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [KitId]           BIGINT        NOT NULL,
    [AccountNo]       VARCHAR (50)  NOT NULL,
    [IWorksStatus]    VARCHAR (50)  NOT NULL,
    [IsSuccess]       BIT           CONSTRAINT [DF_IWorksKits_IsSuccess] DEFAULT ((0)) NOT NULL,
    [ResponseMessage] VARCHAR (MAX) NULL,
    [RetryCount]      INT           CONSTRAINT [DF_IWorksKits_RetryCount] DEFAULT ((0)) NOT NULL,
    [IsActive]        BIT           CONSTRAINT [DF_IWorksKits_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]     DATETIME      CONSTRAINT [DF_IWorksKits_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       BIGINT        NOT NULL,
    [UpdatedDate]     DATETIME      CONSTRAINT [DF_IWorksKits_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]       BIGINT        NOT NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_IWorksKits_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_IWorksKits] PRIMARY KEY CLUSTERED ([IWorksKitId] ASC)
);



