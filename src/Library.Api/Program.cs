using Library.Api.Config.Swagger;
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Scan(selector => selector
    .FromAssemblies(
        Library.Application.AssemblyReference.assembly,
        Library.Infra.AssemblyReference.assembly)
    .AddClasses(publicOnly: false)
    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
    .AsMatchingInterface()
    .WithScopedLifetime());

builder.Services.AddMediatR(cfg => cfg
    .RegisterServicesFromAssembly(Library.Application.AssemblyReference.assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
