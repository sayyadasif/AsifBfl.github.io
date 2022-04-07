CREATE TABLE [dbo].[Documents] (
    [DocumentId]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [DocumentTypeId] BIGINT        NOT NULL,
    [DocumentType]   VARCHAR (50)  NOT NULL,
    [FileName]       VARCHAR (100) CONSTRAINT [DF_Table_1_SortOrder_1] DEFAULT ((1000)) NULL,
    [FilePath]       VARCHAR (255) NULL,
    [ContentType]    VARCHAR (50)  NULL,
    [IsActive]       BIT           CONSTRAINT [DF_Documents_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]    DATETIME      CONSTRAINT [DF_Documents_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      BIGINT        NOT NULL,
    [UpdatedDate]    DATETIME      CONSTRAINT [DF_Documents_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]      BIGINT        NOT NULL,
    [IsDeleted]      BIT           CONSTRAINT [DF_Documents_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED ([DocumentId] ASC)
);

