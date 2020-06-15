using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using xgca.entity;
using xgca.data;
using xgca.data.User;
using xgca.data.AddressType;
using xgca.data.Address;
using xgca.data.AuditLog;
using xgca.data.ContactDetail;
using xgca.data.Company;
using xgca.data.CompanyService;
using xgca.data.CompanyServiceRole;
using xgca.data.CompanyServiceUser;
using xgca.data.CompanyUser;
using xgca.core.Response;
using xgca.core.User;
using xgca.core.Helpers.Http;
using xgca.core.Helpers.Token;
using xgca.core.Company;
using xgca.core.Profile;
using xgca.core.Constants;

namespace xlog_client_management_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var d = env.EnvironmentName;
            var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
             .AddEnvironmentVariables();
            configuration = builder.Build();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<XGCAContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:XGCADb"]));
            
            services.Configure<xgca.core.Models.S3.Variables>(Configuration.GetSection("AWS"));
            services.AddScoped<IXGCAContext, XGCAContext>();
            services.AddScoped<IUserData, UserData>();
            services.AddScoped<IAddressData, AddressData>();
            services.AddScoped<IAddressType, AddressType>();
            services.AddScoped<IAuditLog, AuditLog>();
            services.AddScoped<ICompanyData, CompanyData>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyServiceRole, CompanyServiceRole>();
            services.AddScoped<ICompanyServiceUser, CompanyServiceUser>();
            services.AddScoped<ICompanyUser, CompanyUser>();
            services.AddScoped<IContactDetail, ContactDetail>();
            services.AddScoped<IGeneral, General>();

            services.AddScoped<IGeneralModel, GeneralModel>();

            services.AddScoped<IUser, User>();
            services.AddScoped<xgca.core.Address.IAddress, xgca.core.Address.Address>();
            services.AddScoped<xgca.core.AddressType.IAddressType, xgca.core.AddressType.AddressType>();
            services.AddScoped<ICompany, Company>();
            services.AddScoped<IProfile, Profile>();
            services.AddScoped<IHttpHelper, HttpHelper>();
            services.AddScoped<ITokenHelper, TokenHelper>();
            services.AddScoped<xgca.core.CompanyService.ICompanyService, xgca.core.CompanyService.CompanyService>();
            services.AddScoped<xgca.core.CompanyServiceRole.ICompanyServiceRole, xgca.core.CompanyServiceRole.CompanyServiceRole>();
            services.AddScoped<xgca.core.CompanyServiceUser.ICompanyServiceUser, xgca.core.CompanyServiceUser.CompanyServiceUser>();
            services.AddScoped<xgca.core.CompanyUser.ICompanyUser, xgca.core.CompanyUser.CompanyUser>();
            services.AddScoped<xgca.core.ContactDetail.IContactDetail, xgca.core.ContactDetail.ContactDetail>();

            services.Configure<GlobalCmsApi>(o => {
                o.BaseUrl = Configuration.GetSection("GlobalCMS:BaseUrl").Value;
            });

            services.AddHttpClient();
            services.AddMvc().AddNewtonsoftJson();
            services.AddControllers();            
            services.AddCors(o => o.AddPolicy("AllowAllPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAllPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
