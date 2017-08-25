namespace WavetronicsSpeedListener.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SpeedListenerModel : DbContext
    {
        public SpeedListenerModel()
            : base("name=SpeedListenerModel")
        {
        }

        public virtual DbSet<Speed_Events> Speed_Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
