using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SandwichBucket.Models;

namespace SandwichBucket
{
  public class Startup
  {
    public IConfigurationRoot Configuration { get; set; }

    public Startup(IWebHostEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json");
      Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {

      services.AddMvc();

      services.AddEntityFrameworkMySql()
          .AddDbContext<SandwichBucketContext>(options =>
              options.UseMySql(Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(Configuration["ConnectionStrings:DefaultConnection"])));

      services.AddCors(options => options
          .AddPolicy("CorsPolicy", builder => builder
              .AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
          )
      );
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseDeveloperExceptionPage();
      app.UseRouting();

      //Trying the code below for building Flappybird
      var options = new StaticFileOptions();
      var contentTypeProvider = (FileExtensionContentTypeProvider)options.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
      contentTypeProvider.Mappings.Add(".unityweb", "application/octet-stream");
      // contentTypeProvider.Mappings.Add(".data", "application/octet-stream");
      options.ContentTypeProvider = contentTypeProvider;
      app.UseStaticFiles(options);

      // app.UseStaticFiles();

      app.UseCors("CorsPolicy");

      app.UseEndpoints(routes =>
      {
        routes.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
      });

      app.Run(async (context) =>
      {
        await context.Response.WriteAsync("Error error error!");
      });
    }
  }
}
