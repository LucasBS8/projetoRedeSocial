using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using projetoRedeSocial.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache(); // Adiciona memória cache distribuída
builder.Services.AddSession(options =>
{

    options.IdleTimeout = TimeSpan.FromMinutes(30); // Define o tempo de timeout da sessão
    options.Cookie.HttpOnly = true; // Define o cookie como HttpOnly
    options.Cookie.IsEssential = true; // Marca o cookie como essencial
});
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<Contexto>(options =>
{

    string connectionString = builder.Configuration.GetConnectionString("ChaveDB") ?? "DefaultConnectionString";
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

app.Run();
