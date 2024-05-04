namespace Talabat.APIs.Helpers.Extensions
{
    public static class SwaggerServices
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();
            return Services;
        }
        public static WebApplication UseSwaggerMiddleWares(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
