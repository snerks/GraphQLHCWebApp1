using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DiagnosticAdapter;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GraphQLHCWebApp1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();

            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>();

            //services.AddGraphQL(sp => SchemaBuilder.New()
            //  .AddQueryType<Query>()
            //  .AddMutationType<Mutation>()
            //  .AddServices(sp)
            //  .Create());
        }

        //// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    //if (env.IsDevelopment())
        //    //{
        //    //    app.UseDeveloperExceptionPage();
        //    //}

        //    //app.UseRouting();

        //    //app.UseAuthorization();

        //    //app.UseEndpoints(endpoints =>
        //    //{
        //    //    endpoints.MapControllers();
        //    //});

        //    app.UseGraphQL();
        //}


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseRouting()
            //   .UseEndpoints(endpoints =>
            //   {
            //       endpoints.MapGraphQL();
            //   });

            app.UseRouting()
               .UseEndpoints(endpoints =>
               {
                   endpoints.MapGraphQL();
                   //endpoints.MapGet("/", async context =>
                   //{
                   //    await context.Response.WriteAsync("Hello World!");
                   //});
               });



            // app.UseGraphQL();
        }
    }

    public class Query
    {
        public Character GetCharacter(string name = "Luke") => new Character { Name = name };

        public Widget GetWidget(string name = "Luke") => new Widget { Name = name };
    }

    public class Mutation
    {
        public Person UpdatePerson(UpdatePersonInput input)
        {
            return new Person
            {
                Id = input.Id,
                FirstName = input.FirstName,
                LastName = input.LastName
            };
        }
    }

    public class Character
    {
        public string Id { get; set; } = "1";

        public string Name { get; set; } = "Luke";
    }

    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UpdatePersonInput
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class BaseWidget
    {
        public IList<Person> Owners { get; set; } =
            new List<Person>
            {
                new Person
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Bob",
                    LastName = "Smith"
                }
            };
    }

    public class Widget : BaseWidget
    {
        public string Name { get; set; }
    }
}
