SELECT TOP (1000) [ticker_history_id]
      ,[ticker]
      ,[price]
      ,[klines_name]
      ,[ticker_date]
      ,[EMA7]
      ,[EMA25]
      ,[EMA99]
  FROM [Erky].[dbo].[ticker_history] th
	INNER JOIN [dbo].[klines] k ON k.klines_id = th.klines_id

--	Retrieve list of ticker by date
--	EXEC [ps_get_ticker_history] "20240101", "20241028", "BTC", "1d"

  /*

  USE [Erky]
GO

INSERT INTO [dbo].[ticker_history]
           ([ticker]
           ,[price]
           ,[klines_id]
           ,[ticker_date]
		   ,EMA7
		   ,EMA25
		   ,EMA99)
     VALUES
           ('BTC'
		   ,64858
           ,12	-- | 1d:12 | 4h:8 | 1h:6 | 15m:4
           ,'2024/09/28 00:00:00'
		   ,0
		   ,0
		   ,0)

INSERT INTO [dbo].[ticker_history]
        ([ticker]
        ,[price]
        ,[klines_id]
        ,[ticker_date]
		,EMA7
		,EMA25
		,EMA99)
    VALUES
        ('BTC'
		,65602.01000 
        ,12	-- | 1d:12 | 4h:8 | 1h:6 | 15m:4
        ,'2024/09/29 00:00:00'
		,0
		,0
		,0)

INSERT INTO [dbo].[ticker_history]
        ([ticker]
        ,[price]
        ,[klines_id]
        ,[ticker_date]
		,EMA7
		,EMA25
		,EMA99)
    VALUES
        ('BTC'
		,68428
        ,12	-- | 1d:12 | 4h:8 | 1h:6 | 15m:4
        ,'2024/10/18 00:00:00'
		,0
		,0
		,0)



GO


  */