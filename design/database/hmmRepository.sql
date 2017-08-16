SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[NoteCatalogues]    Script Date: 05/02/2016 11:31:36 ******/
CREATE TABLE [dbo].[NoteCatalogues](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](200) NOT NULL,
    [Schema] XML NOT NULL,
    [Description] [nvarchar](1000) NULL,
CONSTRAINT [PK_NoteCatalogues] PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Tags](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](200) NOT NULL,
    [IsActivated] BIT NOT NULL,
    [Description] [nvarchar](1000) NULL,
CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[ContactInfoCatalogues]    Script Date: 05/02/2016 11:31:36 ******/
CREATE TABLE [dbo].[ContactInfoCatalogues](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](200) NOT NULL,
    [Description] [nvarchar](1000) NULL,
CONSTRAINT [PK_ContactInfoCatalogues] PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[NoteRenders]    Script Date: 05/02/2016 11:31:36 ******/
CREATE TABLE [dbo].[NoteRenders](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](1000) NOT NULL,
    [Namespace] NVARCHAR(1000) NOT NULL,
    [Description] [nvarchar](1000) NULL,
CONSTRAINT [PK_NoteRenders] PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 05/02/2016 11:31:36 ******/
CREATE TABLE [dbo].[Users](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [BirthDay] DATETIME NOT NULL,
    [AccountName] [nvarchar](256) NOT NULL,
    [Password] NVARCHAR(128) NOT NULL,
    [Salt] NVARCHAR(128) NOT NULL,
    [IsActivated] BIT NOT NULL,
    [Description] [nvarchar](1000) NULL,
CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Index [IDX_Unique_AccountName]    Script Date: 05/02/2016 11:42:02 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IDX_Unique_AccountName] ON [dbo].[Users] 
(
	[AccountName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Emails]    Script Date: 05/02/2016 11:31:36 ******/
CREATE TABLE [dbo].[Emails](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [Email] NVARCHAR(1000) NOT NULL,
    [Owner] INT NOT NULL,
    [IsPrimary] BIT NOT NULL,
    [Catalog] INT NOT NULL,
    [IsActivated] BIT NOT NULL,
    [Description] [nvarchar](1000) NULL,
CONSTRAINT [PK_Emails] PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Emails]  WITH CHECK ADD  CONSTRAINT [FK_Emails_ContactInfoCatalogues] FOREIGN KEY([Catalog])
REFERENCES [dbo].[ContactInfoCatalogues] ([ID])
GO

ALTER TABLE [dbo].[Emails] CHECK CONSTRAINT [FK_Emails_ContactInfoCatalogues]
GO

ALTER TABLE [dbo].[Emails]  WITH CHECK ADD  CONSTRAINT [FK_Emails_Users] FOREIGN KEY([Owner])
REFERENCES [dbo].[Users] ([ID])
GO

ALTER TABLE [dbo].[Emails] CHECK CONSTRAINT [FK_Emails_Users]
GO


/****** Object:  Table [dbo].[Phones]    Script Date: 05/02/2016 11:31:36 ******/
CREATE TABLE [dbo].[Phones](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [AreaCode] INT NULL,
    [Phone] NVARCHAR(1000) NOT NULL,
    [Extension] INT NULL,
    [Owner] INT NOT NULL,
    [Country] NVARCHAR(3) NULL,
    [Catalog] INT NOT NULL,
    [IsActivated] BIT NOT NULL,
    [Description] [nvarchar](1000) NULL,
CONSTRAINT [PK_Phones] PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Phones]  WITH CHECK ADD  CONSTRAINT [FK_Phones_ContactInfoCatalogues] FOREIGN KEY([Catalog])
REFERENCES [dbo].[ContactInfoCatalogues] ([ID])
GO

ALTER TABLE [dbo].[Phones] CHECK CONSTRAINT [FK_Phones_ContactInfoCatalogues]
GO

ALTER TABLE [dbo].[Phones]  WITH CHECK ADD  CONSTRAINT [FK_Phones_Users] FOREIGN KEY([Owner])
REFERENCES [dbo].[Users] ([ID])
GO

ALTER TABLE [dbo].[Phones] CHECK CONSTRAINT [FK_Phones_Users]
GO

/****** Object:  Table [dbo].[Notes]    Script Date: 05/02/2016 11:31:36 ******/
CREATE TABLE [dbo].[Notes](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [Subject] [nvarchar](1000) NOT NULL,
    [Content] XML NOT NULL,
    [CatalogId] INT NOT NULL,
    [RenderId] INT NOT NULL,
    [AuthorId] INT NOT NULL,
    [CreateDate] DATETIME NOT NULL,
    [LastModifiedDate] DATETIME NOT NULL,
    [Description] [nvarchar](1000) NULL,
    [Ts] [TIMESTAMP] NOT NULL,
CONSTRAINT [PK_Notes] PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_NoteCatalogues] FOREIGN KEY([CatalogId])
REFERENCES [dbo].[NoteCatalogues] ([ID])
GO

ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_NoteCatalogues]
GO

ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_NoteRenders] FOREIGN KEY([RenderId])
REFERENCES [dbo].[NoteRenders] ([ID])
GO

ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_NoteRenders]
GO

ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_Users] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Users] ([ID])
GO

ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_Users]
GO

CREATE NONCLUSTERED INDEX [IDX_NoteCatalog] ON [dbo].[Notes] 
(
	[CatalogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[NoteTagRefs]    Script Date: 05/02/2016 11:31:36 ******/
CREATE TABLE [dbo].[NoteTagRefs](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [Note] [INT] NOT NULL,
    [Tag] [INT] NOT NULL,
CONSTRAINT [PK_NoteTagRefs] PRIMARY KEY CLUSTERED 
(
    [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[NoteTagRefs]  WITH CHECK ADD  CONSTRAINT [FK_NoteTagRefs_Notes] FOREIGN KEY([Note])
REFERENCES [dbo].[Notes] ([ID])
GO

ALTER TABLE [dbo].[NoteTagRefs] CHECK CONSTRAINT [FK_NoteTagRefs_Notes]
GO

ALTER TABLE [dbo].[NoteTagRefs]  WITH CHECK ADD  CONSTRAINT [FK_NoteTagRefs_Tags] FOREIGN KEY([Tag])
REFERENCES [dbo].[Tags] ([ID])
GO

ALTER TABLE [dbo].[NoteTagRefs] CHECK CONSTRAINT [FK_NoteTagRefs_Tags]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_Unique_Note_Tag_Comb] ON [dbo].[NoteTagRefs] 
(
	[Note] ASC,
	[Tag] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/***** Insert seed data *****/
SET IDENTITY_INSERT dbo.NoteRenders ON
INSERT INTO dbo.NoteRenders( Id, Name, Namespace, Description ) VALUES(1, N'DefaultNoteRender', N'Hmm.Renders', N'default render of note')
SET IDENTITY_INSERT dbo.NoteRenders OFF

SET IDENTITY_INSERT dbo.NoteCatalogues ON
INSERT INTO dbo.NoteCatalogues(Id, Name, [Schema], [Description] ) VALUES ( 1, N'DefaultNoteCatalog', '<?xml version="1.0" encoding="UTF-8" ?><xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:vc="http://www.w3.org/2007/XMLSchema-versioning" xmlns:rns="http://schema.hmm.com/2017" targetNamespace="http://schema.hmm.com/2017" elementFormDefault="qualified" attributeFormDefault="unqualified" vc:minVersion="1.1"><xs:element name="HmmNote"><xs:annotation><xs:documentation>The root of all note managed by HMM</xs:documentation></xs:annotation><xs:complexType><xs:sequence><xs:element name="Content" type="xs:string"/></xs:sequence></xs:complexType></xs:element></xs:schema>', N'default note catalog')
SET IDENTITY_INSERT dbo.NoteCatalogues OFF
