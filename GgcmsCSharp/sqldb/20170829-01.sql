/*
�½�����
*/


-- ----------------------------
-- Table structure for GgcmsAdverts
-- ----------------------------

CREATE TABLE [dbo].[GgcmsAdverts] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Title] nvarchar(255) NULL ,
[Url] nvarchar(255) NULL ,
[Image] nvarchar(255) NULL ,
[GroupKey] nvarchar(100) NULL ,
[Content] nvarchar(MAX) NULL ,
[OrderID] int NOT NULL ,
[Status] int NOT NULL ,
[Describe] nvarchar(255) NULL 
)


GO

-- ----------------------------
-- Indexes structure for table GgcmsAdverts
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table GgcmsAdverts
-- ----------------------------
ALTER TABLE [dbo].[GgcmsAdverts] ADD PRIMARY KEY ([Id])
GO
/*
����������ֵ�
*/
INSERT INTO [dbo].[GgcmsDictionaries] ([Title],[Value],[DictType],[OrderID],[SysFlag],[describe]) VALUES('������','ads_group',1,4,1,'������')
GO