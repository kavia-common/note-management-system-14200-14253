using Microsoft.AspNetCore.Mvc;
using NotesAppBackend.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Controllers and JSON options
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Return validation problems consistently
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());
            return new BadRequestObjectResult(new { message = "Validation failed", errors });
        };
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Db, repositories, services
builder.Services.AddAppDb(builder.Configuration, builder.Environment);
builder.Services.AddRepositories();
builder.Services.AddAppServices();
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddSwaggerWithJwt();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseRouting();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Swagger UI at /docs
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notes App API v1");
    c.RoutePrefix = "docs";
});

// Root health check
app.MapGet("/", () => new { message = "Healthy" });

// Map controllers
app.MapControllers();

app.Run();