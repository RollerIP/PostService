using User_Service.Contexts;
using User_Service.Messaging;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

FirebaseApp.Create(new AppOptions()
{
    ProjectId = "roller-29933",
    Credential = GoogleCredential.FromFile("D:/Documents/Documents/Fontys/Software/RB10/IP/Roller/roller-29933-firebase-adminsdk-jqwx6-e79115a242.json"),
});

// Add services to the container.
builder.Services.AddSingleton<IMessageService, NatsService>();
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseInMemoryDatabase("UsersList"));
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Load needed services
app.Services.GetServices<IMessageService>();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
