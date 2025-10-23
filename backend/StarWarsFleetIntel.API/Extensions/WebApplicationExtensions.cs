using Serilog;
using StarWarsFleetIntel.API.Middleware;

namespace StarWarsFleetIntel.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseCors("AllowFrontends");
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
