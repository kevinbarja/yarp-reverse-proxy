using Microsoft.AspNetCore.HttpLogging;
using Yarp.Gateway.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
});


builder.Services.AddTransient<RequestBodyLoggingMiddleware>();
builder.Services.AddTransient<ResponseBodyLoggingMiddleware>();

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("authenticated", policy =>
//        policy.RequireAuthenticatedUser());
//});

//builder.Services.AddRateLimiter(rateLimiterOptions =>
//{
//    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
//    {
//        options.Window = TimeSpan.FromSeconds(10);
//        options.PermitLimit = 5;
//    });
//});

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthentication();

//app.UseAuthorization();

//app.UseRateLimiter();
app.UseRequestBodyLogging();
app.UseResponseBodyLogging();

app.UseHttpLogging();
app.MapReverseProxy();

app.Run();
