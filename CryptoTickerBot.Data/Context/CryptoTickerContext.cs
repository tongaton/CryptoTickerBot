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
            modelBuilder.Entity<CryptoTicker>(entity =>
            {
                entity.HasNoKey();
            });




            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
