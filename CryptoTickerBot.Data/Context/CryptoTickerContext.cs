using CryptoTickerBot.Data.Domain.CryptoTicker;
using Microsoft.EntityFrameworkCore;
using System;



namespace CryptoTickerBot.Data
{
    public partial class CryptoTickerContext : DbContext
    {
        public DbSet<CryptoTicker> CryptoTickerHistory { get; set; }

        public CryptoTickerContext()
        {
        }

        public CryptoTickerContext(DbContextOptions<CryptoTickerContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CryptoTicker>(
                entity =>
                {
                    entity.HasKey(t => t.TickerHistoryId);
                    entity.Property(t => t.TickerHistoryId).HasColumnName("ticker_history_id");
                    entity.Property(t => t.Ticker).HasColumnName("ticker");
                    entity.Property(t => t.Price).HasColumnName("price").HasColumnType("decimal(24,8)");
                    entity.Property(t => t.KlinesName).HasColumnName("klines_name");
                    entity.Property(t => t.TickerDate).HasColumnName("ticker_date").HasColumnType("datetime");
                    entity.Property(t => t.EMA7).HasColumnName("EMA7");
                    entity.Property(t => t.EMA25).HasColumnName("EMA25");
                    entity.Property(t => t.EMA99).HasColumnName("EMA99");
                    entity.ToTable("ticker_history");
                });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
