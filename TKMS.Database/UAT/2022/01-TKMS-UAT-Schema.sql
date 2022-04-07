/****** Object:  Table [dbo].[Addresses]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Addresses](
	[AddressId] [bigint] IDENTITY(1,1) NOT NULL,
	[AddressDetail] [varchar](max) NOT NULL,
	[Region] [varchar](50) NULL,
	[Zone] [varchar](50) NULL,
	[District] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[PinCode] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BranchDispatches]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BranchDispatches](
	[BranchDispatchId] [bigint] IDENTITY(1,1) NOT NULL,
	[DispatchId] [bigint] NOT NULL,
	[DispatchMode] [varchar](50) NOT NULL,
	[StaffId] [varchar](50) NULL,
	[StaffName] [varchar](50) NULL,
	[StaffContactNo] [varchar](50) NULL,
	[CourierName] [varchar](50) NULL,
	[WayBillNo] [varchar](50) NULL,
	[Remarks] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_BranchDispatches] PRIMARY KEY CLUSTERED 
(
	[BranchDispatchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Branches]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branches](
	[BranchId] [bigint] IDENTITY(1,1) NOT NULL,
	[BranchName] [varchar](50) NOT NULL,
	[BranchCode] [varchar](20) NOT NULL,
	[RegionId] [bigint] NOT NULL,
	[BranchTypeId] [bigint] NOT NULL,
	[AddressId] [bigint] NOT NULL,
	[IfscCode] [varchar](11) NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Branches] PRIMARY KEY CLUSTERED 
(
	[BranchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BranchTransfers]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BranchTransfers](
	[BranchTransferId] [bigint] IDENTITY(1,1) NOT NULL,
	[KitId] [bigint] NOT NULL,
	[FromBranchId] [bigint] NOT NULL,
	[ToBranchId] [bigint] NOT NULL,
	[TransferById] [bigint] NOT NULL,
	[TransferDate] [datetime] NOT NULL,
	[ReceivedById] [bigint] NULL,
	[ReceivedDate] [datetime] NULL,
	[Remarks] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_BranchTransfers] PRIMARY KEY CLUSTERED 
(
	[BranchTransferId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BranchTypes]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BranchTypes](
	[BranchTypeId] [bigint] NOT NULL,
	[BranchTypeName] [varchar](50) NOT NULL,
	[BranchTypeKey] [varchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsAllowIndent] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_BranchTypes] PRIMARY KEY CLUSTERED 
(
	[BranchTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[C5Codes]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[C5Codes](
	[C5CodeId] [bigint] IDENTITY(1,1) NOT NULL,
	[C5CodeName] [varchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_C5Codes] PRIMARY KEY CLUSTERED 
(
	[C5CodeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CardTypes]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CardTypes](
	[CardTypeId] [bigint] IDENTITY(1,1) NOT NULL,
	[CardTypeName] [varchar](50) NOT NULL,
	[CardTypeKey] [varchar](50) NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_CardTypes] PRIMARY KEY CLUSTERED 
(
	[CardTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourierStatuses]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourierStatuses](
	[CourierStatusId] [bigint] NOT NULL,
	[StatusName] [varchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_CourierStatuses] PRIMARY KEY CLUSTERED 
(
	[CourierStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DispatchAudits]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DispatchAudits](
	[DispatchAuditId] [bigint] IDENTITY(1,1) NOT NULL,
	[DispatchId] [bigint] NOT NULL,
	[IndentId] [bigint] NOT NULL,
	[Vendor] [varchar](50) NOT NULL,
	[DispatchQty] [int] NOT NULL,
	[AccountStart] [varchar](50) NOT NULL,
	[AccountEnd] [varchar](50) NOT NULL,
	[SchemeType] [varchar](50) NOT NULL,
	[DispatchDate] [datetime] NOT NULL,
	[ReferenceNo] [varchar](50) NOT NULL,
	[DispatchStatusId] [bigint] NOT NULL,
	[BranchDispatchDate] [datetime] NULL,
	[BranchReceiveDate] [datetime] NULL,
	[Remarks] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_DispatchAudits] PRIMARY KEY CLUSTERED 
(
	[DispatchAuditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dispatches]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dispatches](
	[DispatchId] [bigint] IDENTITY(1,1) NOT NULL,
	[IndentId] [bigint] NOT NULL,
	[Vendor] [varchar](50) NOT NULL,
	[DispatchQty] [int] NOT NULL,
	[AccountStart] [varchar](50) NOT NULL,
	[AccountEnd] [varchar](50) NOT NULL,
	[SchemeType] [varchar](50) NOT NULL,
	[DispatchDate] [datetime] NOT NULL,
	[ReferenceNo] [varchar](50) NOT NULL,
	[DispatchStatusId] [bigint] NOT NULL,
	[BranchDispatchDate] [datetime] NULL,
	[BranchReceiveDate] [datetime] NULL,
	[Remarks] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Dispatches] PRIMARY KEY CLUSTERED 
(
	[DispatchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DispatchStatuses]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DispatchStatuses](
	[DispatchStatusId] [bigint] NOT NULL,
	[StatusName] [varchar](50) NOT NULL,
	[StatusKey] [varchar](50) NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_DispatchStatuses] PRIMARY KEY CLUSTERED 
(
	[DispatchStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DispatchWayBills]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DispatchWayBills](
	[DispatchWayBillId] [bigint] IDENTITY(1,1) NOT NULL,
	[DispatchId] [bigint] NOT NULL,
	[WayBillNo] [varchar](50) NOT NULL,
	[DispatchDate] [datetime] NOT NULL,
	[DeliveryDate] [datetime] NULL,
	[CourierStatusId] [bigint] NOT NULL,
	[ReceiveBy] [varchar](50) NULL,
	[ReceiveStatusId] [bigint] NULL,
	[ReportUploadBy] [bigint] NULL,
	[ReportUploadDate] [datetime] NULL,
	[DispatchStatusId] [bigint] NOT NULL,
	[CourierPartner] [varchar](50) NULL,
	[Remarks] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_DispatchWayBills] PRIMARY KEY CLUSTERED 
(
	[DispatchWayBillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documents]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documents](
	[DocumentId] [bigint] IDENTITY(1,1) NOT NULL,
	[DocumentTypeId] [bigint] NOT NULL,
	[DocumentType] [varchar](50) NOT NULL,
	[FileName] [varchar](100) NULL,
	[FilePath] [varchar](255) NULL,
	[ContentType] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[DocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IndentAudits]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndentAudits](
	[IndentAuditId] [bigint] IDENTITY(1,1) NOT NULL,
	[IndentId] [bigint] NOT NULL,
	[BranchId] [bigint] NOT NULL,
	[SchemeCodeId] [bigint] NOT NULL,
	[C5CodeId] [bigint] NOT NULL,
	[CardTypeId] [bigint] NOT NULL,
	[BfilBranchTypeId] [bigint] NOT NULL,
	[BfilRegionId] [bigint] NOT NULL,
	[BfilBranchId] [bigint] NOT NULL,
	[DispatchAddressId] [bigint] NOT NULL,
	[IndentStatusId] [bigint] NOT NULL,
	[IndentNo] [varchar](50) NOT NULL,
	[IndentDate] [datetime] NOT NULL,
	[NoOfKit] [int] NOT NULL,
	[ContactName] [varchar](500) NOT NULL,
	[ContactNo] [varchar](500) NOT NULL,
	[RejectionReasonId] [bigint] NULL,
	[HoApproveStatusId] [bigint] NULL,
	[HoApproveBy] [bigint] NULL,
	[HoApproveDate] [datetime] NULL,
	[CpuApproveStatusId] [bigint] NULL,
	[CpuApproveBy] [bigint] NULL,
	[CpuApproveDate] [datetime] NULL,
	[Remarks] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
 CONSTRAINT [PK_IndentAudits] PRIMARY KEY CLUSTERED 
(
	[IndentAuditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Indents]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Indents](
	[IndentId] [bigint] IDENTITY(1,1) NOT NULL,
	[BranchId] [bigint] NOT NULL,
	[SchemeCodeId] [bigint] NOT NULL,
	[C5CodeId] [bigint] NOT NULL,
	[CardTypeId] [bigint] NOT NULL,
	[BfilBranchTypeId] [bigint] NOT NULL,
	[BfilRegionId] [bigint] NOT NULL,
	[BfilBranchId] [bigint] NOT NULL,
	[DispatchAddressId] [bigint] NOT NULL,
	[IndentStatusId] [bigint] NOT NULL,
	[IndentNo] [varchar](50) NOT NULL,
	[IndentDate] [datetime] NOT NULL,
	[NoOfKit] [int] NOT NULL,
	[ContactName] [varchar](500) NOT NULL,
	[ContactNo] [varchar](500) NOT NULL,
	[RejectionReasonId] [bigint] NULL,
	[HoApproveStatusId] [bigint] NULL,
	[HoApproveBy] [bigint] NULL,
	[HoApproveDate] [datetime] NULL,
	[CpuApproveStatusId] [bigint] NULL,
	[CpuApproveBy] [bigint] NULL,
	[CpuApproveDate] [datetime] NULL,
	[Remarks] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Indents] PRIMARY KEY CLUSTERED 
(
	[IndentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IndentStatuses]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndentStatuses](
	[IndentStatusId] [bigint] NOT NULL,
	[StatusName] [varchar](50) NOT NULL,
	[StatusKey] [varchar](50) NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_IndentStatuses] PRIMARY KEY CLUSTERED 
(
	[IndentStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IWorksKits]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IWorksKits](
	[IWorksKitId] [bigint] IDENTITY(1,1) NOT NULL,
	[KitId] [bigint] NOT NULL,
	[AccountNo] [varchar](50) NOT NULL,
	[IWorksStatus] [varchar](50) NOT NULL,
	[IsSuccess] [bit] NOT NULL,
	[ResponseMessage] [varchar](255) NULL,
	[RetryCount] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_IWorksKits] PRIMARY KEY CLUSTERED 
(
	[IWorksKitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KitAudits]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KitAudits](
	[KitAuditId] [bigint] IDENTITY(1,1) NOT NULL,
	[KitId] [bigint] NOT NULL,
	[IndentId] [bigint] NOT NULL,
	[DispatchId] [bigint] NOT NULL,
	[BranchId] [bigint] NOT NULL,
	[CifNo] [varchar](50) NOT NULL,
	[AccountNo] [varchar](50) NOT NULL,
	[KitStatusId] [bigint] NOT NULL,
	[AllocatedToId] [bigint] NULL,
	[AllocatedDate] [datetime] NULL,
	[AllocatedById] [bigint] NULL,
	[KitDamageReasonId] [bigint] NULL,
	[DamageRemarks] [varchar](500) NULL,
	[IsDestructionApproved] [bit] NULL,
	[DestructionById] [bigint] NULL,
	[DestructionRemarks] [varchar](500) NULL,
	[AssignedDate] [datetime] NULL,
	[CustomerName] [varchar](50) NULL,
	[Remarks] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_KitAudits] PRIMARY KEY CLUSTERED 
(
	[KitAuditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KitDamageReasons]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KitDamageReasons](
	[KitDamageReasonId] [bigint] IDENTITY(1,1) NOT NULL,
	[Reason] [varchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_KitDamageReasons] PRIMARY KEY CLUSTERED 
(
	[KitDamageReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KitReturns]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KitReturns](
	[KitReturnId] [bigint] IDENTITY(1,1) NOT NULL,
	[KitId] [bigint] NOT NULL,
	[ReturnBy] [bigint] NOT NULL,
	[ReturnDate] [datetime] NOT NULL,
	[ReturnAcceptedBy] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_KitReturns] PRIMARY KEY CLUSTERED 
(
	[KitReturnId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Kits]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kits](
	[KitId] [bigint] IDENTITY(1,1) NOT NULL,
	[IndentId] [bigint] NOT NULL,
	[DispatchId] [bigint] NOT NULL,
	[BranchId] [bigint] NOT NULL,
	[CifNo] [varchar](50) NOT NULL,
	[AccountNo] [varchar](50) NOT NULL,
	[KitStatusId] [bigint] NOT NULL,
	[AllocatedToId] [bigint] NULL,
	[AllocatedDate] [datetime] NULL,
	[AllocatedById] [bigint] NULL,
	[KitDamageReasonId] [bigint] NULL,
	[DamageRemarks] [varchar](500) NULL,
	[IsDestructionApproved] [bit] NULL,
	[DestructionById] [bigint] NULL,
	[DestructionRemarks] [varchar](500) NULL,
	[AssignedDate] [datetime] NULL,
	[CustomerName] [varchar](50) NULL,
	[Remarks] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Kits] PRIMARY KEY CLUSTERED 
(
	[KitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KitStatuses]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KitStatuses](
	[KitStatusId] [bigint] NOT NULL,
	[StatusName] [varchar](50) NOT NULL,
	[StatusKey] [varchar](50) NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_KitStatuses] PRIMARY KEY CLUSTERED 
(
	[KitStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](128) NULL,
	[TimeStamp] [datetimeoffset](7) NOT NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [xml] NULL,
	[LogEvent] [nvarchar](max) NULL,
	[UserName] [nvarchar](200) NULL,
	[IP] [varchar](200) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[NotificationId] [bigint] IDENTITY(1,1) NOT NULL,
	[NotificationTypeId] [bigint] NOT NULL,
	[BranchId] [bigint] NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[DispatchId] [bigint] NULL,
	[KitId] [bigint] NULL,
	[RedirectUrl] [varchar](100) NULL,
	[VisibleToRoles] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationTypes]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationTypes](
	[NotificationTypeId] [bigint] NOT NULL,
	[NotificationTypeName] [varchar](100) NOT NULL,
	[NotificationTemplate] [varchar](max) NOT NULL,
	[UrlTemplate] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[VisibleToRoles] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_NotificationTypes] PRIMARY KEY CLUSTERED 
(
	[NotificationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Regions]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Regions](
	[RegionId] [bigint] IDENTITY(1,1) NOT NULL,
	[SystemRoId] [varchar](50) NOT NULL,
	[RegionName] [varchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[AddressId] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Regions] PRIMARY KEY CLUSTERED 
(
	[RegionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RejectionReasons]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RejectionReasons](
	[RejectionReasonId] [bigint] IDENTITY(1,1) NOT NULL,
	[Reason] [varchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_RejectionReasons] PRIMARY KEY CLUSTERED 
(
	[RejectionReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [bigint] NOT NULL,
	[RoleTypeId] [bigint] NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
	[RoleKey] [varchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsAllowUserCreate] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleTypes]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleTypes](
	[RoleTypeId] [bigint] NOT NULL,
	[RoleTypeName] [varchar](50) NOT NULL,
	[RoleTypeKey] [varchar](50) NULL,
	[SortOrder] [int] NOT NULL,
	[IsAllowUserCreate] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_RoleTypes] PRIMARY KEY CLUSTERED 
(
	[RoleTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchemeCodes]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchemeCodes](
	[SchemeCodeId] [bigint] IDENTITY(1,1) NOT NULL,
	[SchemeCodeName] [varchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_SchemeCodes] PRIMARY KEY CLUSTERED 
(
	[SchemeCodeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SentSmses]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SentSmses](
	[SentSmsId] [bigint] IDENTITY(1,1) NOT NULL,
	[MobileNumber] [varchar](20) NOT NULL,
	[Otp] [varchar](20) NULL,
	[Message] [varchar](500) NOT NULL,
	[IsSuccess] [bit] NOT NULL,
	[ResponseMessage] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Smses] PRIMARY KEY CLUSTERED 
(
	[SentSmsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[SettingId] [bigint] IDENTITY(1,1) NOT NULL,
	[SettingName] [varchar](100) NOT NULL,
	[SettingKey] [varchar](50) NOT NULL,
	[SettingValue] [varchar](max) NOT NULL,
	[SettingDesc] [varchar](max) NULL,
	[IsEditable] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[SettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserRoleId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 18-01-2022 15:11:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[StaffId] [varchar](50) NOT NULL,
	[FullName] [varchar](100) NOT NULL,
	[BranchId] [bigint] NOT NULL,
	[RoleTypeId] [bigint] NOT NULL,
	[Email] [varchar](100) NULL,
	[Password] [varchar](255) NOT NULL,
	[Salt] [varchar](255) NULL,
	[MobileNo] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Addresses] ADD  CONSTRAINT [DF_Table_1_SortOrder]  DEFAULT ((1000)) FOR [Zone]
GO
ALTER TABLE [dbo].[Addresses] ADD  CONSTRAINT [DF_Table_1_IsActive]  DEFAULT ((1)) FOR [District]
GO
ALTER TABLE [dbo].[Addresses] ADD  CONSTRAINT [DF_Table_1_CreatedDate]  DEFAULT (getdate()) FOR [State]
GO
ALTER TABLE [dbo].[Addresses] ADD  CONSTRAINT [DF_Addresses_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Addresses] ADD  CONSTRAINT [DF_Addresses_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Addresses] ADD  CONSTRAINT [DF_Addresses_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Addresses] ADD  CONSTRAINT [DF_Addresses_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[BranchDispatches] ADD  CONSTRAINT [DF_BranchDispatches_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BranchDispatches] ADD  CONSTRAINT [DF_BranchDispatches_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[BranchDispatches] ADD  CONSTRAINT [DF_BranchDispatches_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[BranchDispatches] ADD  CONSTRAINT [DF_BranchDispatches_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Branches] ADD  CONSTRAINT [DF_Branches_IfscCode]  DEFAULT ('') FOR [IfscCode]
GO
ALTER TABLE [dbo].[Branches] ADD  CONSTRAINT [DF_Branches_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[Branches] ADD  CONSTRAINT [DF_Branches_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Branches] ADD  CONSTRAINT [DF_Branches_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Branches] ADD  CONSTRAINT [DF_Branches_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Branches] ADD  CONSTRAINT [DF_Branches_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[BranchTransfers] ADD  CONSTRAINT [DF_BranchTransfers_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BranchTransfers] ADD  CONSTRAINT [DF_BranchTransfers_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[BranchTransfers] ADD  CONSTRAINT [DF_BranchTransfers_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[BranchTransfers] ADD  CONSTRAINT [DF_BranchTransfers_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[BranchTypes] ADD  CONSTRAINT [DF_BranchTypes_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[BranchTypes] ADD  CONSTRAINT [DF_BranchTypes_IsActive1]  DEFAULT ((0)) FOR [IsAllowIndent]
GO
ALTER TABLE [dbo].[BranchTypes] ADD  CONSTRAINT [DF_BranchTypes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BranchTypes] ADD  CONSTRAINT [DF_BranchTypes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[BranchTypes] ADD  CONSTRAINT [DF_BranchTypes_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[BranchTypes] ADD  CONSTRAINT [DF_BranchTypes_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[C5Codes] ADD  CONSTRAINT [DF_C5Codes_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[C5Codes] ADD  CONSTRAINT [DF_C5Codes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[C5Codes] ADD  CONSTRAINT [DF_C5Codes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[C5Codes] ADD  CONSTRAINT [DF_C5Codes_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[C5Codes] ADD  CONSTRAINT [DF_C5Codes_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[CardTypes] ADD  CONSTRAINT [DF_CardTypes_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[CardTypes] ADD  CONSTRAINT [DF_CardTypes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[CardTypes] ADD  CONSTRAINT [DF_CardTypes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[CardTypes] ADD  CONSTRAINT [DF_CardTypes_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[CardTypes] ADD  CONSTRAINT [DF_CardTypes_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[CourierStatuses] ADD  CONSTRAINT [DF_CourierStatuses_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[CourierStatuses] ADD  CONSTRAINT [DF_CourierStatuses_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[CourierStatuses] ADD  CONSTRAINT [DF_CourierStatuses_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[CourierStatuses] ADD  CONSTRAINT [DF_CourierStatuses_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[CourierStatuses] ADD  CONSTRAINT [DF_CourierStatuses_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[DispatchAudits] ADD  CONSTRAINT [DF_DispatchAudits_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[DispatchAudits] ADD  CONSTRAINT [DF_DispatchAudits_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[DispatchAudits] ADD  CONSTRAINT [DF_DispatchAudits_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[DispatchAudits] ADD  CONSTRAINT [DF_DispatchAudits_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Dispatches] ADD  CONSTRAINT [DF_Dispatches_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Dispatches] ADD  CONSTRAINT [DF_Dispatches_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Dispatches] ADD  CONSTRAINT [DF_Dispatches_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Dispatches] ADD  CONSTRAINT [DF_Dispatches_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[DispatchStatuses] ADD  CONSTRAINT [DF_DispatchStatuses_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[DispatchStatuses] ADD  CONSTRAINT [DF_DispatchStatuses_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[DispatchStatuses] ADD  CONSTRAINT [DF_DispatchStatuses_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[DispatchStatuses] ADD  CONSTRAINT [DF_DispatchStatuses_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[DispatchStatuses] ADD  CONSTRAINT [DF_DispatchStatuses_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[DispatchWayBills] ADD  CONSTRAINT [DF_DispatchWayBills_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[DispatchWayBills] ADD  CONSTRAINT [DF_DispatchWayBills_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[DispatchWayBills] ADD  CONSTRAINT [DF_DispatchWayBills_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[DispatchWayBills] ADD  CONSTRAINT [DF_DispatchWayBills_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [DF_Table_1_SortOrder_1]  DEFAULT ((1000)) FOR [FileName]
GO
ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [DF_Documents_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [DF_Documents_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [DF_Documents_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [DF_Documents_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[IndentAudits] ADD  CONSTRAINT [DF_IndentAudits_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[IndentAudits] ADD  CONSTRAINT [DF_IndentAudits_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[IndentAudits] ADD  CONSTRAINT [DF_IndentAudits_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[IndentAudits] ADD  CONSTRAINT [DF_IndentAudits_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[IndentAudits] ADD  CONSTRAINT [DF_IndentAudits_TimeStamp]  DEFAULT (getdate()) FOR [TimeStamp]
GO
ALTER TABLE [dbo].[Indents] ADD  CONSTRAINT [DF_Indents_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Indents] ADD  CONSTRAINT [DF_Indents_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Indents] ADD  CONSTRAINT [DF_Indents_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Indents] ADD  CONSTRAINT [DF_Indents_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[IndentStatuses] ADD  CONSTRAINT [DF_IndentStatuses_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[IndentStatuses] ADD  CONSTRAINT [DF_IndentStatuses_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[IndentStatuses] ADD  CONSTRAINT [DF_IndentStatuses_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[IndentStatuses] ADD  CONSTRAINT [DF_IndentStatuses_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[IndentStatuses] ADD  CONSTRAINT [DF_IndentStatuses_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[IWorksKits] ADD  CONSTRAINT [DF_IWorksKits_IsSuccess]  DEFAULT ((0)) FOR [IsSuccess]
GO
ALTER TABLE [dbo].[IWorksKits] ADD  CONSTRAINT [DF_IWorksKits_RetryCount]  DEFAULT ((0)) FOR [RetryCount]
GO
ALTER TABLE [dbo].[IWorksKits] ADD  CONSTRAINT [DF_IWorksKits_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[IWorksKits] ADD  CONSTRAINT [DF_IWorksKits_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[IWorksKits] ADD  CONSTRAINT [DF_IWorksKits_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[IWorksKits] ADD  CONSTRAINT [DF_IWorksKits_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[KitAudits] ADD  CONSTRAINT [DF_KitAudits_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[KitAudits] ADD  CONSTRAINT [DF_KitAudits_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[KitAudits] ADD  CONSTRAINT [DF_KitAudits_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[KitAudits] ADD  CONSTRAINT [DF_KitAudits_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[KitDamageReasons] ADD  CONSTRAINT [DF_KitDamageReasons_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[KitDamageReasons] ADD  CONSTRAINT [DF_KitDamageReasons_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[KitDamageReasons] ADD  CONSTRAINT [DF_KitDamageReasons_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[KitDamageReasons] ADD  CONSTRAINT [DF_KitDamageReasons_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[KitDamageReasons] ADD  CONSTRAINT [DF_KitDamageReasons_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[KitReturns] ADD  CONSTRAINT [DF_KitReturns_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[KitReturns] ADD  CONSTRAINT [DF_KitReturns_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[KitReturns] ADD  CONSTRAINT [DF_KitReturns_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[KitReturns] ADD  CONSTRAINT [DF_KitReturns_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Kits] ADD  CONSTRAINT [DF_Kits_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Kits] ADD  CONSTRAINT [DF_Kits_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Kits] ADD  CONSTRAINT [DF_Kits_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Kits] ADD  CONSTRAINT [DF_Kits_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[KitStatuses] ADD  CONSTRAINT [DF_KitStatuses_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[KitStatuses] ADD  CONSTRAINT [DF_KitStatuses_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[KitStatuses] ADD  CONSTRAINT [DF_KitStatuses_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[KitStatuses] ADD  CONSTRAINT [DF_KitStatuses_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[KitStatuses] ADD  CONSTRAINT [DF_KitStatuses_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Notifications] ADD  CONSTRAINT [DF_Notifications_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Notifications] ADD  CONSTRAINT [DF_Notifications_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Notifications] ADD  CONSTRAINT [DF_Notifications_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Notifications] ADD  CONSTRAINT [DF_Notifications_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[NotificationTypes] ADD  CONSTRAINT [DF_NotificationTypes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NotificationTypes] ADD  CONSTRAINT [DF_NotificationTypes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[NotificationTypes] ADD  CONSTRAINT [DF_NotificationTypes_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[NotificationTypes] ADD  CONSTRAINT [DF_NotificationTypes_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Regions] ADD  CONSTRAINT [DF_Regions_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[Regions] ADD  CONSTRAINT [DF_Regions_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Regions] ADD  CONSTRAINT [DF_Regions_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Regions] ADD  CONSTRAINT [DF_Regions_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Regions] ADD  CONSTRAINT [DF_Regions_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RejectionReasons] ADD  CONSTRAINT [DF_RejectionReasons_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[RejectionReasons] ADD  CONSTRAINT [DF_RejectionReasons_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[RejectionReasons] ADD  CONSTRAINT [DF_RejectionReasons_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RejectionReasons] ADD  CONSTRAINT [DF_RejectionReasons_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[RejectionReasons] ADD  CONSTRAINT [DF_RejectionReasons_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_IsAllowUserCreate]  DEFAULT ((0)) FOR [IsAllowUserCreate]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RoleTypes] ADD  CONSTRAINT [DF_RoleTypes_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[RoleTypes] ADD  CONSTRAINT [DF_RoleTypes_IsAllowUserCreate]  DEFAULT ((0)) FOR [IsAllowUserCreate]
GO
ALTER TABLE [dbo].[RoleTypes] ADD  CONSTRAINT [DF_RoleTypes_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[RoleTypes] ADD  CONSTRAINT [DF_RoleTypes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RoleTypes] ADD  CONSTRAINT [DF_RoleTypes_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[RoleTypes] ADD  CONSTRAINT [DF_RoleTypes_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[SchemeCodes] ADD  CONSTRAINT [DF_SchemeCodes_SortOrder]  DEFAULT ((1000)) FOR [SortOrder]
GO
ALTER TABLE [dbo].[SchemeCodes] ADD  CONSTRAINT [DF_SchemeCodes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SchemeCodes] ADD  CONSTRAINT [DF_SchemeCodes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SchemeCodes] ADD  CONSTRAINT [DF_SchemeCodes_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[SchemeCodes] ADD  CONSTRAINT [DF_SchemeCodes_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[SentSmses] ADD  CONSTRAINT [DF_Smses_IsSuccess]  DEFAULT ((0)) FOR [IsSuccess]
GO
ALTER TABLE [dbo].[SentSmses] ADD  CONSTRAINT [DF_Smses_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SentSmses] ADD  CONSTRAINT [DF_Smses_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SentSmses] ADD  CONSTRAINT [DF_Smses_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[SentSmses] ADD  CONSTRAINT [DF_Smses_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_IsEditable]  DEFAULT ((1)) FOR [IsEditable]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Password]  DEFAULT ('') FOR [Password]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_MobileNo]  DEFAULT ('') FOR [MobileNo]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
