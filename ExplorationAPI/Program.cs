using Azure.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo()
    {
        Description = "This is effectively the client for now.",
        Title = "Exploration Game",
        Version = "v1"
    });
});
string azureUri = builder.Configuration["AzureCosmos:URI"];
string azureKey = builder.Configuration["AzureCosmos:PrimaryKey"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    builder.Configuration.AddAzureKeyVault(new Uri("https://explorationkv.vault.azure.net/"), new DefaultAzureCredential());
    azureUri = builder.Configuration["azureUri"];
    azureKey = builder.Configuration["PrimaryKey"];
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
