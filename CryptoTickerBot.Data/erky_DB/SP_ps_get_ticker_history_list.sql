  
/****************************************************************************************************************  
*Procedure  Name: ps_get_ticker_history_list
*Database: Erky  
*  
*Description: Get History of Ticket/kline by date
*20241028 (GF): Creaci�n  
*****************************************************************************************************************/  

CREATE OR ALTER PROCEDURE [dbo].[ps_get_ticker_history_list] 
    @date_from datetime,
    @date_to datetime,
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
	WHERE ticker_date BETWEEN @date_from AND @date_to
      AND ticker = @ticker
      AND k.klines_name = @klines
	ORDER BY th.ticker_date	;

END;