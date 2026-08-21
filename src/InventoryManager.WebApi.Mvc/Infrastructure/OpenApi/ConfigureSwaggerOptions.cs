namespace InventoryManager.WebApi.Mvc.Infrastructure.OpenApi
{
    using InventoryManager.WebApi.Infrastructure.Configuration;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private const string OPEN_API_FILENAME = "OpenApiDocumentation.xml";
        private readonly ConfigurationApp _configuration;
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="configuration">The bound <see cref="ConfigurationApp"/> settings.</param>
        /// <param name="apiVersionDescription">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IOptions<ConfigurationApp> configuration, IApiVersionDescriptionProvider apiVersionDescription)
        {
            _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
            _apiVersionDescriptionProvider = apiVersionDescription ?? throw new ArgumentNullException(nameof(apiVersionDescription));
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            options.DescribeAllParametersInCamelCase();
            options.CustomSchemaIds(type => type.FullName);
            options.ResolveConflictingActions(x => x.First());

            options.OperationFilter<AuthorizeCheckOperationFilter>();
            options.OperationFilter<ApiVersionOperationFilter>();

            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfoForApiVersion(description));
            }


            // How the OAuth2 implicit flow would be declared once an identity provider is in
            // place. The authority and scopes come from the Security section of appsettings.

            //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //{
            //    Type = SecuritySchemeType.OAuth2,
            //    Flows = new OpenApiOAuthFlows
            //    {
            //        Implicit = new OpenApiOAuthFlow
            //        {
            //            AuthorizationUrl = new Uri($"{_configuration.Security.Authority}/connect/authorize", UriKind.Absolute),
            //            Scopes = new Dictionary<string, string>
            //            {
            //                { _configuration.Security.ApiScope, _configuration.Security.ApiScopeName },
            //            }
            //        }
            //    }
            //});

            var xmlPath = Path.Combine(AppContext.BaseDirectory, OPEN_API_FILENAME);
            options.IncludeXmlComments(xmlPath);
        }

        private static OpenApiInfo CreateOpenApiInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Inventory Manager API",
                Version = description.ApiVersion.ToString(),
                Description = "A layered SOA REST API for managing inventory items.",
                Contact = new OpenApiContact() { Name = "Enrique Carrasco Contreras", Url = new Uri("https://github.com/smaugcoath") }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}