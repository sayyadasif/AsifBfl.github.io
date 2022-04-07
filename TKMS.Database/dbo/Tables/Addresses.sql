CREATE TABLE [dbo].[Addresses] (
    [AddressId]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [AddressDetail] VARCHAR (MAX) NOT NULL,
    [Region]        VARCHAR (50)  NULL,
    [Zone]          VARCHAR (50)  CONSTRAINT [DF_Table_1_SortOrder] DEFAULT ((1000)) NULL,
    [District]      VARCHAR (50)  CONSTRAINT [DF_Table_1_IsActive] DEFAULT ((1)) NULL,
    [State]         VARCHAR (50)  CONSTRAINT [DF_Table_1_CreatedDate] DEFAULT (getdate()) NULL,
    [PinCode]       VARCHAR (50)  NOT NULL,
    [IsActive]      BIT           CONSTRAINT [DF_Addresses_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_Addresses_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     BIGINT        NOT NULL,
    [UpdatedDate]   DATETIME      CONSTRAINT [DF_Addresses_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]     BIGINT        NOT NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_Addresses_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ([AddressId] ASC)
);



