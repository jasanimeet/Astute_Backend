using System;
using System.Configuration;
using System.Text;
using astute.Authorization;
using astute.Models;
using astute.Repository;
using astute.TaskScheduler;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace astute
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
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue; // Adjust as needed
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = int.MaxValue; // Adjust as needed
            });

            //services.AddDbContext<AstuteDbContext>();
            services.AddDbContext<AstuteDbContext>(config =>
            config.UseSqlServer(
             "AstuteConnection",
             providerOptions =>
             {
                 providerOptions.CommandTimeout(7200);
             })
            );
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<ITermsService, TermsService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IProcessService, ProcessService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IPointerService, PointerService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IBGMService, BGMService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IInvoiceRemarksService, InvoiceRemarksService>();
            services.AddScoped<IEmpRightsService, EmpRightsService>();
            services.AddScoped<IHolidayService, HolidayService>();
            services.AddScoped<IRapaportService, RapaportService>();
            services.AddScoped<IPartyService, PartyService>();
            services.AddScoped<ITermsAndConditionService, TermsAndConditionService>();
            services.AddScoped<IJWTAuthentication, JWTAuthentication>();
            services.AddScoped<IExchange_Rate_Service, Exchange_Rate_Service>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ILabUserService, LabUserService>();

            services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {   
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtToken:Issuer"],
                    ValidAudience = Configuration["JwtToken:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtToken:SecretKey"])),
                    //ValidateLifetime = true, // Validate the token's lifetime
                    //ClockSkew = TimeSpan.Zero,

                };
            });

            var schedulerEnabled = Configuration.GetValue<bool>("Scheduler_Enabled");
            if (schedulerEnabled)
            {
                services.AddHostedService<ScheduledJobService>();
            }

            services.AddHttpContextAccessor();

            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {   
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }            

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();   
            app.UseAuthorization();
            app.UseCors("corsapp");
            app.UseStaticFiles();
            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints =>
            {   
                endpoints.MapControllers();
            });
        }
    }
}
