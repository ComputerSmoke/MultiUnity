using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Multiunity.Server.Sharding;
using Shared.Records;

namespace MultiunityServer.Rest
{
    internal class Api
    {
        World world;
        public Api(World world)
        {
            this.world = world;
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Multiunity API", Description = "an API for a game.", Version = "v1" });
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Multiunity API V1");
            });

            app.MapGet($"/{world.name}/rooms", () => world.RoomList());
            app.MapGet($"/{world.name}/rooms/{{id}}", (int id) => world.HasRoom(id));

            app.MapPost($"/world.name/rooms", CreateRoom);



            app.Run();
        }
        private bool CreateRoom(int id) {
            if (world.HasRoom(id)) return false;
            world.CreateRoom(id);
            return true;
        }
    }
}
