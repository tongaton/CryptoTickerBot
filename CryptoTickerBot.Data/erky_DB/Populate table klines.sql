USE [Erky]
GO

INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES (1, '1m');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '3m');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '5m');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '15m');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '30m');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '1h');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '2h');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '4h');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '6h');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '8h');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '12h');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '1d');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '3d');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '1w');
INSERT INTO [dbo].[klines] ([klines_id],[klines_name]) VALUES ((SELECT MAX([klines_id]) FROM [klines]) + 1, '1M');

GO

