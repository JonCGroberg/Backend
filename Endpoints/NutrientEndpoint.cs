using Microsoft.EntityFrameworkCore;

public static class NutrientEndpointExtensions
{
    public static void RegisterNutrientEndpoint(this WebApplication app)
    {
        var nutrientEndPoint = app.MapGroup("/nutrients").RequireAuthorization().WithTags("Nutrients");

        nutrientEndPoint.MapGet("/", GetAllNutrients);
        nutrientEndPoint.MapGet("/{id}", GetNutrient);
        nutrientEndPoint.MapPost("/", CreateNutrient);
        nutrientEndPoint.MapPut("/{id}", UpdateNutrient);
        nutrientEndPoint.MapDelete("/{id}", DeleteNutrient);

        async Task<IResult> GetAllNutrients(NutrientDatabase db) => TypedResults.Ok(await db.Nutrients.ToListAsync());
        async Task<IResult> GetNutrient(NutrientDatabase db, int id) =>
            await db.Nutrients.FindAsync(id)
                is Nutrient nutrient
                    ? TypedResults.Ok(nutrient)
                    : TypedResults.NotFound();
        async Task<IResult> CreateNutrient(NutrientDatabase db, Nutrient nutrient)
        {
            db.Nutrients.Add(nutrient);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/{nutrient.Id}", nutrient);
        }
        async Task<IResult> UpdateNutrient(NutrientDatabase db, int id, Nutrient nutrient)
        {
            if (id != nutrient.Id) return Results.BadRequest("Id mismatch");
            db.Nutrients.Update(nutrient);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }
        async Task<IResult> DeleteNutrient(NutrientDatabase db, int id)
        {
            var nutrient = await db.Nutrients.FindAsync(id);
            if (nutrient == null) return Results.NotFound();
            db.Nutrients.Remove(nutrient);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

    }
}