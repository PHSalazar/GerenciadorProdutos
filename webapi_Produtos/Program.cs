using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using webapi_Produtos.Data;
using webapi_Produtos.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Api Gerenciador de Produtos",
                Version = "v1",
                Description = "API para Gerenciamento de Produtos"
            }
        );

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta maneira: Bearer SEU_TOKEN",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "api-autenticacao",
            ValidAudience = "api-produtos",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8dd246bea4ed33a8f5b2ac02a0f3b6cc0e2d09ea25220baf0678f0a5e7198f86554147d0ffe4072bc0423d0ac9cfa24817b8e809479f7cc14a3fe2f75313082ef07ed9f5e9f34483a95e655628ad9e23b40785b605618b5dadcdb62a086c4195091592ecc3a29483f7c75e0c74bb1b2b999a7006133f8695c94b4b14f5b9aabe8f8ce59e4e2162cc832ef9ad65048eaf69ebfa8b080324ccf8bbb11e1b8cff8941e1b7ef70f61a24c319a1dc9ed5b93f27623d272d3ee6975b0a59902bfd7a9e9f433a53490e2dcb6b0e767e723a8c531e7192f98dfffa73a29e0281a3dfd9ed68c3a8282b1093cf25f96a982ac780771e706d49012896badbbf3a35f1c65dd0"))
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Erro de autenticação: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validado com sucesso.");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddScoped<IProduto, ProdutoService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
