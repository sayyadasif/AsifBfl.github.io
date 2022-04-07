CREATE TABLE [dbo].[SentSmses] (
    [SentSmsId]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [MobileNumber]    VARCHAR (20)  NOT NULL,
    [Otp]             VARCHAR (20)  NULL,
    [Message]         VARCHAR (500) NOT NULL,
    [IsSuccess]       BIT           CONSTRAINT [DF_Smses_IsSuccess] DEFAULT ((0)) NOT NULL,
    [ResponseMessage] VARCHAR (500) NULL,
    [IsActive]        BIT           CONSTRAINT [DF_Smses_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]     DATETIME      CONSTRAINT [DF_Smses_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       BIGINT        NOT NULL,
    [UpdatedDate]     DATETIME      CONSTRAINT [DF_Smses_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]       BIGINT        NOT NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_Smses_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Smses] PRIMARY KEY CLUSTERED ([SentSmsId] ASC)
);

