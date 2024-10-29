  
/****************************************************************************************************************  
*Procedure  Name: pui_ticker_history  
*Database: Erky  
*  
*Description: Insert or Update Ticket/kline for timestamp
*20241029 (GF): Creación  
*****************************************************************************************************************/  

USE Erky
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[pui_ticker_history]
    @timestamp datetime,
    @ticker varchar(10),
    @klines varchar(10),
	@price decimal(24,8)
AS
BEGIN

	SET NOCOUNT ON;

	--	Consulto primero antes de insertar para no duplicar valores
	SELECT TOP 1 [ticker_history_id]
		INTO #TempTicker
		FROM [Erky].[dbo].[ticker_history] th 
			INNER JOIN [dbo].[klines] k ON k.klines_id = th.klines_id
			WHERE th.ticker = @ticker
				AND th.ticker_date = @timestamp
				AND k.klines_name = @klines

	
	IF EXISTS (SELECT 1 FROM #TempTicker)
	BEGIN
		SELECT 0 as [ticker_history_id], 'The price has already recorded for the klines.' as Detail;
	END
	ELSE
	BEGIN
			BEGIN try

				INSERT INTO [dbo].[ticker_history]
					([ticker]
					,[price]
					,[klines_id]
					,[ticker_date]
					,EMA7
					,EMA25
					,EMA99)
				VALUES
					(@ticker
					,@price
					,(SELECT klines_id FROM dbo.[klines] WHERE klines_name = @klines)
					,@timestamp
					,0
					,0
					,0)

				-- Devuelvo la última agregada
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
				WHERE [ticker_history_id] = @@IDENTITY

				END try
	
				BEGIN catch
						declare	@ErrorMessage nvarchar(4000), @ErrorSeverity int, @ErrorState int
					select	@ErrorMessage = error_message(), @ErrorSeverity = error_severity(), @ErrorState = error_state()
					raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState)
				END catch

	END;




END;