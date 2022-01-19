using System;
using System.Linq;
using System.Text;
using NRH_UserAdress.Rabbit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using NRH_UserAdress.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NRH_UserAdress.Rabbit
{
    public class RabbitReceive
    {
        public static void Main()
        {
            ListenForIntegrationEvents();
        }

        private static void ListenForIntegrationEvents()
        {
            var channel = RabbitChannel.Init();
            var consumer = new EventingBasicConsumer(channel);

Console.WriteLine("bar");
            consumer.Received += (model, ea) =>
            {
Console.WriteLine("foo");
                var contextOptions = new DbContextOptionsBuilder<UserAdressContext>()
                    .UseSqlServer("Data Source=nrhtodoapipublic.c2hgnaiy8dpt.ca-central-1.rds.amazonaws.com,1433; Initial Catalog=NRH_UserAdress; User ID='NRHDev01'; Password='!aA111111';")
                    .Options;
                //var options = new DbContextOptionsBuilder<DataContext>().UseSqlServer("Data Source=nrhtodoapipublic.c2hgnaiy8dpt.ca-central-1.rds.amazonaws.com,1433; Initial Catalog=NRH_UserDB; User ID='NRHDev01'; Password='!aA111111';").Options;


                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var data = JObject.Parse(message);
                var type = ea.RoutingKey;
Console.WriteLine("type: " + type);

                var dbContext = new UserAdressContext(contextOptions);

                var entityType = dbContext.Model.FindEntityType(typeof(Account));

                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    //var id   = data["depid"].ToObject<int>();
                    var id = data["userid"].ToObject<string>();
                    var PaysID = data["pays"].ToObject<int>();
                    var ProvinceID = data["province"].ToObject<int>();
                    var VilleID = data["ville"].ToObject<int>(); ;

Console.WriteLine("PaysID: " + PaysID);


                    var User = new Account { ID = int.Parse(id), PaysId = PaysID, ProvinceId = ProvinceID, VilleId = VilleID };
//Console.WriteLine("add: " + type);
                    if (type == "user.add") //  nouveau User
                    {
                        dbContext.Account.Add(User);

                    }
                    else if (type == "user.edit")
                    {
                        dbContext.Account.Update(User);
//Console.WriteLine("edit: " + type);
                    }
                    else if (type == "user.delete")
                    {
//Console.WriteLine("delete: " + type);
                        dbContext.Account.Remove(User);

                    }
                    dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} ON");
                    dbContext.SaveChanges();
                    dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} OFF");

                    transaction.Commit();
                }

            };
            channel.BasicConsume(queue: "user.adress",
                                     autoAck: true,
                                     consumer: consumer);
        }





//        private static void ListenForIntegrationEvents()
//        {
//            var channel = RabbitChannel.Init();
            
//            var consumer = new EventingBasicConsumer(channel);

//            consumer.Received += (model, ea) =>
//            {

//                var builder = WebApplication.CreateBuilder();
//                var contextOptions = new DbContextOptionsBuilder<UserAdressContext>()
//                    .UseSqlServer(builder.Configuration.GetConnectionString("myconn02"))
//                    .Options;

//                var dbContext = new UserAdressContext(contextOptions);
//                var body = ea.Body.ToArray();
//                var message = Encoding.UTF8.GetString(body);
//                var data = JObject.Parse(message);
//                var type = ea.RoutingKey;
//console.writeline("type: " + type);
//                if (type == "User.add")
//                {
//                    var entityType = dbContext.Model.FindEntityType(typeof( Users ));

//                    using (var transaction = dbContext.Database.BeginTransaction())
//                    {
//                        var id   = data["id"].ToObject<int>();
//                        var name = data["name"].ToObject<string>();

//                        // Items.SelectToken("Documents[0].Brands")?.ToObject<string[]>();


//                        Console.WriteLine("id:" + id);
//                        Console.WriteLine("name:" + name);
//                        var UserAdress = new Users { ID = id, Name = name };
//                        dbContext.Users.Add(UserAdress);
//                        dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} ON");
//                        dbContext.SaveChanges();
//                        dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} OFF");
//                        transaction.Commit();
//                    }
//                }
//                else if (type == "User.horaire")
//                {
//                    var entityType = dbContext.Model.FindEntityType(typeof(Users));

//                    using (var transaction = dbContext.Database.BeginTransaction())
//                    {
//                        var id = data["id"].ToObject<int>();
//                        var name = data["name"].ToObject<string>();

//                        // Items.SelectToken("Documents[0].Brands")?.ToObject<string[]>();


//                        Console.WriteLine("id:" + id);
//                        Console.WriteLine("name:" + name);
//                        var UserAdress = new Users { ID = id, Name = name };
//                        dbContext.Users.Add(UserAdress);
//                        dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} ON");
//                        dbContext.SaveChanges();
//                        dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} OFF");
//                        transaction.Commit();
//                    }
//                }
//            };
//            channel.BasicConsume(queue: "user.UserAdress",
//                                     autoAck: true,
//                                     consumer: consumer);
//        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //       {
        //            webBuilder.UseStartup<Program>();
        //       });
    }
}