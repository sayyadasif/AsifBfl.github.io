SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[KitAssignedCustomers](
	[KitAssignedCustomerId] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountNumber] [varchar](50) NOT NULL,
	[AssignedDate] [datetime] NOT NULL,
	[CustomerName] [varchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ProcessedDate] [datetime] NULL,
 CONSTRAINT [PK_KitAssignedCustomers] PRIMARY KEY CLUSTERED 
(
	[KitAssignedCustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[KitAssignedCustomers] ADD  CONSTRAINT [DF_KitAssignedCustomers_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
