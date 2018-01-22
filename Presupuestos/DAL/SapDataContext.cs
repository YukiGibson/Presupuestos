namespace Presupuestos.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Presupuestos.Models;

    public partial class SapDataContext : DbContext
    {
        public SapDataContext()
            : base("name=SapDataContext")
        {
        }

        public virtual DbSet<OITM> OITM { get; set; }
        public virtual DbSet<OCRD> OCRD { get; set; }
        public virtual DbSet<OSLP> OSLPs { get; set; }
        public virtual DbSet<OWOR> OWOR { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OITM>()
                .Property(e => e.VATLiable)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.PrchseItem)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SellItem)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.InvntItem)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.OnHand)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.IsCommited)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.OnOrder)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.MaxLevel)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.NumInBuy)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.ReorderQty)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.MinLevel)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.LstEvlPric)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.CustomPer)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.Canceled)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.WholSlsTax)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.RetilrTax)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SpcialDisc)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.TrackSales)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.NumInSale)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.Consig)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.Counted)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.OpenBlnc)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.EvalSystem)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.FREE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.Transfered)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.BlncTrnsfr)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.CommisPcnt)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.CommisSum)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.TreeType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.TreeQty)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.LastPurPrc)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.ExitPrice)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.AssetItem)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.WasCounted)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.ManSerNum)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SHeight1)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SHeight2)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SWidth1)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SWidth2)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SLength1)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.Slength2)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SVolume)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SWeight1)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SWeight2)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.BHeight1)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.BHeight2)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.BWidth1)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.BWidth2)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.BLength1)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.Blength2)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.BVolume)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.BWeight1)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.BWeight2)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup4)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup5)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup6)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup7)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup8)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup9)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup10)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup11)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup12)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup13)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup14)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup15)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup16)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup17)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup18)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup19)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup20)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup21)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup22)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup23)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup24)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup25)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup26)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup27)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup28)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup29)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup30)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup31)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup32)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup33)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup34)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup35)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup36)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup37)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup38)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup39)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup40)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup41)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup42)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup43)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup44)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup45)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup46)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup47)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup48)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup49)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup50)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup51)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup52)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup53)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup54)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup55)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup56)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup57)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup58)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup59)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup60)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup61)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup62)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup63)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.QryGroup64)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SalFactor1)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SalFactor2)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SalFactor3)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SalFactor4)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.PurFactor1)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.PurFactor2)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.PurFactor3)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.PurFactor4)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.AvgPrice)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.PurPackUn)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.SalPackUn)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.ManBtchNum)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.ManOutOnly)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.DataSource)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.validFor)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.frozenFor)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.BlockOut)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.Deleted)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.GLMethod)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.TaxType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.ByWh)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.WTLiable)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.ItemType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.StockValue)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.Phantom)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.IssueMthd)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.FREE1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.PricingPrc)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.MngMethod)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.ReorderPnt)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.PlaningSys)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.PrcrmntMtd)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.OrdrMulti)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.MinOrdrQty)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.IndirctTax)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.ProductSrc)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.ItemClass)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.Excisable)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OITM>()
                .Property(e => e.AssblValue)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.U_Ancho_Plg)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.U_Largo_Plg)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OITM>()
                .Property(e => e.U_Gramaje)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OSLP>()
                .Property(e => e.Commission)
                .HasPrecision(19, 6);

            modelBuilder.Entity<OSLP>()
                .Property(e => e.Locked)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSLP>()
                .Property(e => e.DataSource)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
