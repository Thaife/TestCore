using ApplicationCore.Web.Startup;
using Base.Web.Startup;

var builder = WebApplication.CreateBuilder(args);
BaseStartupConfig.ProgramStart(builder.Configuration);
BaseStartupConfig.ConfigureServices(builder.Services, builder.Configuration);
//
var app = builder.Build();
//
BaseStartupConfig.ConfigureApp(app);

app.Run();
