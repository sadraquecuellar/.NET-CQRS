using Ambev.DeveloperEvaluation.Domain.Common.Messaging.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Users.Repositories;
using Ambev.DeveloperEvaluation.MessageBroker.RabbitMqMessageBroker;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISaleRepository, SalesRepository>();
        builder.Services.AddScoped<IMessageBroker, RabbitMqMessageBroker>();
    }
}