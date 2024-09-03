using Boxes.Infrastructure;

namespace Boxes.API.Infrastructure
{
    public class BoxesContextSeed : IDbSeeder<BoxesContext>
    {
        public Task SeedAsync(BoxesContext context)
        {
            return Task.CompletedTask;
        }
    }
}
