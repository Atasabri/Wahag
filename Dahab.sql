
CREATE TABLE [dbo].[Admins](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[IsManager] [bit] NOT NULL,
	[Access_Codes] [bit] NOT NULL,
	[Access_Categories] [bit] NOT NULL,
	[Access_Products] [bit] NOT NULL,
	[Access_Users] [bit] NOT NULL,
	[Access_Orders] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 11/19/2019 9:56:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Name_EN] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Codes]    Script Date: 11/19/2019 9:56:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Codes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [int] NOT NULL,
	[Discount] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 11/19/2019 9:56:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Subject] [nvarchar](max) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Favorites]    Script Date: 11/19/2019 9:56:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Favorites](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NOT NULL,
	[Product_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order_Details]    Script Date: 11/19/2019 9:56:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order_Details](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Order_ID] [int] NOT NULL,
	[Product_ID] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[Price] [float] NOT NULL,
	[Message] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Orders]    Script Date: 11/19/2019 9:56:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[User_ID] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[Place] [nvarchar](max) NOT NULL,
	[Lat] [float] NULL,
	[Log] [float] NULL,
	[UseVisa] [bit] NOT NULL,
	[TotalPrice] [float] NOT NULL,
	[Discount] [float] NULL,
	[DiscountType] [nvarchar](max) NULL,
	[FinalPrice] [float] NOT NULL,
	[IsArriveToUser] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Products]    Script Date: 11/19/2019 9:56:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Name_EN] [nvarchar](max) NOT NULL,
	[Model] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Description_EN] [nvarchar](max) NOT NULL,
	[Discount] [float] NULL,
	[Price] [float] NOT NULL,
	[TotalPrice] [float] NOT NULL,
	[Cat_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sizes]    Script Date: 11/19/2019 9:56:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sizes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Size] [int] NOT NULL,
	[Cat_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Subscribers]    Script Date: 11/19/2019 9:56:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscribers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/19/2019 9:56:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Token] [nvarchar](200) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NULL,
	[AllowNews] [bit] NOT NULL,
	[Address] [nvarchar](max) NULL,
	[BillAddress] [nvarchar](max) NULL,
	[Facebook_ID] [nvarchar](200) NULL,
	[HasOrders] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Admins] ON 

GO
INSERT [dbo].[Admins] ([ID], [UserName], [Password], [IsManager], [Access_Codes], [Access_Categories], [Access_Products], [Access_Users], [Access_Orders]) VALUES (1, N'Ata Sabri', N'01142229025', 1, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[Admins] ([ID], [UserName], [Password], [IsManager], [Access_Codes], [Access_Categories], [Access_Products], [Access_Users], [Access_Orders]) VALUES (4, N'Ahmed', N'123456', 0, 0, 1, 1, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[Admins] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

GO
INSERT [dbo].[Categories] ([ID], [Name], [Name_EN]) VALUES (9, N'النوع الاول ', N'Category 1')
GO
INSERT [dbo].[Categories] ([ID], [Name], [Name_EN]) VALUES (10, N'النوع الثاني', N'Category 2')
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Codes] ON 

GO
INSERT [dbo].[Codes] ([ID], [Code], [Discount]) VALUES (4, 95706, 5)
GO
INSERT [dbo].[Codes] ([ID], [Code], [Discount]) VALUES (12, 446121, 9)
GO
INSERT [dbo].[Codes] ([ID], [Code], [Discount]) VALUES (1015, 254395, 80)
GO
SET IDENTITY_INSERT [dbo].[Codes] OFF
GO
SET IDENTITY_INSERT [dbo].[Favorites] ON 

GO
INSERT [dbo].[Favorites] ([ID], [User_ID], [Product_ID]) VALUES (3, 1, 11)
GO
SET IDENTITY_INSERT [dbo].[Favorites] OFF
GO
SET IDENTITY_INSERT [dbo].[Order_Details] ON 

GO
INSERT [dbo].[Order_Details] ([ID], [Order_ID], [Product_ID], [Count], [Price], [Message]) VALUES (30, 16, 11, 1, 8000, N'Test')
GO
INSERT [dbo].[Order_Details] ([ID], [Order_ID], [Product_ID], [Count], [Price], [Message]) VALUES (31, 17, 11, 1, 8000, N'Test')
GO
SET IDENTITY_INSERT [dbo].[Order_Details] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

GO
INSERT [dbo].[Orders] ([ID], [Date], [User_ID], [Name], [Phone], [City], [Place], [Lat], [Log], [UseVisa], [TotalPrice], [Discount], [DiscountType], [FinalPrice], [IsArriveToUser]) VALUES (14, CAST(0x0000A96E01307AED AS DateTime), 1, N'Ata Sabri Ata Ahmed', N'01142229025', N'Cairo', N'6 October', NULL, NULL, 1, 185, 10, N'First Order Discount', 166.5, 1)
GO
INSERT [dbo].[Orders] ([ID], [Date], [User_ID], [Name], [Phone], [City], [Place], [Lat], [Log], [UseVisa], [TotalPrice], [Discount], [DiscountType], [FinalPrice], [IsArriveToUser]) VALUES (15, CAST(0x0000A974011C790E AS DateTime), 1, N'Ata Sabri Ata Ahmed', N'01142229025', N'Cairo', N'6 October', 29.9626118, 30.943626, 1, 95, NULL, NULL, 95, 1)
GO
INSERT [dbo].[Orders] ([ID], [Date], [User_ID], [Name], [Phone], [City], [Place], [Lat], [Log], [UseVisa], [TotalPrice], [Discount], [DiscountType], [FinalPrice], [IsArriveToUser]) VALUES (16, CAST(0x0000A97600FF5D98 AS DateTime), 1, N'Ata Sabri Ata Ahmed', N'01142229025', N'Cairo', N'6 October', NULL, NULL, 1, 8000, NULL, NULL, 8000, 0)
GO
INSERT [dbo].[Orders] ([ID], [Date], [User_ID], [Name], [Phone], [City], [Place], [Lat], [Log], [UseVisa], [TotalPrice], [Discount], [DiscountType], [FinalPrice], [IsArriveToUser]) VALUES (17, CAST(0x0000A9760100E0EC AS DateTime), 1, N'Ata Sabri Ata Ahmed', N'01142229025', N'Cairo', N'6 October', NULL, NULL, 1, 8000, 10, N'Code Discount', 7200, 0)
GO
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

GO
INSERT [dbo].[Products] ([ID], [Name], [Name_EN], [Model], [Description], [Description_EN], [Discount], [Price], [TotalPrice], [Cat_ID]) VALUES (11, N'المنتج الاول', N'Product 2', N'موديل', N'تفاصيل المنتج الاول', N'Description for Product 1', 20, 10000, 8000, 9)
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Sizes] ON 

GO
INSERT [dbo].[Sizes] ([ID], [Size], [Cat_ID]) VALUES (11, 33, 9)
GO
INSERT [dbo].[Sizes] ([ID], [Size], [Cat_ID]) VALUES (12, 66, 9)
GO
INSERT [dbo].[Sizes] ([ID], [Size], [Cat_ID]) VALUES (13, 77, 9)
GO
INSERT [dbo].[Sizes] ([ID], [Size], [Cat_ID]) VALUES (14, 99, 10)
GO
INSERT [dbo].[Sizes] ([ID], [Size], [Cat_ID]) VALUES (15, 12, 10)
GO
INSERT [dbo].[Sizes] ([ID], [Size], [Cat_ID]) VALUES (16, 22, 10)
GO
SET IDENTITY_INSERT [dbo].[Sizes] OFF
GO
SET IDENTITY_INSERT [dbo].[Subscribers] ON 

GO
INSERT [dbo].[Subscribers] ([ID], [Email]) VALUES (2, N'ataeldon@gmail.com')
GO
SET IDENTITY_INSERT [dbo].[Subscribers] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

GO
INSERT [dbo].[Users] ([ID], [Name], [Token], [FirstName], [LastName], [Phone], [Email], [Password], [AllowNews], [Address], [BillAddress], [Facebook_ID], [HasOrders]) VALUES (1, N'Ata Sabri', N't7fCbntMWD/40QY7jsXmT4a3jYMNJ7Fmazx5divD14xJZS6iZZk8sst+A+I7kOsi', N'Ata', N'Sabri', N'01142229025', N'ataeldon@gmail.com', N'123456', 1, N'', N'', NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__tmp_ms_x__C9F284560DE5644C]    Script Date: 11/19/2019 9:56:16 AM ******/
ALTER TABLE [dbo].[Admins] ADD UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Codes__A25C5AA79897C6E3]    Script Date: 11/19/2019 9:56:16 AM ******/
ALTER TABLE [dbo].[Codes] ADD UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__tmp_ms_x__1EB4F81793E9AEAD]    Script Date: 11/19/2019 9:56:16 AM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__tmp_ms_x__A9D10534A0836AA1]    Script Date: 11/19/2019 9:56:16 AM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[Favorites]  WITH CHECK ADD  CONSTRAINT [FK__Favorites__Produ__1FCDBCEB] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Favorites] CHECK CONSTRAINT [FK__Favorites__Produ__1FCDBCEB]
GO
ALTER TABLE [dbo].[Favorites]  WITH CHECK ADD  CONSTRAINT [FK__Favorites__User___1ED998B2] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Favorites] CHECK CONSTRAINT [FK__Favorites__User___1ED998B2]
GO
ALTER TABLE [dbo].[Order_Details]  WITH CHECK ADD  CONSTRAINT [FK__Order_Det__Order__267ABA7A] FOREIGN KEY([Order_ID])
REFERENCES [dbo].[Orders] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order_Details] CHECK CONSTRAINT [FK__Order_Det__Order__267ABA7A]
GO
ALTER TABLE [dbo].[Order_Details]  WITH CHECK ADD  CONSTRAINT [FK__Order_Det__Produ__276EDEB3] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order_Details] CHECK CONSTRAINT [FK__Order_Det__Produ__276EDEB3]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK__Orders__User_ID__239E4DCF] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK__Orders__User_ID__239E4DCF]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK__Products__Cat_ID__1BFD2C07] FOREIGN KEY([Cat_ID])
REFERENCES [dbo].[Categories] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK__Products__Cat_ID__1BFD2C07]
GO
ALTER TABLE [dbo].[Sizes]  WITH CHECK ADD  CONSTRAINT [FK__Sizes__Cat_ID__15502E78] FOREIGN KEY([Cat_ID])
REFERENCES [dbo].[Categories] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Sizes] CHECK CONSTRAINT [FK__Sizes__Cat_ID__15502E78]
GO
USE [master]
GO
ALTER DATABASE [Dahab] SET  READ_WRITE 
GO
