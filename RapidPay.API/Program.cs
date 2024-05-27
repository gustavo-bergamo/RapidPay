using Microsoft.EntityFrameworkCore;
using RapidPay.API.Middlewares;
using RapidPay.DependecyInjection;
using RapidPay.Domain.Data;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

DependencyInitializer.Initializer(builder.Services, configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RapidPay Docs v1");
        c.DocExpansion(DocExpansion.None);
    });
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
    await dbContext.Database.MigrateAsync();
}

app.UseExceptionHandler();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<UserMiddleware>();
app.UseMiddleware<ErrorMiddleware>();

app.Run();
