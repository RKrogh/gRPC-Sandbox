using GrpcServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on HTTP/2
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(
        5000,
        listenOptions =>
        {
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        }
    );
});

// Add gRPC services to the container
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Map gRPC services
app.MapGrpcService<CalculatorServiceImpl>();

// Add a simple health check endpoint
app.MapGet("/", () => "gRPC Calculator Server is running! Use a gRPC client to connect.");

Console.WriteLine("Starting gRPC Calculator Server...");
Console.WriteLine("Server will be available at: http://localhost:5000");
Console.WriteLine("gRPC endpoint: http://localhost:5000");
Console.WriteLine("Press Ctrl+C to stop the server.");

app.Run();
