using System.Net.Http;
using Calculator;
using Grpc.Core;
using Grpc.Net.Client;

namespace GrpcClient;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("gRPC Calculator Client");
        Console.WriteLine("======================");
        Console.WriteLine();

        // Create a channel to the gRPC server
        var channel = GrpcChannel.ForAddress(
            "http://localhost:5000",
            new GrpcChannelOptions
            {
                HttpHandler = new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    KeepAlivePingPolicy = HttpKeepAlivePingPolicy.WithActiveRequests,
                },
            }
        );
        var client = new CalculatorService.CalculatorServiceClient(channel);

        try
        {
            // Test all calculator operations
            await TestAdd(client);
            await TestSubtract(client);
            await TestMultiply(client);
            await TestDivide(client);
            await TestDivideByZero(client); // This will demonstrate error handling
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"gRPC Error: {ex.Status.StatusCode} - {ex.Status.Detail}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static async Task TestAdd(CalculatorService.CalculatorServiceClient client)
    {
        Console.WriteLine("Testing Add operation...");
        var request = new AddRequest { A = 10, B = 20 };
        var response = await client.AddAsync(request);
        Console.WriteLine($"  {request.A} + {request.B} = {response.Result}");
        Console.WriteLine();
    }

    static async Task TestSubtract(CalculatorService.CalculatorServiceClient client)
    {
        Console.WriteLine("Testing Subtract operation...");
        var request = new SubtractRequest { A = 50, B = 15 };
        var response = await client.SubtractAsync(request);
        Console.WriteLine($"  {request.A} - {request.B} = {response.Result}");
        Console.WriteLine();
    }

    static async Task TestMultiply(CalculatorService.CalculatorServiceClient client)
    {
        Console.WriteLine("Testing Multiply operation...");
        var request = new MultiplyRequest { A = 7, B = 8 };
        var response = await client.MultiplyAsync(request);
        Console.WriteLine($"  {request.A} * {request.B} = {response.Result}");
        Console.WriteLine();
    }

    static async Task TestDivide(CalculatorService.CalculatorServiceClient client)
    {
        Console.WriteLine("Testing Divide operation...");
        var request = new DivideRequest { A = 100, B = 4 };
        var response = await client.DivideAsync(request);
        Console.WriteLine($"  {request.A} / {request.B} = {response.Result}");
        Console.WriteLine();
    }

    static async Task TestDivideByZero(CalculatorService.CalculatorServiceClient client)
    {
        Console.WriteLine("Testing Divide by Zero (Error Handling)...");
        var request = new DivideRequest { A = 10, B = 0 };

        try
        {
            var response = await client.DivideAsync(request);
            Console.WriteLine($"  {request.A} / {request.B} = {response.Result}");
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"  Error caught: {ex.Status.StatusCode} - {ex.Status.Detail}");
        }
        Console.WriteLine();
    }
}
