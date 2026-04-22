namespace Musicality.Configurations;

public static class SwaggerApplicationBuilderExtensions
{
    public static WebApplication UseMusicalitySwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}
