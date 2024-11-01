  
/****************************************************************************************************************  
*Procedure  Name: ps_get_ticker_history_list
*Database: Erky  
*  
*Description: Get History of Ticket/kline by TIMESTAMP
*20241029 (GF): Creaci�n  
*****************************************************************************************************************/  

CREATE OR ALTER PROCEDURE [dbo].[ps_get_ticker_history] 
    @timestamp datetime,
    @ticker varchar(10),
    @klines varchar(10)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [ticker_history_id]
      ,[ticker]
      ,[price]
      ,[klines_name]
      ,[ticker_date]
      ,[EMA7]
      ,[EMA25]
	  ,[EMA99]
    FROM ticker_history th
		INNER JOIN [dbo].[klines] k ON k.klines_id = th.klines_id
	WHERE ticker_date = @timestamp
      AND ticker = @ticker
      AND k.klines_name = @klines;

END;