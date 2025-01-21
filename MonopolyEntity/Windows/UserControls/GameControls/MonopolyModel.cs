using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MonopolyEntity.Windows.UserControls.GameControls
{
    public partial class MonopolyModel : DbContext
    {
        public MonopolyModel()
            : base("name=MonopolyModel")
        {
        }

        public virtual DbSet<Board> Boards { get; set; }
        public virtual DbSet<BoxItem> BoxItems { get; set; }
        public virtual DbSet<BusinessToGive> BusinessToGives { get; set; }
        public virtual DbSet<Casino> Casinos { get; set; }
        public virtual DbSet<CasinoType> CasinoTypes { get; set; }
        public virtual DbSet<Cell> Cells { get; set; }
        public virtual DbSet<CellType> CellTypes { get; set; }
        public virtual DbSet<Chance> Chances { get; set; }
        public virtual DbSet<ChanceMoney> ChanceMoneys { get; set; }
        public virtual DbSet<ChanceMove> ChanceMoves { get; set; }
        public virtual DbSet<ChanceType> ChanceTypes { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<InventoryStaff> InventoryStaffs { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<LotBox> LotBoxes { get; set; }
        public virtual DbSet<PictureFile> PictureFiles { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<PlayerGame> PlayerGames { get; set; }
        public virtual DbSet<PriceForLevel> PriceForLevels { get; set; }
        public virtual DbSet<PriceType> PriceTypes { get; set; }
        public virtual DbSet<Rearity> Rearities { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<StationType> StationTypes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<SystemColor> SystemColors { get; set; }
        public virtual DbSet<Tax> Taxes { get; set; }
        public virtual DbSet<TaxType> TaxTypes { get; set; }
        public virtual DbSet<Trade> Trades { get; set; }
        public virtual DbSet<TradeAttrib> TradeAttribs { get; set; }
        public virtual DbSet<TradeStatu> TradeStatus { get; set; }
        public virtual DbSet<WonLevel> WonLevels { get; set; }
        public virtual DbSet<WonPlayer> WonPlayers { get; set; }
        public virtual DbSet<WonTablesInCasino> WonTablesInCasinoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CasinoType>()
                .HasMany(e => e.WonTablesInCasinoes)
                .WithOptional(e => e.CasinoType)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<CellType>()
                .HasMany(e => e.Cells)
                .WithOptional(e => e.CellType1)
                .HasForeignKey(e => e.CellType);

            modelBuilder.Entity<ChanceType>()
                .HasMany(e => e.Chances)
                .WithOptional(e => e.ChanceType)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.InventoryStaffs)
                .WithOptional(e => e.Item)
                .HasForeignKey(e => e.StaffId);

            modelBuilder.Entity<LotBox>()
                .HasMany(e => e.BoxItems)
                .WithOptional(e => e.LotBox)
                .HasForeignKey(e => e.BoxId);

            modelBuilder.Entity<PictureFile>()
                .HasMany(e => e.BoxItems)
                .WithOptional(e => e.PictureFile)
                .HasForeignKey(e => e.PicId);

            modelBuilder.Entity<PictureFile>()
                .HasMany(e => e.Cells)
                .WithOptional(e => e.PictureFile)
                .HasForeignKey(e => e.PicId);

            modelBuilder.Entity<PictureFile>()
                .HasMany(e => e.LotBoxes)
                .WithOptional(e => e.PictureFile)
                .HasForeignKey(e => e.PicId);

            modelBuilder.Entity<PictureFile>()
                .HasMany(e => e.Players)
                .WithOptional(e => e.PictureFile)
                .HasForeignKey(e => e.PictureId);

            modelBuilder.Entity<PlayerGame>()
                .HasMany(e => e.Stations)
                .WithOptional(e => e.PlayerGame)
                .HasForeignKey(e => e.OwnerId);

            modelBuilder.Entity<PlayerGame>()
                .HasMany(e => e.Trades)
                .WithOptional(e => e.PlayerGame)
                .HasForeignKey(e => e.ReciverId);

            modelBuilder.Entity<PlayerGame>()
                .HasMany(e => e.Trades1)
                .WithOptional(e => e.PlayerGame1)
                .HasForeignKey(e => e.SenderId);

            modelBuilder.Entity<PlayerGame>()
                .HasMany(e => e.TradeAttribs)
                .WithOptional(e => e.PlayerGame)
                .HasForeignKey(e => e.PlayerId);

            modelBuilder.Entity<PlayerGame>()
                .HasMany(e => e.WonPlayers)
                .WithOptional(e => e.PlayerGame)
                .HasForeignKey(e => e.PlayerId);

            modelBuilder.Entity<PriceType>()
                .HasMany(e => e.PriceForLevels)
                .WithOptional(e => e.PriceType)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<StationType>()
                .HasMany(e => e.Stations)
                .WithOptional(e => e.StationType)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<SystemColor>()
                .HasMany(e => e.Rearities)
                .WithOptional(e => e.SystemColor)
                .HasForeignKey(e => e.ColorId);

            modelBuilder.Entity<TaxType>()
                .HasMany(e => e.Taxes)
                .WithOptional(e => e.TaxType)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<TradeStatu>()
                .HasMany(e => e.Trades)
                .WithOptional(e => e.TradeStatu)
                .HasForeignKey(e => e.StatusId);
        }
    }
}
