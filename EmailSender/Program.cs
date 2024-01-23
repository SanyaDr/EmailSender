// using Blazored.LocalStorage;
using EmailSender.Components;
using EmailSender.Interface;
using EmailSender.Models;
using EmailSender.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<EmailService>();
builder.Services.Configure<SmtpConfig>(options =>
{
    builder.Configuration.GetSection("EmailConfig").Bind(options);
    options.UserName = builder.Configuration["SmtpLogin"] ?? "";
    options.Password = builder.Configuration["Password"] ?? "";
});
builder.Services.AddScoped<IEmailHistorySaver, EmailHistoryJsonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();