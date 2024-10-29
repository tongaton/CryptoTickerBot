USE [Erky]
GO

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
	
	WHERE th.ticker = 'xrp'

	ALTER TABLE ticker_history
ALTER COLUMN ticker_date DATETIME;


  /*
	--		Retrieve ticker price from a timestamp
	INSERT INTO #TempTicker
	EXEC [ps_get_ticker_history] '2024/10/27 16:00:00', 'XRP', '4h'

	--		Retrieve list of ticker by date
	EXEC [ps_get_ticker_history_list] "20240101", "20241028", "BTC", "1d"
	EXEC [ps_get_ticker_history_list] "20240101", "20241028", "BTC", "4h"
	EXEC [ps_get_ticker_history_list] "20240101", "20241028", "BTC", "1h"
	EXEC [ps_get_ticker_history_list] "20240101", "20241028", "BTC", "15m"
	EXEC [ps_get_ticker_history_list] "20240101", "20241028", "XRP", "1d"
	EXEC [ps_get_ticker_history_list] "20240101", "20241028", "XRP", "4h"
	EXEC [ps_get_ticker_history_list] "20241026 00:00:00", "20241026 23:59:59", "XRP", "4h"
	EXEC [ps_get_ticker_history_list] "20240101", "20241028", "XRP", "1h"
	EXEC [ps_get_ticker_history_list] "20240101", "20241028", "XRP", "15m"
	--		Insert new values
	EXEC [pui_ticker_history] '2024/10/29 00:00:00', 'XRP', '1d', 0.5192
	EXEC [pui_ticker_history] '2024/10/28 00:00:00', 'XRP', '1d', 0.5195
	EXEC [pui_ticker_history] '2024/10/26 00:00:00', 'XRP', '1d', 0.5168
	EXEC [pui_ticker_history] '2024/10/26 00:00:00', 'XRP', '4h', 0.5191
	EXEC [pui_ticker_history] '2024/10/27 20:00:00', 'XRP', '4h', 0.5195
	EXEC [pui_ticker_history] '2024/10/27 16:00:00', 'XRP', '4h', 0.5176
	EXEC [pui_ticker_history] '2024/10/27 12:00:00', 'XRP', '4h', 0.5139
	EXEC [pui_ticker_history] '2024/10/27 08:00:00', 'XRP', '4h', 0.5121
	EXEC [pui_ticker_history] '2024/10/26 05:00:00', 'XRP', '4h', 0.5141
  */
