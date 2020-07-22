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
using xgca.core.AuditLog;
using xgca.core.User;
using xgca.core.Helpers.Http;
using xgca.core.Helpers.Token;
using xgca.core.Company;
using xgca.core.Profile;
using xgca.core.Constants;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using xgca.core.Helpers;
using System.Text.RegularExpressions;
using xgca.core.CompanyServiceUserRole;
using xgca.data.CompanyServiceUserRole;
using xgca.core.CompanyGroupResource;
using xgca.data.CompanyGroupResource;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using xgca.core.Helpers.QueryFilter;
using Z.EntityFramework.Plus;
using xgca.core.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

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
            //Instantiate Cognito Provider on Startup
            //AmazonCognitoIdentityProviderClient cognitoIdentityProvider =
            // new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), Amazon.RegionEndpoint.USEast1);

            ////Instantiate Cognito User Pool
            //CognitoUserPool userPool = new CognitoUserPool(Configuration.GetSection("AWSCognito:UserPoolId").Value,
            //    Configuration.GetSection("AWSCognito:UserPoolClientId").Value, cognitoIdentityProvider);

            services.AddCognitoIdentity();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Audience = Configuration.GetSection("AWSCognito:UserPoolClientId").Value;
                options.Authority = $"https://cognito-idp.{Configuration.GetSection("AWSCognito:Region").Value}.amazonaws.com/{Configuration.GetSection("AWSCognito:UserPoolId").Value}";
            });

            services.AddDbContext<XGCAContext>(opts => opts.UseLazyLoadingProxies(false).UseSqlServer(Configuration["ConnectionString:XGCADb"]));

            //services.AddSingleton<IAmazonCognitoIdentityProvider>(cognitoIdentityProvider);
            //services.AddSingleton<CognitoUserPool>(userPool);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<xgca.core.Models.S3.Variables>(Configuration.GetSection("AWS"));
            services.AddScoped<IXGCAContext, XGCAContext>();
            services.AddScoped<IUserData, UserData>();
            services.AddScoped<IAddressData, AddressData>();
            services.AddScoped<IAddressType, AddressType>();
            services.AddScoped<IAuditLogData, AuditLogData>();
            services.AddScoped<ICompanyData, CompanyData>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyServiceRole, CompanyServiceRole>();
            services.AddScoped<ICompanyServiceUser, CompanyServiceUser>();
            services.AddScoped<ICompanyUser, CompanyUser>();
            services.AddScoped<IContactDetail, ContactDetail>();
            services.AddScoped<IGeneral, General>();

            services.AddScoped<ICompanyGroupResource, CompanyGroupResource>();
            services.AddScoped<ICompanyGroupResourceData, CompanyGroupResourceData>();

            services.AddScoped<ICompanyServiceUserRole, CompanyServiceUserRole>();
            services.AddScoped<ICompanyServiceUserRoleData, CompanyServiceUserRoleData>();

            services.AddScoped<IAuditLogHelper, AuditLogHelper>();
            services.AddScoped<IQueryFilterHelper, QueryFilterHelper>();
            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IGeneralModel, GeneralModel>();

            services.AddScoped<IUser, User>();
            services.AddScoped<xgca.core.Address.IAddress, xgca.core.Address.Address>();
            services.AddScoped<xgca.core.AddressType.IAddressType, xgca.core.AddressType.AddressType>();
            services.AddScoped<ICompany, Company>();
            services.AddScoped<IAuditLogCore, AuditLogCore>();
            services.AddScoped<IProfile, xgca.core.Profile.Profile>();
            services.AddScoped<IHttpHelper, HttpHelper>();
            services.AddScoped<ITokenHelper, TokenHelper>();
            services.AddScoped<xgca.core.CompanyService.ICompanyService, xgca.core.CompanyService.CompanyService>();
            services.AddScoped<xgca.core.CompanyServiceRole.ICompanyServiceRole, xgca.core.CompanyServiceRole.CompanyServiceRole>();
            services.AddScoped<xgca.core.CompanyServiceUser.ICompanyServiceUser, xgca.core.CompanyServiceUser.CompanyServiceUser>();
            services.AddScoped<xgca.core.CompanyUser.ICompanyUser, xgca.core.CompanyUser.CompanyUser>();
            services.AddScoped<xgca.core.ContactDetail.IContactDetail, xgca.core.ContactDetail.ContactDetail>();
            services.AddScoped<IGLobalCmsService, xgca.core.Services.GlobalCmsService>();

            services.Configure<xgca.core.Helpers.GlobalCmsService>(o =>
            {
                o.BaseUrl = Configuration.GetSection("GlobalCmsService:BaseUrl").Value;
                o.GetServiceDetails = Configuration.GetSection("GlobalCmsService:GetServiceDetails").Value;
                o.GetService = Configuration.GetSection("GlobalCmsService:GetService").Value;
                o.GetCountry = Configuration.GetSection("GlobalCmsService:GetCountry").Value;
                o.GetState = Configuration.GetSection("GlobalCmsService:GetState").Value;
                o.GetCity = Configuration.GetSection("GlobalCmsService:GetCity").Value;
                o.GetUserType = Configuration.GetSection("GlobalCmsService:GetUserType").Value;
                o.GetResourcesForAuthorization = Configuration.GetSection("GlobalCmsService:GetResourcesForAuthorization").Value;
            });

            services.Configure<OptimusAuthService>(o => {
                o.BaseUrl = Configuration.GetSection("OptimusAuthService:BaseUrl").Value;
                o.EnableUserBatch = Configuration.GetSection("OptimusAuthService:EnableUserBatch").Value;
                o.DisableUserBatch = Configuration.GetSection("OptimusAuthService:DisableUserBatch").Value;
                o.EnableUser = Configuration.GetSection("OptimusAuthService:EnableUser").Value;
                o.DisableUser = Configuration.GetSection("OptimusAuthService:DisableUser").Value;
                o.SingleRegisterUser = Configuration.GetSection("OptimusAuthService:SingleRegisterUser").Value;
            });

            services.AddHttpClient();
            services.AddMvc().AddNewtonsoftJson(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            });
            services.AddControllers();            
            services.AddCors(o => o.AddPolicy("AllowAllPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.ConfigureApplicationCookie(options => {
                options.AccessDeniedPath = "/Account/Login";
                options.LoginPath = "/Account/Denied";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.Events.OnRedirectToLogin = context => {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "XLOG Client Management API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();


            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", name: "XLOG Auth API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAllPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
