using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Project.Data.Repository;
using Project.Model.Commands;
using Project.Model.Events;
using Project.Model.Notifications;
using Project.Model.Queries;
using Project.Model.Queries.Interfaces;
using Project.Model.Repository.Interfaces;
using Project.Model.Settings;

namespace Project.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.Configure<CosmosDBSettings>(options =>
            {
                options.EndpointUri = Configuration.GetSection("CosmosConnection:EndpointUrl").Value;
                options.PrimaryKey = Configuration.GetSection("CosmosConnection:PrimaryKey").Value;
                options.DatabaseId = Configuration.GetSection("CosmosConnection:DatabaseId").Value;
                options.ContainerId = Configuration.GetSection("CosmosConnection:ContainerId").Value;
                options.PartitionKey = Configuration.GetSection("CosmosConnection:PartitionKey").Value;
                options.ApplicationName = Configuration.GetSection("CosmosConnection:ApplicationName").Value;                
            });

            services.AddTransient<IQuestionRepositoryWrite, QuestionRepositoryWrite>();
            services.AddTransient<IQuestionRepositoryRead, QuestionRepositoryRead>();

            services.AddTransient<IQuestionQueries, QuestionQueries>();

            services.AddMediatR(typeof(Startup));
            services.AddScoped<IRequestHandler<AddQuestionCommand, bool>, AddQuestionCommandHandler>();
            services.AddScoped<IRequestHandler<AddAnswerCommand, bool>, AddAnswerCommandHandler>();
            services.AddScoped<IRequestHandler<LikeQuestionCommand, bool>, LikeQuestionCommandHandler>();            
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("CorsPolicy");
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
