using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.DAL;
using WebApplication2.Middleware;

namespace WebApplication2.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            if (httpContext.Request != null)
            {
                string path = httpContext.Request.Path;
                string method = httpContext.Request.Method.ToString();
                string querystring = httpContext.Request?.QueryString.ToString();
                
                string bodyStr = "";

                using (StreamReader reader
                 = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    
                        bodyStr = await reader.ReadToEndAsync();
                    httpContext.Request.Body.Position = 0;
                    Console.WriteLine("test" + bodyStr);

                    using (StreamWriter writer = new StreamWriter("C:\\Users\\kbernatjanuszkiewicz\\source\\repos\\Cw3\\WebApplication2"))
                    {
                        writer.WriteLine(path);
                        writer.WriteLine(method);
                        writer.WriteLine(querystring);
                        writer.WriteLine(bodyStr);

                    }
                }



                //logowanie do pliku
            }

            await _next(httpContext);
        }
    }
}

public static class MyMiddlewareExtensions
{
    public static IApplicationBuilder UseMyMiddleWare(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}
