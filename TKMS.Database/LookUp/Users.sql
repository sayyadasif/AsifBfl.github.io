SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT INTO [dbo].[Users]
           ([UserId]
           ,[StaffId]
           ,[FullName]
           ,[BranchId]
           ,[RoleTypeId]
           ,[Email]
           ,[Password]
           ,[Salt]
           ,[MobileNo]
           ,[IsActive]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdatedDate]
           ,[UpdatedBy]
           ,[IsDeleted])
     VALUES
           (1
		   ,'Admin'
           ,'Administrator'
           ,0
           ,6
           ,'admin@test.com'
           ,'eYwgHqZ/DHGPfUMj8Swk/YSsiUPRGZq/WfSNv04VLfQ=' --Admin123
           ,'j0E1sK50Dw7aPQ=='
           ,'9999999999'
           ,1
           ,GETDATE()
           ,1
           ,GETDATE()
           ,1
           ,0)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO

INSERT INTO [dbo].[UserRoles]
           ([UserId]
           ,[RoleId])
     VALUES
           (1
           ,10)
GO
