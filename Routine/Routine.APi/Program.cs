using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Routine.APi.Data;
using System;

namespace Routine.APi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using(var scope = host.Services.CreateScope())
            {
                try
                {
                    //每次运行都把数据库删了重建
                    var dbContext = scope.ServiceProvider.GetService<RoutineDbContext>();
                    dbContext.Database.EnsureCreated();
                    //dbContext.Database.Migrate();
                }
                catch(Exception e)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "Database Migration Error!");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureLogging((context, logging) =>
                {
                    //清空之前的日志设置
                    logging.ClearProviders();
                    //采用配置文件中的日志配置
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    //添加控制台日志打印
                    logging.AddConsole();
                    //添加output日志打印
                    logging.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls("http://localhost:8866");
                });
    }
}
