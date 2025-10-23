namespace Eventmanagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<EventmanagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'EventmanagementContext' not found.")));

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication("UserCookie")
                .AddCookie("UserCookie", options =>
                {
                    options.LoginPath = "/Login/AccessDenied";
                    options.AccessDeniedPath = "/Login/AccessDenied";
                    options.LogoutPath = "/Login/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomerOnly", policy => policy.Requirements.Add(new MustBeCustomerRequirement()));
                options.AddPolicy("CoworkerOnly", policy => policy.Requirements.Add(new MustBeCoworkerRequirement()));
                options.AddPolicy("OrganizerOnly", policy => policy.Requirements.Add(new MustBeOrganizerRequirement()));
                options.AddPolicy("LocationOnly", policy => policy.Requirements.Add(new MustBeLocationRequirement()));
                options.AddPolicy("ModeratorOnly", policy => policy.Requirements.Add(new MustBeModeratorRequirement()));
                options.AddPolicy("PerformerOnly", policy => policy.Requirements.Add(new MustBePerformerRequirement()));
                options.AddPolicy("LocationOrCoworkerOnly", policy => policy.Requirements.Add(new MustBeLocationOrCoworkerRequirement()));
                options.AddPolicy("OrganizerOrCoworkerOnly", policy => policy.Requirements.Add(new MustBeOrganizerOrCoworkerRequirement()));

                options.AddPolicy("RegisteredOnly", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new MustBeRegisteredRequirement());
                });
            });

            builder.Services.AddMemoryCache();

            builder.Services.AddScoped<IAuthorizationHandler, MustBeCustomerHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, MustBeCoworkerHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, MustBeOrganizerHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, MustBeLocationHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, MustBeModeratorHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, MustBePerformerHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, MustBeRegisteredHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, MustBeLocationOrCoworkerHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, MustBeOrganizerOrCoworkerHandler>();

            builder.Services.AddSession();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.Run();
        }
    }
}
