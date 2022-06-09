using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Stellarium;
using System.Net;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DB");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("Data Source=localhost\\SQLEXPRESS01;Initial Catalog=Stellarium;Integrated Security=True;MultipleActiveResultSets=True"));
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/login1", "login2");
});

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = 209715200;
    x.MultipartBodyLengthLimit = 209715200;
    x.MultipartHeadersLengthLimit = 209715200;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 209715200;
});

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "564844095411-3nfvcdjnaj26u0qj1kkifi4849i3n6pl.apps.googleusercontent.com";
        options.ClientSecret = "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";
    });


var app = builder.Build();

app.UseDeveloperExceptionPage();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapRazorPages();

app.Run();
