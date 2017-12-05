namespace Tests.Integration.Mvc.EntityFrameworkClasses
{
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using TestData.Models;

    public class AppContext : DbContext
    {
        public AppContext()
            : base("DefaultConnection")
        {
        }

        public IDbSet<AllTypesModel> AllTypesModels { get; set; }

        internal void Seed(IQueryable<AllTypesModel> data, int saveEach = 100)
        {
            var allCount = data.Count();
            var counter = 0;
            foreach (var item in data)
            {
                this.AllTypesModels.Add(item);
                counter++;
                if (counter % saveEach == 0)
                {
                    this.SaveChanges();
                    Trace.WriteLine($"{counter} of {allCount} items added to sql database.");
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AllTypesModel>().Property(x => x.DecimalProperty).HasPrecision(16, 16);

            base.OnModelCreating(modelBuilder);
        }
    }
}