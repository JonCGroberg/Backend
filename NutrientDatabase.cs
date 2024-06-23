using Microsoft.EntityFrameworkCore;

class NutrientDatabase : DbContext
{
    public DbSet<Nutrient> Nutrients { get; set; }

    public NutrientDatabase(DbContextOptions<NutrientDatabase> options) : base(options) { }
}