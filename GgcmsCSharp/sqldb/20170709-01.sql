/*
添加修改密码权限菜单
*/

INSERT INTO [dbo].[GgcmsPowers] ([PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES ( N'清理缓存', N'28', N'2', N'clearCache', null, N'0', N'0', null, N'clearCache')
GO
INSERT INTO [dbo].[GgcmsPowers] ([PowerName], [OrderId], [ParentId], [PowerTag], [IconClass], [ShowInSidebar], [PowerType], [PowerParams], [Path]) VALUES ( N'应用重启', N'29', N'2', N'appRestart', null, N'0', N'0', null, N'appRestart')
GO