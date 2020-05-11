using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication2.DAL;
using WebApplication2.Middleware;
using WebApplication2.Models;
using WebApplication2.Service;

namespace WebApplication2
{
    public class Startup
    {
        static string connectionString = "Data Source=localhost\\localsql;Initial Catalog=apbd1;User ID=sa;Password=Szuchow97!";
        //private IDbService _dbService;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //_dbService = dbService;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStudentDbService, SqlServerDbService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMyMiddleWare();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            
            app.Use(async (context, next) =>
            {
                if (!context.Request.Headers.ContainsKey("Index"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Nie podałes indeksu");
                    return;
                }

                string index = context.Request.Headers["Index"].ToString();
                

                //check in db

                using (SqlConnection connection = new SqlConnection(
               connectionString))
                {
                    SqlCommand command = new SqlCommand("select * from Student where IndexNumber=@id", connection);
                    command.Parameters.AddWithValue("id", index);
                    command.Connection.Open();
                    var dr = command.ExecuteReader();
                    while (!dr.Read())
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Nie ma studenta o takim indeksie");
                        return;
                    }
                    

                }


                await next();
            });
            app.UseHttpsRedirection();
            //app.UseMiddleware<LoggingMiddleware>();

            
            app.UseMvc();
        }
    }
}
