using InventoryApp.Application.Interfaces;
using InventoryApp.Infrastructure.Persistence;
using InventoryApp.Infrastructure.Repositories;
using InventoryApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IPartyRepository, PartyRepository>();
            services.AddScoped<IItemPriceRepository, ItemPriceRepository>();
            services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IProductionRepository, ProductionRepository>();
            services.AddScoped<IInvoiceService, InvoiceService>();

            return services;
        }
    }
}