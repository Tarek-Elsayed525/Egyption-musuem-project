using eg_mu.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. DATABASE & INFRASTRUCTURE SERVICES
// ============================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GrandEgyptianContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ============================================================
// 2. CORS & DOCUMENTATION (السياسات العامة)
// ============================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Grand Egyptian Museum API",
        Version = "v1",
        Description = "Professional Backend Service for GEM (Open Access Mode)"
    });
    // تم إزالة إعدادات الـ Security من Swagger لتسهيل التجربة المباشرة
});

var app = builder.Build();

// ============================================================
// 3. REQUEST PIPELINE (الترتيب الجديد البسيط)
// ============================================================

// تفعيل واجهة Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "GEM API V1");
    options.DocumentTitle = "GEM API Documentation";
    options.DisplayRequestDuration();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// تفعيل الـ CORS
app.UseCors("AllowAll");

// تم إزالة UseAuthentication لأننا لا نستخدم التوكن الآن
app.UseAuthorization();

app.MapControllers();

app.Run();