CREATE TABLE [dbo].[Users] (
    [UserId]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [StaffId]     VARCHAR (50)  NOT NULL,
    [FullName]    VARCHAR (100) NOT NULL,
    [BranchId]    BIGINT        NOT NULL,
    [RoleTypeId]  BIGINT        NOT NULL,
    [Email]       VARCHAR (100) NULL,
    [Password]    VARCHAR (255) CONSTRAINT [DF_Users_Password] DEFAULT ('') NOT NULL,
    [Salt]        VARCHAR (255) NULL,
    [MobileNo]    VARCHAR (50)  CONSTRAINT [DF_Users_MobileNo] DEFAULT ('') NOT NULL,
    [IsActive]    BIT           CONSTRAINT [DF_Users_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate] DATETIME      CONSTRAINT [DF_Users_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   BIGINT        NOT NULL,
    [UpdatedDate] DATETIME      CONSTRAINT [DF_Users_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]   BIGINT        NOT NULL,
    [IsDeleted]   BIT           CONSTRAINT [DF_Users_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);









