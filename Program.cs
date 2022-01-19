using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using NRH_UserAdress.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using NRH_UserAdress.Rabbit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<UserAdressContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("myconn02")));

builder.Services.AddMvc();
builder.Services.AddDbContext<UserAdressContext>(item => item.UseSqlServer(builder.Configuration.GetConnectionString("myconn02")));

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"temp-keys"))
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "NRH_UserAdress", Version = "v1" });


});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (builder.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NRH_UserAdress v1"));
// generated swagger json and swagger ui middleware
app.UseSwagger();
app.UseSwagger(c =>
{
    c.RouteTemplate = "NiovarRH/UserAdressMicroservices/swagger/{documentname}/swagger.json";
});


app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/NiovarRH/UserAdressMicroservices/swagger/v1/swagger.json", "UserAdresss");
    c.RoutePrefix = "NiovarRH/UserAdressMicroservices/swagger";
});
//}

// message queue listener
RabbitReceive.Main();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();













