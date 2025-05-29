using ClinicSystemModularMonolith.Infrastructure.Middleware;
using Serilog;
using Users.Api.Extension;
using Users.Application;
using Users.Domain;
using Users.Domain.Seeders;
using Users.Infrastructure;
using Users.Infrastructure.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) =>
    cfg.ReadFrom.Configuration(ctx.Configuration)
       .Enrich.FromLogContext()
       .WriteTo.Console());

builder.Services.AddOpenApi();
builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services
    .AddUserPersistence(builder.Configuration)
    .AddUserApplication()
    .AddUserInfrastructure();
builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    await RolePermissionSeeder.SeedAsync(db);
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseSerilogRequestLogging();

app.UseMiddleware<RequestLogContextMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors(policy =>
{
    policy.AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials()
          .WithOrigins("https://localhost:3000");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
