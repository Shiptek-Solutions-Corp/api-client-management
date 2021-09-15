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
using xlog_client_management_api.Middlewares;
using xgca.core.Helpers.Guest;
using xgca.core.Guest;
using xgca.data.Guest;
using xgca.core.Helpers.Utility;
using xgca.data.PreferredContact;
using xgca.core.PreferredContact;
using xgca.data.Invite;
using xgca.core.Invite;
using xgca.core.Email;
using xgca.data.PreferredProvider;
using xgca.core.PreferredProvider;
using xgca.core.Helpers.PreferredProvider;
using xgca.core.Helpers.Service;
using xgca.data.Repositories;
using Z.EntityFramework.Extensions;
using xgca.data.CompanyTaxSettings;
using xgca.core.ResponseV2;
using xas.core.accreditation.Request;
using xas.data.accreditation.Request;
using xas.core._ResponseModel;
using FluentValidation;
using xas.core.Request;
using xgca.core.Validators.Request;
using xas.core.TruckArea;
using xas.data.DataModel.TruckArea;
using xas.data.DataModel.PortArea;
using xas.core.PortArea;
using xas.core.ServiceRoleConfig;
using xas.core.ServiceProvider;
using xas.data.DataModel.ServiceRoleConfig;
using Amazon.SecurityToken;
using xgca.core.Models.Accreditation.PortArea;

namespace xlog_client_management_api
{
    //Scaffold-dbContext "Server=127.0.0.1,11433;Initial Catalog=Dev-Client;User ID=userClientDev;Password=VlormyFrbidrt" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -t Company.Company, Settings.KycStatus, Company.CompanyStructure, Company.CompanyDocuments, Company.CompanyBeneficialOwners, Company.CompanyDirectors, Company.CompanySections, Settings.DocumentType, Settings.DocumentCategory, Settings.BeneficialOwnersType, Settings.SectionStatus, Settings.Section, Settings.KYCLog -ContextDir Context -Context ClientServiceContext -f
    //Scaffold-dbContext "Server=127.0.0.1,11433;Initial Catalog=Dev-Client;User ID=userClientDev;Password=VlormyFrbidrt" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -t Accreditation.AccreditationStatusConfig, Accreditation.Request, Accreditation.TruckArea, Accreditation.PortArea, Accreditation.ServiceRoleConfig -ContextDir Context -Context ClientServiceContext -f

    //$env:ASPNETCORE_ENVIRONMENT='local'
    //Script-Migration -Idempotent
    //EntityFrameworkCore\Add-Migration nameofmigration
    public class Startup
    {
        private string[] envLst = new string[] { "local", "dev2" };
        private string currentEnvironment = String.Empty;

        public IConfiguration Configuration { get; }
        private string conString { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            currentEnvironment = env.EnvironmentName;

            currentEnvironment = env.EnvironmentName;
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();
            configuration = builder.Build();
            Configuration = configuration;

            if (env.EnvironmentName == "local")
            {
                conString = Configuration["ConnectionString:XGCADb"];
            }
            else
            {
                conString = SecretsManager.GetConnectionString(
                    Configuration.GetSection("AWSSecretsManager:SecretName").Value,
                    Configuration.GetSection("AWSSecretsManager:Region").Value,
                    Configuration.GetSection("ConnectionString:DatabaseName").Value
                );
            }
        }



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
            services.AddDbContext<XGCAContext>(i => i.UseSqlServer(conString));
            //services.AddDbContext<XGCAContext>(opts => opts.UseLazyLoadingProxies(false).UseSqlServer(Configuration["ConnectionString:XGCADb"]));

            //services.AddDbContextPool<XGCAContext>(opts => opts.UseSqlServer(conString, builder =>
            //{
            //    builder.EnableRetryOnFailure();
            //    //builder.ServerVersion(new System.Version("5.6.10"), ServerType.MySql);
            //}));

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
            services.AddScoped<IEmail, Email>();
            services.AddScoped<IGuestData, GuestData>();
            services.AddScoped<IInviteData, InviteData>();
            services.AddScoped<IPreferredContactData, PreferredContactData>();
            services.AddScoped<IPreferredProviderData, PreferredProviderData>();
            services.AddScoped<IGeneral, General>();
            services.AddScoped<ICompanySectionRepository, CompanySectionRepository>();
            services.AddScoped<ICompanyStructureRepository, CompanyStructureRepository>();
            services.AddScoped<ICompanyBeneficialOwnersRepository, CompanyBeneficialOwnersRepository>();
            services.AddScoped<ICompanyDocumentRepository, CompanyDocumentRepository>();
            services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
            services.AddScoped<ICompanyDirectorRepository, CompanyDirectorRepository>();
            services.AddScoped<IKYCStatusRepository, KYCStatusRepository>();
            services.AddScoped<ICompanySectionRepository, CompanySectionRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddTransient<ICompanyDataV2, CompanyDataV2>();
            services.AddTransient<ICompanyTaxSettingsRepository, CompanyTaxSettingsRepository>();
            services.AddScoped<IKYCLogRepository, KYCLogRepository>();


            services.AddScoped<ICompanyGroupResource, CompanyGroupResource>();
            services.AddScoped<ICompanyGroupResourceData, CompanyGroupResourceData>();

            services.AddScoped<ICompanyServiceUserRole, CompanyServiceUserRole>();
            services.AddScoped<ICompanyServiceUserRoleData, CompanyServiceUserRoleData>();

            services.AddScoped<IAuditLogHelper, AuditLogHelper>();
            services.AddScoped<IGuestHelper, GuestHelper>();
            services.AddScoped<IQueryFilterHelper, QueryFilterHelper>();
            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IGeneralModel, GeneralModel>();
            services.AddScoped<IPagedResponse, PagedResponse>();
            services.AddScoped<IPreferredContactHelper, PreferredContactHelper>();
            services.AddScoped<IPreferredProviderHelper, PreferredProviderHelper>();
            services.AddScoped<IProfile, xgca.core.Profile.Profile>();
            services.AddScoped<IHttpHelper, HttpHelper>();
            services.AddScoped<ITokenHelper, TokenHelper>();
            services.AddScoped<IServiceHelper, ServiceHelper>();
            services.AddScoped<IUtilityHelper, UtilityHelper>();

            services.AddScoped<IAuditLogCore, AuditLogCore>();
            services.AddScoped<ICompany, Company>();
            services.AddScoped<IGuestCore, GuestCore>();
            services.AddScoped<IUser, User>();
            services.AddScoped<xgca.core.Address.IAddress, xgca.core.Address.Address>();
            services.AddScoped<xgca.core.AddressType.IAddressType, xgca.core.AddressType.AddressType>();
            services.AddScoped<xgca.core.CompanyService.ICompanyService, xgca.core.CompanyService.CompanyService>();
            services.AddScoped<xgca.core.CompanyServiceRole.ICompanyServiceRole, xgca.core.CompanyServiceRole.CompanyServiceRole>();
            services.AddScoped<xgca.core.CompanyServiceUser.ICompanyServiceUser, xgca.core.CompanyServiceUser.CompanyServiceUser>();
            services.AddScoped<xgca.core.CompanyUser.ICompanyUser, xgca.core.CompanyUser.CompanyUser>();
            services.AddScoped<xgca.core.ContactDetail.IContactDetail, xgca.core.ContactDetail.ContactDetail>();
            services.AddScoped<IInviteCore, InviteCore>();
            services.AddScoped<IPreferredContactCore, PreferredContactCore>();
            services.AddScoped<IPreferredProviderCore, PreferredProviderCore>();
            services.AddScoped<IYourEDIService, YourEDIService>();
            services.AddTransient<ICompanyStructureService, CompanyStructureService>();
            services.AddTransient<ICompanyBeneficialOwnerService, CompanyBeneficialOwnerService>();
            services.AddTransient<ICompanyDirectorService, CompanyDirectorService>();
            services.AddTransient<ICompanyDocumentService, CompanyDocumentService>();
            services.AddTransient<ICompanySectionService, CompanySectionService>();
            services.AddTransient<IDocumentTypeService, DocumentTypeService>();
            services.AddTransient<ICompanyServiceV2, CompanyServiceV2>();
            services.AddTransient<ICompanyTaxSettingsService, CompanyTaxSettingsService>();
            services.AddTransient<IKYCLogService, KYCLogService>();
            services.AddTransient<IPaginationResponse, PaginationResponse>();

            services.AddScoped<IGLobalCmsService, xgca.core.Services.GlobalCmsService>();

            //Accreditation
            services.AddScoped<IRequestCore, RequestCore>();
            services.AddScoped<IPortAreaData, PortAreaData>();
            services.AddScoped<IPortAreaCore, PortAreaCore>();
            services.AddScoped<ITruckAreaData, TruckAreaData>();
            services.AddScoped<IRequestData, RequestData>();
            services.AddScoped<IGeneralResponse, GeneralResponse>();
            services.AddScoped<IValidator<List<RequestModel>>, CreateRequestValidator>();
            services.AddScoped<ITruckAreaCore, TruckAreaCore>();
            services.AddScoped<IServiceRoleConfigCore, ServiceRoleConfigCore>();
            services.AddScoped<ICServiceProvider, CServiceProvider>();
            services.AddScoped<IServiceRoleConfigData, ServiceRoleConfigData>();
            services.AddScoped<IValidator<List<CreatePortAreaModel>>, CreatePortAreadResponsiblityValidator>();
            

            //AWS
            services.AddAWSService<IAmazonSecurityTokenService>();

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
                o.ServiceList = Configuration.GetSection("GlobalCmsService:ServiceList").Value;
                o.PortDetailsBulk = Configuration.GetSection("GlobalCmsService:PortDetailsBulk").Value;
                o.RetrieveService = Configuration.GetSection("GlobalCmsService:RetrieveService").Value;
                o.GetServiceRoleId = Configuration.GetSection("GlobalCmsService:GetServiceRoleId").Value;
                o.PortSearch = Configuration.GetSection("GlobalCmsService:PortSearch").Value;



            });

            services.Configure<OptimusAuthService>(o => {
                o.BaseUrl = Configuration.GetSection("OptimusAuthService:BaseUrl").Value;
                o.EnableUserBatch = Configuration.GetSection("OptimusAuthService:EnableUserBatch").Value;
                o.DisableUserBatch = Configuration.GetSection("OptimusAuthService:DisableUserBatch").Value;
                o.EnableUser = Configuration.GetSection("OptimusAuthService:EnableUser").Value;
                o.DisableUser = Configuration.GetSection("OptimusAuthService:DisableUser").Value;
                o.SingleRegisterUser = Configuration.GetSection("OptimusAuthService:SingleRegisterUser").Value;
            });

            services.Configure<WebsiteLinks>(o =>
            {
                o.Login = Configuration.GetSection("WebsiteLinks:Login").Value;
            });

            services.Configure<EmailApi>(o =>
            {
                o.BaseUrl = Configuration.GetSection("EmailApi:BaseUrl").Value;
                o.ApiKey = Configuration.GetSection("EmailApi:ApiKey").Value;
            });

            services.Configure<EmailTemplate>(o =>
            {
                o.BaseTemplate = Configuration.GetSection("EmailTemplate:BaseTemplate").Value;
                o.SendContactInviteTemplate = Configuration.GetSection("EmailTemplate:SendContactInviteTemplate").Value;
                o.SendProviderInviteTemplate = Configuration.GetSection("EmailTemplate:SendProviderInviteTemplate").Value;
                o.SendCompanyActivationTemplate = Configuration.GetSection("EmailTemplate:SendCompanyActivationTemplate").Value;
            });

            services.Configure<EvaultEndPoints>(option => Configuration.GetSection("EvaultEndPoints").Bind(option));
            services.Configure<AuthConfig>(option => Configuration.GetSection("AuthConfig").Bind(option));
            
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
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    return Task.CompletedTask;
                };
            });

            if (envLst.Contains(currentEnvironment))
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "XLOG Client Management API", Version = "v1" });
                    c.SwaggerDoc("v2", new OpenApiInfo { Title = "XLOG Accreditation API", Version = "v1" });

                    // Set the comments path for the Swagger JSON and UI.
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Cognito Access Token",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            if (envLst.Contains(currentEnvironment))
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", name: "Client Management API V1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", name: "Accreditation API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseCors("AllowAllPolicy");

            app.UseRouting();

            //app.UseMiddleware<HttpInterceptor>();

            //app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

           
        }
    }
}
