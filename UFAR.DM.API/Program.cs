using Microsoft.EntityFrameworkCore;
using UFAR.DM.API.Core.Services.Section;
using UFAR.DM.API.Core.Services.Expression;
using UFAR.DM.API.Core.Services.Word;
using UFAR.DM.API.Core.Services.ChatGPT;
using UFAR.DM.API.Core.Services.Question;
using UFAR.DM.API.Data.DAO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MainDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MainConnectionString")));

builder.Services.AddScoped<IGPTservices, GPTservices>();
builder.Services.AddScoped<IQuestionServices, QuestionServices>();
builder.Services.AddScoped<IWordServices, WordServices>();
builder.Services.AddScoped<IExpressionServices, ExpressionServices>();
builder.Services.AddScoped<ISectionServices, SectionServices>();
builder.Services.AddApplicationInsightsTelemetry();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try {
    var dbContext = services.GetRequiredService<MainDbContext>();

    // Ensure the database is created/migrated
    dbContext.Database.EnsureCreated();

    // Example of querying data with eager loading
    var sectionsWithWordsAndExpressions = dbContext.Sections
        .Include(s => s.Words)
        .Include(s => s.Expressions)
        .ToList();

    // Example of using the retrieved data
    foreach (var section in sectionsWithWordsAndExpressions) {
        Console.WriteLine($"Section: {section.Name}");
        foreach (var word in section.Words) {
            Console.WriteLine($"- Word: {word.Word}");
        }
        foreach (var expression in section.Expressions) {
            Console.WriteLine($"- Expression: {expression.Expression}");
        }
    }
} catch (Exception ex) {
    Console.WriteLine("An error occurred while seeding the database.");
    Console.WriteLine(ex.Message);
}

app.MapControllers();

app.Run();