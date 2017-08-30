/*
修改友情链接表LinkType为字符型
*/
ALTER TABLE [dbo].[GgcmsFriendLinks] ALTER COLUMN [LinkType] nvarchar(100) 
GO

/*
插入友情链接分类字典
*/
INSERT INTO [dbo].[GgcmsDictionaries] ([Title],[Value],[DictType],[OrderID],[SysFlag],[describe]) VALUES('友情链接类型','link_type',1,5,1,'友情链接类型')
GO

--修改字典类型DictType为字符型
ALTER TABLE [dbo].[GgcmsDictionaries] ALTER COLUMN [DictType] nvarchar(100) 
GO
