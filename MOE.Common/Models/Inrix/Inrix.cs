namespace MOE.Common.Models.Inrix
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Inrix : DbContext
    {
        public Inrix()
            : base("name=Inrix")
        {
        }

        public virtual DbSet<Datum> Data { get; set; }
        public virtual DbSet<Group_Members> Group_Members { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Route_Members> Route_Members { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<Segment_Members> Segment_Members { get; set; }
        public virtual DbSet<Segment> Segments { get; set; }
        public virtual DbSet<TMC> TMCs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Datum>()
                .Property(e => e.tmc_code)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.Group_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.Group_Description)
                .IsUnicode(false);

            modelBuilder.Entity<Route_Members>()
                .Property(e => e.Segment_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Route>()
                .Property(e => e.Route_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Route>()
                .Property(e => e.Route_Description)
                .IsUnicode(false);

            modelBuilder.Entity<Segment_Members>()
                .Property(e => e.TMC)
                .IsUnicode(false);

            modelBuilder.Entity<Segment>()
                .Property(e => e.Segment_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Segment>()
                .Property(e => e.Segment_Description)
                .IsUnicode(false);

            modelBuilder.Entity<TMC>()
                .Property(e => e.TMC1)
                .IsUnicode(false);

            modelBuilder.Entity<TMC>()
                .Property(e => e.Direction)
                .IsUnicode(false);

            modelBuilder.Entity<TMC>()
                .Property(e => e.TMC_Start)
                .IsUnicode(false);

            modelBuilder.Entity<TMC>()
                .Property(e => e.TMC_Stop)
                .IsUnicode(false);

            modelBuilder.Entity<TMC>()
                .Property(e => e.Length)
                .HasPrecision(10, 3);

            modelBuilder.Entity<TMC>()
                .Property(e => e.Street)
                .IsUnicode(false);
        }
    }
}
