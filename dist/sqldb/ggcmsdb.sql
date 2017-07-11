/*
Navicat SQL Server Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 120000
Source Host           : 127.0.0.1:1433
Source Database       : GgcmsDB
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 120000
File Encoding         : 65001

Date: 2017-07-01 17:48:50
*/


-- ----------------------------
-- Table structure for GgcmsArticlePages
-- ----------------------------
DROP TABLE [dbo].[GgcmsArticlePages]
GO
CREATE TABLE [dbo].[GgcmsArticlePages] (
[Id] int NOT NULL IDENTITY(1,1) ,
[OrderId] int NOT NULL ,
[Content] nvarchar(MAX) NULL ,
[Title] nvarchar(255) NULL ,
[Article_Id] int NOT NULL 
)


GO

-- ----------------------------
-- Records of GgcmsArticlePages
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsArticlePages] ON
GO
SET IDENTITY_INSERT [dbo].[GgcmsArticlePages] OFF
GO

-- ----------------------------
-- Table structure for GgcmsArticles
-- ----------------------------
DROP TABLE [dbo].[GgcmsArticles]
GO
CREATE TABLE [dbo].[GgcmsArticles] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Content] nvarchar(MAX) NULL ,
[Title] nvarchar(255) NULL ,
[Hits] int NOT NULL ,
[CreateTime] datetime NOT NULL ,
[TitleImg] nvarchar(255) NULL ,
[TitleThumbnail] nvarchar(255) NULL ,
[MemberId] int NOT NULL ,
[RedirectUrl] nvarchar(255) NULL ,
[Source] nvarchar(255) NULL ,
[SourceUrl] nvarchar(255) NULL ,
[Keywords] nvarchar(255) NULL ,
[Description] nvarchar(255) NULL ,
[TmplName] nvarchar(50) NULL ,
[StyleName] nvarchar(50) NULL ,
[PageTitle] nvarchar(255) NULL ,
[ExtModelId] int NOT NULL ,
[MobileTmplName] nvarchar(50) NULL ,
[ShowType] int NOT NULL ,
[ShowLevel] int NOT NULL ,
[Author] nvarchar(50) NULL ,
[Category_Id] int NOT NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[GgcmsArticles]', RESEED, 23)
GO

-- ----------------------------
-- Records of GgcmsArticles
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsArticles] ON
GO
INSERT INTO [dbo].[GgcmsArticles] ([Id], [Content], [Title], [Hits], [CreateTime], [TitleImg], [TitleThumbnail], [MemberId], [RedirectUrl], [Source], [SourceUrl], [Keywords], [Description], [TmplName], [StyleName], [PageTitle], [ExtModelId], [MobileTmplName], [ShowType], [ShowLevel], [Author], [Category_Id]) VALUES (N'22', N'<p>fadsafdsafdsa</p><p>fadsafdsafdsa</p><p>fadsafdsafdsa</p><p>fadsafdsafdsa</p><p>fadsafdsafdsa</p><p>fadsafdsafdsa</p>', N'dsfafdsafdsaf', N'0', N'2017-06-29 17:39:15.317', N'', N'', N'0', N'', N'', N'', N'', N'dsafdsdsdfdsadfs', N'', N'', N'', N'0', N'', N'0', N'0', N'abc', N'19')
GO
GO
INSERT INTO [dbo].[GgcmsArticles] ([Id], [Content], [Title], [Hits], [CreateTime], [TitleImg], [TitleThumbnail], [MemberId], [RedirectUrl], [Source], [SourceUrl], [Keywords], [Description], [TmplName], [StyleName], [PageTitle], [ExtModelId], [MobileTmplName], [ShowType], [ShowLevel], [Author], [Category_Id]) VALUES (N'23', N'<p>dsadsddasdas</p>', N'dddddddddddd', N'0', N'2017-06-30 12:56:39.103', N'', N'', N'0', N'', N'', N'', N'ddddddddddddddddddd', N'ddddddddddddd', N'', N'', N'', N'0', N'', N'0', N'0', N'', N'18')
GO
GO
SET IDENTITY_INSERT [dbo].[GgcmsArticles] OFF
GO

-- ----------------------------
-- Table structure for GgcmsAttachments
-- ----------------------------
DROP TABLE [dbo].[GgcmsAttachments]
GO
CREATE TABLE [dbo].[GgcmsAttachments] (
[Id] int NOT NULL IDENTITY(1,1) ,
[AttaTitle] nvarchar(255) NULL ,
[AttaUrl] nvarchar(255) NULL ,
[Describe] nvarchar(255) NULL ,
[AttaSize] bigint NOT NULL ,
[RealName] nvarchar(255) NULL ,
[CreateTime] datetime NOT NULL ,
[Articles_Id] int NOT NULL 
)


GO

-- ----------------------------
-- Records of GgcmsAttachments
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsAttachments] ON
GO
SET IDENTITY_INSERT [dbo].[GgcmsAttachments] OFF
GO

-- ----------------------------
-- Table structure for GgcmsCategories
-- ----------------------------
DROP TABLE [dbo].[GgcmsCategories]
GO
CREATE TABLE [dbo].[GgcmsCategories] (
[Id] int NOT NULL IDENTITY(1,1) ,
[CategoryName] nvarchar(50) NOT NULL ,
[OrderId] int NOT NULL ,
[LogoImg] nvarchar(255) NULL ,
[StyleName] nvarchar(50) NULL ,
[ParentId] int NOT NULL ,
[TmplName] nvarchar(50) NULL ,
[MobileTmplName] nvarchar(50) NULL ,
[ArticleTmplName] nvarchar(50) NULL ,
[ArticleMobileTmplName] nvarchar(50) NULL ,
[RedirectUrl] nvarchar(255) NULL ,
[PageSize] int NOT NULL ,
[ImgWidth] int NOT NULL ,
[ImgHeight] int NOT NULL ,
[RssFeed] nvarchar(255) NULL ,
[Keywords] nvarchar(255) NULL ,
[Description] nvarchar(255) NULL ,
[Content] nvarchar(MAX) NULL ,
[ExtAttrib] nvarchar(MAX) NULL ,
[ExtModelId] int NOT NULL ,
[CategoryType] int NOT NULL ,
[ArticleTotal] int NOT NULL DEFAULT ((0)) 
)


GO
DBCC CHECKIDENT(N'[dbo].[GgcmsCategories]', RESEED, 25)
GO

-- ----------------------------
-- Records of GgcmsCategories
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsCategories] ON
GO
INSERT INTO [dbo].[GgcmsCategories] ([Id], [CategoryName], [OrderId], [LogoImg], [StyleName], [ParentId], [TmplName], [MobileTmplName], [ArticleTmplName], [ArticleMobileTmplName], [RedirectUrl], [PageSize], [ImgWidth], [ImgHeight], [RssFeed], [Keywords], [Description], [Content], [ExtAttrib], [ExtModelId], [CategoryType], [ArticleTotal]) VALUES (N'1', N'首页', N'0', N'', N'', N'0', N'', N'', N'', N'', N'/', N'0', N'0', N'0', N'', N'', N'', N'<p>dddddd</p>', N'', N'0', N'0', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsCategories] ([Id], [CategoryName], [OrderId], [LogoImg], [StyleName], [ParentId], [TmplName], [MobileTmplName], [ArticleTmplName], [ArticleMobileTmplName], [RedirectUrl], [PageSize], [ImgWidth], [ImgHeight], [RssFeed], [Keywords], [Description], [Content], [ExtAttrib], [ExtModelId], [CategoryType], [ArticleTotal]) VALUES (N'2', N'文档', N'2', N'http://localhost:54926/Content/Upload/temp/tmpE2B2.tmp.jpeg', N'default', N'0', N'list_1.cshtml', N'm_list_22.cshtml', N'view_1.cshtml', N'm_view_aaaaa.cshtml', N'', N'0', N'0', N'0', N'', N'', N'aaaaaaaaa', N'<p>bbbdsafdsafdsaf</p>', N'', N'0', N'0', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsCategories] ([Id], [CategoryName], [OrderId], [LogoImg], [StyleName], [ParentId], [TmplName], [MobileTmplName], [ArticleTmplName], [ArticleMobileTmplName], [RedirectUrl], [PageSize], [ImgWidth], [ImgHeight], [RssFeed], [Keywords], [Description], [Content], [ExtAttrib], [ExtModelId], [CategoryType], [ArticleTotal]) VALUES (N'3', N'关于', N'10', N'http://localhost:54926/Content/Upload/temp/tmp16D4.tmp.jpeg', N'default', N'0', N'list_1.cshtml', N'm_list_22.cshtml', N'view_1.cshtml', N'm_view_aaaaa.cshtml', N'', N'0', N'0', N'0', N'', N'', N'fdsdfdfdfsa', N'<p>safdsfdsafdsafds</p>', N'', N'0', N'0', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsCategories] ([Id], [CategoryName], [OrderId], [LogoImg], [StyleName], [ParentId], [TmplName], [MobileTmplName], [ArticleTmplName], [ArticleMobileTmplName], [RedirectUrl], [PageSize], [ImgWidth], [ImgHeight], [RssFeed], [Keywords], [Description], [Content], [ExtAttrib], [ExtModelId], [CategoryType], [ArticleTotal]) VALUES (N'4', N'博客', N'3', N'http://localhost:54926/Content/Upload/temp/tmp64B8.tmp.jpeg', N'default', N'0', N'list_blog.cshtml', N'm_list_22.cshtml', N'view_blog.cshtml', N'm_view_aaaaa.cshtml', N'', N'0', N'0', N'0', N'', N'', N'bbbbb', N'', N'', N'0', N'0', N'1')
GO
GO
INSERT INTO [dbo].[GgcmsCategories] ([Id], [CategoryName], [OrderId], [LogoImg], [StyleName], [ParentId], [TmplName], [MobileTmplName], [ArticleTmplName], [ArticleMobileTmplName], [RedirectUrl], [PageSize], [ImgWidth], [ImgHeight], [RssFeed], [Keywords], [Description], [Content], [ExtAttrib], [ExtModelId], [CategoryType], [ArticleTotal]) VALUES (N'18', N'动态', N'1', N'http://7xw2i7.com1.z0.glb.clouddn.com/tmp9347.tmp.jpeg', N'default', N'0', N'list_1.cshtml', N'm_list_22.cshtml', N'view_1.cshtml', N'm_view_aaaaa.cshtml', N'', N'0', N'0', N'0', N'', N'', N'动态摘要信息', N'<p><img src="http://7xw2i7.com1.z0.glb.clouddn.com/f01f3aac2eb89d541be9007e1164051201f91361232da-zMszpS_fw658-480x320.jpeg" style="width: 300px;" class="fr-fic fr-dib" data-code="0" data-msg="0" data-data="[object Object]"></p>', N'', N'0', N'0', N'1')
GO
GO
INSERT INTO [dbo].[GgcmsCategories] ([Id], [CategoryName], [OrderId], [LogoImg], [StyleName], [ParentId], [TmplName], [MobileTmplName], [ArticleTmplName], [ArticleMobileTmplName], [RedirectUrl], [PageSize], [ImgWidth], [ImgHeight], [RssFeed], [Keywords], [Description], [Content], [ExtAttrib], [ExtModelId], [CategoryType], [ArticleTotal]) VALUES (N'19', N'基础知识', N'4', N'', N'default', N'4', N'list_blog.cshtml', N'm_list_22.cshtml', N'view_blog.cshtml', N'm_view_aaaaa.cshtml', N'', N'0', N'0', N'0', N'', N'', N'', N'', N'', N'0', N'0', N'1')
GO
GO
INSERT INTO [dbo].[GgcmsCategories] ([Id], [CategoryName], [OrderId], [LogoImg], [StyleName], [ParentId], [TmplName], [MobileTmplName], [ArticleTmplName], [ArticleMobileTmplName], [RedirectUrl], [PageSize], [ImgWidth], [ImgHeight], [RssFeed], [Keywords], [Description], [Content], [ExtAttrib], [ExtModelId], [CategoryType], [ArticleTotal]) VALUES (N'20', N'后端开发', N'5', N'', N'', N'4', N'', N'', N'', N'', N'', N'0', N'0', N'0', N'', N'', N'', N'', N'', N'0', N'0', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsCategories] ([Id], [CategoryName], [OrderId], [LogoImg], [StyleName], [ParentId], [TmplName], [MobileTmplName], [ArticleTmplName], [ArticleMobileTmplName], [RedirectUrl], [PageSize], [ImgWidth], [ImgHeight], [RssFeed], [Keywords], [Description], [Content], [ExtAttrib], [ExtModelId], [CategoryType], [ArticleTotal]) VALUES (N'21', N'前端开发', N'6', N'', N'', N'4', N'', N'', N'', N'', N'', N'0', N'0', N'0', N'', N'', N'', N'', N'', N'0', N'0', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsCategories] ([Id], [CategoryName], [OrderId], [LogoImg], [StyleName], [ParentId], [TmplName], [MobileTmplName], [ArticleTmplName], [ArticleMobileTmplName], [RedirectUrl], [PageSize], [ImgWidth], [ImgHeight], [RssFeed], [Keywords], [Description], [Content], [ExtAttrib], [ExtModelId], [CategoryType], [ArticleTotal]) VALUES (N'22', N'数据库', N'7', N'', N'', N'4', N'', N'', N'', N'', N'', N'0', N'0', N'0', N'', N'', N'', N'', N'', N'0', N'0', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsCategories] ([Id], [CategoryName], [OrderId], [LogoImg], [StyleName], [ParentId], [TmplName], [MobileTmplName], [ArticleTmplName], [ArticleMobileTmplName], [RedirectUrl], [PageSize], [ImgWidth], [ImgHeight], [RssFeed], [Keywords], [Description], [Content], [ExtAttrib], [ExtModelId], [CategoryType], [ArticleTotal]) VALUES (N'23', N'移动开发', N'8', N'', N'default', N'4', N'list_blog.cshtml', N'm_list_22.cshtml', N'view_blog.cshtml', N'm_view_aaaaa.cshtml', N'', N'0', N'0', N'0', N'', N'', N'', N'', N'', N'0', N'0', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsCategories] ([Id], [CategoryName], [OrderId], [LogoImg], [StyleName], [ParentId], [TmplName], [MobileTmplName], [ArticleTmplName], [ArticleMobileTmplName], [RedirectUrl], [PageSize], [ImgWidth], [ImgHeight], [RssFeed], [Keywords], [Description], [Content], [ExtAttrib], [ExtModelId], [CategoryType], [ArticleTotal]) VALUES (N'24', N'UI设计', N'9', N'', N'default', N'4', N'list_blog.cshtml', N'm_list_22.cshtml', N'view_blog.cshtml', N'm_view_aaaaa.cshtml', N'', N'0', N'0', N'0', N'', N'', N'', N'', N'', N'0', N'0', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsCategories] ([Id], [CategoryName], [OrderId], [LogoImg], [StyleName], [ParentId], [TmplName], [MobileTmplName], [ArticleTmplName], [ArticleMobileTmplName], [RedirectUrl], [PageSize], [ImgWidth], [ImgHeight], [RssFeed], [Keywords], [Description], [Content], [ExtAttrib], [ExtModelId], [CategoryType], [ArticleTotal]) VALUES (N'25', N'开源', N'11', N'', N'', N'0', N'', N'', N'', N'', N'', N'0', N'0', N'0', N'', N'', N'', N'', N'', N'0', N'0', N'0')
GO
GO
SET IDENTITY_INSERT [dbo].[GgcmsCategories] OFF
GO

-- ----------------------------
-- Table structure for GgcmsDictionaries
-- ----------------------------
DROP TABLE [dbo].[GgcmsDictionaries]
GO
CREATE TABLE [dbo].[GgcmsDictionaries] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Title] nvarchar(100) NULL ,
[DictType] int NOT NULL ,
[OrderID] int NOT NULL ,
[SysFlag] int NOT NULL ,
[describe] nvarchar(255) NULL ,
[Value] nvarchar(100) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[GgcmsDictionaries]', RESEED, 9)
GO

-- ----------------------------
-- Records of GgcmsDictionaries
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsDictionaries] ON
GO
INSERT INTO [dbo].[GgcmsDictionaries] ([Id], [Title], [DictType], [OrderID], [SysFlag], [describe], [Value]) VALUES (N'2', N'爱好', N'1', N'1', N'1', N'个人爱好', null)
GO
GO
INSERT INTO [dbo].[GgcmsDictionaries] ([Id], [Title], [DictType], [OrderID], [SysFlag], [describe], [Value]) VALUES (N'3', N'特长', N'1', N'2', N'1', N'特长', null)
GO
GO
INSERT INTO [dbo].[GgcmsDictionaries] ([Id], [Title], [DictType], [OrderID], [SysFlag], [describe], [Value]) VALUES (N'4', N'学历', N'1', N'3', N'1', N'学历', null)
GO
GO
INSERT INTO [dbo].[GgcmsDictionaries] ([Id], [Title], [DictType], [OrderID], [SysFlag], [describe], [Value]) VALUES (N'6', N'本地上传', N'-1', N'1', N'1', N'本地文件上传', N'local')
GO
GO
INSERT INTO [dbo].[GgcmsDictionaries] ([Id], [Title], [DictType], [OrderID], [SysFlag], [describe], [Value]) VALUES (N'8', N'七牛上传', N'-1', N'2', N'1', N'七牛文件上传', N'qiniu')
GO
GO
SET IDENTITY_INSERT [dbo].[GgcmsDictionaries] OFF
GO

-- ----------------------------
-- Table structure for GgcmsFriendLinks
-- ----------------------------
DROP TABLE [dbo].[GgcmsFriendLinks]
GO
CREATE TABLE [dbo].[GgcmsFriendLinks] (
[Id] int NOT NULL IDENTITY(1,1) ,
[OrderId] int NOT NULL ,
[Url] nvarchar(255) NULL ,
[WebName] nvarchar(100) NULL ,
[LogoImg] nvarchar(255) NULL ,
[Status] int NOT NULL ,
[LinkType] int NOT NULL ,
[RelationId] int NOT NULL ,
[ExtAttrib] nvarchar(MAX) NULL 
)


GO

-- ----------------------------
-- Records of GgcmsFriendLinks
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsFriendLinks] ON
GO
SET IDENTITY_INSERT [dbo].[GgcmsFriendLinks] OFF
GO

-- ----------------------------
-- Table structure for GgcmsKeywords
-- ----------------------------
DROP TABLE [dbo].[GgcmsKeywords]
GO
CREATE TABLE [dbo].[GgcmsKeywords] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Keyword] nvarchar(50) NULL ,
[Url] nvarchar(255) NULL ,
[Describe] nvarchar(255) NULL ,
[Status] int NOT NULL 
)


GO

-- ----------------------------
-- Records of GgcmsKeywords
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsKeywords] ON
GO
SET IDENTITY_INSERT [dbo].[GgcmsKeywords] OFF
GO

-- ----------------------------
-- Table structure for GgcmsMembers
-- ----------------------------
DROP TABLE [dbo].[GgcmsMembers]
GO
CREATE TABLE [dbo].[GgcmsMembers] (
[Id] int NOT NULL IDENTITY(1,1) ,
[UserName] nvarchar(50) NULL ,
[PassWord] nvarchar(50) NULL ,
[Sex] bit NOT NULL ,
[Email] nvarchar(100) NULL ,
[Scores] int NOT NULL ,
[Avatar] nvarchar(255) NULL ,
[JoinTime] datetime NOT NULL ,
[Level] int NOT NULL ,
[Phone] nvarchar(20) NULL ,
[Roles_Id] int NOT NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[GgcmsMembers]', RESEED, 5)
GO

-- ----------------------------
-- Records of GgcmsMembers
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsMembers] ON
GO
INSERT INTO [dbo].[GgcmsMembers] ([Id], [UserName], [PassWord], [Sex], [Email], [Scores], [Avatar], [JoinTime], [Level], [Phone], [Roles_Id]) VALUES (N'5', N'admin', N'e10adc3949ba59abbe56e057f20f883e', N'0', null, N'0', null, N'2017-05-05 11:42:41.460', N'0', null, N'0')
GO
GO
SET IDENTITY_INSERT [dbo].[GgcmsMembers] OFF
GO

-- ----------------------------
-- Table structure for GgcmsModuleColumns
-- ----------------------------
DROP TABLE [dbo].[GgcmsModuleColumns]
GO
CREATE TABLE [dbo].[GgcmsModuleColumns] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ColName] nvarchar(50) NULL ,
[ColTitle] nvarchar(50) NULL ,
[ColType] nvarchar(50) NULL ,
[Length] int NOT NULL ,
[ColDecimal] int NOT NULL ,
[OrderId] int NOT NULL ,
[Options] nvarchar(MAX) NULL ,
[Module_Id] int NOT NULL 
)


GO

-- ----------------------------
-- Records of GgcmsModuleColumns
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsModuleColumns] ON
GO
SET IDENTITY_INSERT [dbo].[GgcmsModuleColumns] OFF
GO

-- ----------------------------
-- Table structure for GgcmsModules
-- ----------------------------
DROP TABLE [dbo].[GgcmsModules]
GO
CREATE TABLE [dbo].[GgcmsModules] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ModuleName] nvarchar(50) NULL ,
[Describe] nvarchar(255) NULL ,
[TableName] nvarchar(50) NULL ,
[ViewName] nvarchar(50) NULL 
)


GO

-- ----------------------------
-- Records of GgcmsModules
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsModules] ON
GO
SET IDENTITY_INSERT [dbo].[GgcmsModules] OFF
GO

-- ----------------------------
-- Table structure for GgcmsPowers
-- ----------------------------
DROP TABLE [dbo].[GgcmsPowers]
GO
CREATE TABLE [dbo].[GgcmsPowers] (
[Id] int NOT NULL IDENTITY(1,1) ,
[PowerName] nvarchar(50) NULL ,
[OrderId] int NOT NULL ,
[ParentId] int NOT NULL ,
[PowerTag] nvarchar(255) NULL ,
[IconClass] nvarchar(255) NULL ,
[ShowInSidebar] bit NOT NULL DEFAULT ((0)) ,
[PowerType] int NOT NULL DEFAULT ((0)) ,
[PowerParams] nvarchar(255) NULL ,
[Path] nvarchar(255) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[GgcmsPowers]', RESEED, 1033)
GO

-- ----------------------------
-- Records of GgcmsPowers
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsPowers] ON
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1', N'管理首页', N'1', N'0', N'home', N'icon-home', N'1', N'0', null, N'home')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'2', N'系统管理', N'2', N'0', N'system', N'icon-settings', N'1', N'0', null, null)
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'3', N'权限管理', N'21', N'2', N'power', N'icon-check', N'1', N'0', null, N'power')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'4', N'分类内容', N'4', N'0', N'contents', N'icon-grid', N'1', N'0', null, null)
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'5', N'风格模板', N'5', N'0', N'styleTemplate', N'icon-layers', N'1', N'0', null, N'styles')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'6', N'任务管理', N'6', N'0', N'tasks', N'icon-list', N'1', N'0', null, null)
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'7', N'网站设置', N'3', N'0', N'site', N'icon-globe', N'1', N'0', null, null)
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'8', N'分类导航', N'41', N'4', N'category', N'icon-directions', N'1', N'0', null, N'category')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'9', N'专题管理', N'42', N'4', N'topic', N'icon-puzzle', N'1', N'0', null, N'topic')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'10', N'文章管理', N'44', N'4', N'article', N'icon-note', N'1', N'0', null, N'article')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'11', N'基本信息', N'71', N'7', N'settings', N'icon-wrench', N'1', N'0', null, N'settings')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'12', N'广告管理', N'72', N'7', N'ads', N'icon-star', N'1', N'0', null, N'ads')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1011', N'系统工具', N'22', N'2', N'tools', N'icon-magic-wand', N'1', N'0', null, N'tools')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1012', N'文件管理', N'23', N'2', N'files', N'icon-folder-alt', N'1', N'0', null, N'files')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1013', N'用户管理', N'24', N'2', N'member', N'icon-people', N'1', N'0', null, N'member')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1014', N'角色管理', N'25', N'2', N'roles', N'icon-graduation', N'1', N'0', null, N'roles')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1015', N'系统字典', N'26', N'2', N'dictionary', N'icon-notebook', N'1', N'0', null, N'dictionary')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1016', N'单页文章', N'43', N'4', N'single', N'icon-doc', N'1', N'0', null, N'single')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1018', N'友情链接', N'73', N'7', N'links', N'icon-link', N'1', N'0', null, N'links')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1019', N'全站标签', N'74', N'7', N'tags', N'icon-tag', N'1', N'0', null, N'tags')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1020', N'SQL语句', N'62', N'6', N'sql', N'fa fa-code', N'1', N'0', null, N'sql')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1022', N'任务执行', N'61', N'6', N'runTask', N'icon-equalizer', N'1', N'0', null, N'runTask')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1023', N'采集规则', N'63', N'6', N'gather', N'icon-cloud-download', N'1', N'0', null, N'gather')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1024', N'代码片段', N'75', N'7', N'codes', N'fa fa-html5', N'1', N'0', null, N'codes')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1025', N'分类导航编辑', N'411', N'8', N'categoryEdit', null, N'0', N'0', null, N'categoryEdit')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1026', N'文章编辑', N'421', N'10', N'articleEdit', null, N'0', N'0', null, N'articleEdit')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1027', N'系统字典编辑', N'101501', N'1015', N'dictionaryEdit', null, N'0', N'0', null, N'dictionaryEdit')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1028', N'风格编辑', N'511', N'5', N'stylesEdit', null, N'0', N'0', null, N'stylesEdit')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1030', N'模板浏览', N'512', N'5', N'template', null, N'0', N'0', null, N'template')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1031', N'静态文件', N'513', N'5', N'staticFile', null, N'0', N'0', null, N'staticFile')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1032', N'静态文件编辑', N'514', N'5', N'staticFileEdit', null, N'0', N'0', null, N'staticFileEdit')
GO
GO
INSERT INTO [dbo].[GgcmsPowers] ([Id], [PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES (N'1033', N'模板编辑', N'515', N'5', N'templateEdit', null, N'0', N'0', null, N'templateEdit')
GO
GO
SET IDENTITY_INSERT [dbo].[GgcmsPowers] OFF
GO

-- ----------------------------
-- Table structure for GgcmsRolePowers
-- ----------------------------
DROP TABLE [dbo].[GgcmsRolePowers]
GO
CREATE TABLE [dbo].[GgcmsRolePowers] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Role_Id] int NOT NULL ,
[Power_Id] int NOT NULL 
)


GO

-- ----------------------------
-- Records of GgcmsRolePowers
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsRolePowers] ON
GO
SET IDENTITY_INSERT [dbo].[GgcmsRolePowers] OFF
GO

-- ----------------------------
-- Table structure for GgcmsStyles
-- ----------------------------
DROP TABLE [dbo].[GgcmsStyles]
GO
CREATE TABLE [dbo].[GgcmsStyles] (
[Id] int NOT NULL IDENTITY(1,1) ,
[StyleName] nvarchar(50) NULL ,
[Folder] nvarchar(50) NULL ,
[Descrip] nvarchar(255) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[GgcmsStyles]', RESEED, 8)
GO

-- ----------------------------
-- Records of GgcmsStyles
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsStyles] ON
GO
INSERT INTO [dbo].[GgcmsStyles] ([Id], [StyleName], [Folder], [Descrip]) VALUES (N'8', N'默认风格', N'default', N'default')
GO
GO
SET IDENTITY_INSERT [dbo].[GgcmsStyles] OFF
GO

-- ----------------------------
-- Table structure for GgcmsSysConfigs
-- ----------------------------
DROP TABLE [dbo].[GgcmsSysConfigs]
GO
CREATE TABLE [dbo].[GgcmsSysConfigs] (
[Id] int NOT NULL IDENTITY(1,1) ,
[CfgName] nvarchar(50) NULL ,
[CfgValue] nvarchar(MAX) NULL ,
[Descrip] nvarchar(255) NULL ,
[GroupId] int NOT NULL ,
[Options] nvarchar(MAX) NULL ,
[OrderId] int NOT NULL ,
[Protection] bit NOT NULL DEFAULT ((0)) 
)


GO
DBCC CHECKIDENT(N'[dbo].[GgcmsSysConfigs]', RESEED, 40)
GO

-- ----------------------------
-- Records of GgcmsSysConfigs
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsSysConfigs] ON
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'1', N'cfg_basehost', N'http://localhost:54926', N'站点根网址', N'1', N'{"type":"url"}', N'1', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'4', N'cfg_logo', N'http://7xw2i7.com1.z0.glb.clouddn.com/tmpB8CC.tmp.jpeg', N'logo上传', N'1', N'{"type":"file","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"ddddd","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","datasource":"","egroup":"","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":".png,.jpg,.gif","extensionMessage":"请上传扩展名为.png , .jpg , .gif的图片","fileSize":200,"fileSizenMessage":"文件不能超过100k","preview":true}', N'2', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'5', N'cfg_artkey_tmpl', N'aaaaaaaaaaaaaaaa', N'关键词替换模板', N'4', N'{"type":"text","required":true,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"aaaaa","requiredMessage":"bbbbbb","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"替换关键词{txt}，替换地址{url} 例:<a href=\"{url}\">{txt}</a>","preview":false,"datasource":"","egroup":"","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'6', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'6', N'cfg_uploadmode', N'local', N'存储方式', N'3', N'{"type":"select","required":true,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","datasource":"sysdict","egroup":"-1","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'1', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'7', N'cfg_indexname', N'首页', N'主页链接名', N'1', N'{"type":"email","required":false,"min":0,"max":0,"minLength":4,"maxLength":255,"pattern":"","message":"必须输入电子邮箱地址","requiredMessage":"不能不填","minMessage":"","maxMessage":"","minLengthMessage":"太短","maxLengthMessage":"太长","patternMessage":"","datasource":"","egroup":"","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":""}', N'5', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'10', N'cfg_indexurl', N'http://www.baidu.com', N'网页主页链接', N'1', N'', N'6', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'13', N'cfg_webname', N'GGCMS 码农记忆', N'网站名称', N'1', N'', N'8', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'14', N'cfg_powerby', N'<p>&copy; GGCMS 2010-2017. All Rights Reserved.</p><p><a href="http://www.miibeian.gov.cn/" target="_blank">豫ICP备17010644号-1</a></p>', N'网站版权信息', N'1', N'{
type:"simpleditor",

}', N'9', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'15', N'cfg_ddimg_width', N'0', N'标题图默认宽度', N'2', N'{
type:"number",
max:650,
min:80,
maxMessage:"太大了",
minMessage:"太小了"
}', N'1', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'17', N'cfg_ddimg_height', N'0', N'标题图默认高度', N'2', N'{
type:"number",
max:650,
min:80,
maxMessage:"太大了",
minMessage:"太小了"
}', N'2', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'18', N'cfg_default_style', N'default', N'默认风格', N'2', N'{"type":"select","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"style","egroup":"artRecommend","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'3', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'20', N'cfg_template_home', N'index_main.cshtml', N'首页模板', N'2', N'{"type":"select","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"template","egroup":"index","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'4', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'21', N'cfg_template_list', N'list_1.cshtml', N'栏目页模板', N'2', N'{"type":"select","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"template","egroup":"list","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'5', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'22', N'cfg_template_view', N'view_1.cshtml', N'文章页模板', N'2', N'{"type":"select","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"template","egroup":"view","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'6', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'23', N'cfg_mob_enable', N'False', N'是否启用移动端模板', N'2', N'{"type":"switch","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"","egroup":"","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'7', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'24', N'cfg_mob_flag', N'Iphone|iPod|Mobile|Android|Opera Mini|BlackBerry|webOS|UCWEB|Blazer|PSP', N'移动端识别标识', N'2', N'', N'8', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'25', N'cfg_template_m_home', N'm_index_main.cshtml', N'移动端首页模板', N'2', N'{"type":"select","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"template","egroup":"m_index","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'9', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'26', N'cfg_template_m_list', N'm_list_22.cshtml', N'移动端栏目页模板', N'2', N'{"type":"select","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"template","egroup":"m_list","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'10', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'27', N'cfg_template_m_view', N'm_view_aaaaa.cshtml', N'移动端文章页模板', N'2', N'{"type":"select","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"template","egroup":"m_view","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'11', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'29', N'cfg_access_key', N'', N'APP_ACCESS_KEY', N'3', N'', N'2', N'1')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'30', N'cfg_secret_key', N'', N'APP_SECRET_KEY', N'3', N'{"type":"password","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","datasource":"","egroup":"","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'3', N'1')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'31', N'cfg_bucket', N'ggcms', N'上传空间(bucket)', N'3', N'', N'4', N'1')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'33', N'cfg_link_template', N'', N'链接格式', N'3', N'{"type":"text","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"链接模板，替换文件名{fn}，例:http://www.ggcms.com/img/{fn}","datasource":"","egroup":"","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'5', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'34', N'cfg_cache_enable', N'True', N'是否启用缓存', N'4', N'{"type":"switch","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"","egroup":"","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'1', N'1')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'35', N'cfg_cache_dir', N'', N'缓存页面保存路径', N'4', N'', N'2', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'36', N'cfg_cache_timeout', N'', N'缓存超时(分钟)', N'4', N'{"type":"number","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"","egroup":"","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'3', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'37', N'cfg_artkey_enable', N'True', N'是否启用站内关键词', N'4', N'{"type":"switch","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"","egroup":"","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'4', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'38', N'cfg_artkey_rn', N'', N'每篇文章替换几个', N'4', N'{"type":"number","required":false,"min":0,"max":0,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"","egroup":"","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'5', N'0')
GO
GO
INSERT INTO [dbo].[GgcmsSysConfigs] ([Id], [CfgName], [CfgValue], [Descrip], [GroupId], [Options], [OrderId], [Protection]) VALUES (N'40', N'cfg_page_size', N'10', N'每页显示记录数', N'3', N'{"type":"number","required":false,"min":1,"max":100,"minLength":0,"maxLength":0,"pattern":"","message":"","requiredMessage":"","minMessage":"","maxMessage":"","minLengthMessage":"","maxLengthMessage":"","patternMessage":"","helpMessage":"","preview":false,"datasource":"","egroup":"","multiple":false,"onColor":"info","offColor":"default","onText":"ON","offText":"OFF","minDate":"","minDateMessage":"","maxDate":"","maxDateMessage":"","extension":"","extensionMessage":"","fileSize":0,"fileSizenMessage":""}', N'0', N'0')
GO
GO
SET IDENTITY_INSERT [dbo].[GgcmsSysConfigs] OFF
GO

-- ----------------------------
-- Table structure for GgcmsTasks
-- ----------------------------
DROP TABLE [dbo].[GgcmsTasks]
GO
CREATE TABLE [dbo].[GgcmsTasks] (
[Id] int NOT NULL IDENTITY(1,1) ,
[TaskName] nvarchar(50) NULL ,
[TaskType] int NOT NULL ,
[TaskConfigs] nvarchar(MAX) NULL ,
[Status] int NOT NULL ,
[Switch] int NOT NULL ,
[PlanType] int NOT NULL ,
[RunTime] datetime NOT NULL ,
[PlanOptions] nvarchar(MAX) NULL 
)


GO

-- ----------------------------
-- Records of GgcmsTasks
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsTasks] ON
GO
SET IDENTITY_INSERT [dbo].[GgcmsTasks] OFF
GO

-- ----------------------------
-- Table structure for GgcmsTopics
-- ----------------------------
DROP TABLE [dbo].[GgcmsTopics]
GO
CREATE TABLE [dbo].[GgcmsTopics] (
[Id] int NOT NULL IDENTITY(1,1) ,
[TopicName] nvarchar(255) NULL ,
[CreateTime] datetime NOT NULL ,
[Content] ntext NULL ,
[PageSize] int NOT NULL ,
[TmplName] nvarchar(50) NULL ,
[MobileTmplName] nvarchar(50) NULL ,
[Title] nvarchar(255) NULL ,
[Description] nvarchar(255) NULL ,
[Keywords] nvarchar(255) NULL ,
[LogoImg] nvarchar(255) NULL ,
[RedirectUrl] nvarchar(255) NULL ,
[StyleName] nvarchar(50) NULL ,
[ExtAttrib] nvarchar(MAX) NULL ,
[TopicIds] nvarchar(MAX) NULL 
)


GO

-- ----------------------------
-- Records of GgcmsTopics
-- ----------------------------
SET IDENTITY_INSERT [dbo].[GgcmsTopics] ON
GO
SET IDENTITY_INSERT [dbo].[GgcmsTopics] OFF
GO

-- ----------------------------
-- Indexes structure for table GgcmsArticlePages
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsArticlePages
-- ----------------------------
ALTER TABLE [dbo].[GgcmsArticlePages] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsArticles
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsArticles
-- ----------------------------
ALTER TABLE [dbo].[GgcmsArticles] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsAttachments
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsAttachments
-- ----------------------------
ALTER TABLE [dbo].[GgcmsAttachments] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsCategories
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsCategories
-- ----------------------------
ALTER TABLE [dbo].[GgcmsCategories] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsDictionaries
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsDictionaries
-- ----------------------------
ALTER TABLE [dbo].[GgcmsDictionaries] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsFriendLinks
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsFriendLinks
-- ----------------------------
ALTER TABLE [dbo].[GgcmsFriendLinks] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsKeywords
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsKeywords
-- ----------------------------
ALTER TABLE [dbo].[GgcmsKeywords] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsMembers
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsMembers
-- ----------------------------
ALTER TABLE [dbo].[GgcmsMembers] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsModuleColumns
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsModuleColumns
-- ----------------------------
ALTER TABLE [dbo].[GgcmsModuleColumns] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsModules
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsModules
-- ----------------------------
ALTER TABLE [dbo].[GgcmsModules] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsPowers
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsPowers
-- ----------------------------
ALTER TABLE [dbo].[GgcmsPowers] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsRolePowers
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsRolePowers
-- ----------------------------
ALTER TABLE [dbo].[GgcmsRolePowers] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsStyles
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsStyles
-- ----------------------------
ALTER TABLE [dbo].[GgcmsStyles] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsSysConfigs
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsSysConfigs
-- ----------------------------
ALTER TABLE [dbo].[GgcmsSysConfigs] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsTasks
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsTasks
-- ----------------------------
ALTER TABLE [dbo].[GgcmsTasks] ADD PRIMARY KEY ([Id])
GO

-- ----------------------------
-- Indexes structure for table GgcmsTopics
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsTopics
-- ----------------------------
ALTER TABLE [dbo].[GgcmsTopics] ADD PRIMARY KEY ([Id])
GO
