CREATE TABLE [dbo].[SchemeC5Codes] (
    [SchemeC5CodeId] BIGINT IDENTITY (1, 1) NOT NULL,
    [SchemeCodeId]   BIGINT CONSTRAINT [DF_SchemeC5Codes_SchemeCodeId] DEFAULT ((0)) NOT NULL,
    [C5CodeId]       BIGINT NOT NULL,
    CONSTRAINT [PK_SchemeC5Codes] PRIMARY KEY CLUSTERED ([SchemeC5CodeId] ASC)
);

