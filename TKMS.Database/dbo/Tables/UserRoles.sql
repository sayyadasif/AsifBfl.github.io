CREATE TABLE [dbo].[UserRoles] (
    [UserRoleId] BIGINT IDENTITY (1, 1) NOT NULL,
    [UserId]     BIGINT NOT NULL,
    [RoleId]     BIGINT NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([UserRoleId] ASC)
);

