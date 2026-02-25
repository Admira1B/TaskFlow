using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TaskFlow.Shared.Core.Extensions {
    public static class AspNetIdentityExtensions {
        public static IServiceCollection AddAspNetIdentity<TContext, TUser, TRole>(this IServiceCollection services)
            where TContext : IdentityDbContext<TUser, TRole, Guid>  
            where TUser : IdentityUser<Guid>
            where TRole : IdentityRole<Guid> { 

            services.AddIdentity<TUser, TRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<TContext>()
            .AddDefaultTokenProviders()
            .AddRoles<TRole>();

            return services;
        }
    }
}
