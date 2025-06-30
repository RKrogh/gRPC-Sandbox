# gRPC Tutorial with C# .NET

This project demonstrates how to implement gRPC communication between two C# .NET applications. It serves as a comprehensive tutorial for learning gRPC fundamentals.

## What is gRPC?

gRPC is a high-performance, open-source universal RPC (Remote Procedure Call) framework developed by Google. It enables client and server applications to communicate transparently and develop connected systems.

### Key Features:
- **Language Agnostic**: Works across multiple programming languages
- **High Performance**: Uses HTTP/2 for transport and Protocol Buffers for serialization
- **Strongly Typed**: Contract-first approach with `.proto` files
- **Streaming Support**: Supports unary, server streaming, client streaming, and bidirectional streaming
- **Code Generation**: Automatic client and server code generation

## Project Structure

```
gRPC/
├── proto/                          # Protocol Buffer definitions
│   └── calculator.proto            # Service definition
├── GrpcServer/                     # gRPC Server application
│   ├── Program.cs                  # Server entry point
│   ├── Services/                   # Service implementations
│   │   └── CalculatorService.cs    # Calculator service implementation
│   └── GrpcServer.csproj           # Server project file
├── GrpcClient/                     # gRPC Client application
│   ├── Program.cs                  # Client entry point
│   └── GrpcClient.csproj           # Client project file
└── README.md                       # This file
```

## Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022, VS Code, or any C# IDE
- Basic understanding of C# and async programming

## Getting Started

### 1. Build the Projects

```bash
# Build the server
dotnet build GrpcServer/GrpcServer.csproj

# Build the client
dotnet build GrpcClient/GrpcClient.csproj
```

### 2. Run the Applications

**Terminal 1 - Start the Server:**
```bash
cd GrpcServer
dotnet run
```

**Terminal 2 - Start the Client:**
```bash
cd GrpcClient
dotnet run
```

## How It Works

### 1. Protocol Buffer Definition (`proto/calculator.proto`)

The `.proto` file defines the service contract:

```protobuf
service CalculatorService {
  rpc Add(AddRequest) returns (AddResponse);
  rpc Subtract(SubtractRequest) returns (SubtractResponse);
  rpc Multiply(MultiplyRequest) returns (MultiplyResponse);
  rpc Divide(DivideRequest) returns (DivideResponse);
}
```

### 2. Server Implementation (`GrpcServer/Services/CalculatorService.cs`)

The server implements the service methods defined in the proto file:

```csharp
public override async Task<AddResponse> Add(AddRequest request, ServerCallContext context)
{
    var result = request.A + request.B;
    return new AddResponse { Result = result };
}
```

### 3. Client Implementation (`GrpcClient/Program.cs`)

The client creates a channel and calls the remote methods:

```csharp
using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new CalculatorService.CalculatorServiceClient(channel);

var response = await client.AddAsync(new AddRequest { A = 10, B = 20 });
Console.WriteLine($"Result: {response.Result}");
```

## Key Concepts Explained

### Protocol Buffers (protobuf)

Protocol Buffers is Google's language-neutral, platform-neutral, extensible mechanism for serializing structured data. It's more efficient than JSON and provides strong typing.

**Example:**
```protobuf
message AddRequest {
  int32 a = 1;
  int32 b = 2;
}
```

### gRPC Communication Flow

1. **Client** creates a gRPC channel to the server
2. **Client** creates a client stub using the generated code
3. **Client** calls methods on the stub (looks like local method calls)
4. **gRPC** serializes the request using Protocol Buffers
5. **gRPC** sends the request over HTTP/2
6. **Server** receives the request and deserializes it
7. **Server** processes the request and creates a response
8. **Server** serializes the response and sends it back
9. **Client** receives and deserializes the response

### Service Types

This tutorial demonstrates **Unary RPC** (request-response), but gRPC supports:

- **Unary RPC**: Client sends one request, server sends one response
- **Server Streaming RPC**: Client sends one request, server sends stream of responses
- **Client Streaming RPC**: Client sends stream of requests, server sends one response
- **Bidirectional Streaming RPC**: Both sides send streams of messages

## Code Generation

The `Grpc.Tools` NuGet package automatically generates C# code from `.proto` files during build:

```xml
<PackageReference Include="Grpc.Tools" Version="2.60.0">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

Generated files include:
- Request/Response message classes
- Service client class
- Service base class for server implementation

## Error Handling

gRPC uses status codes for error handling:

```csharp
try
{
    var response = await client.DivideAsync(new DivideRequest { A = 10, B = 0 });
}
catch (RpcException ex)
{
    Console.WriteLine($"gRPC Error: {ex.Status.StatusCode} - {ex.Status.Detail}");
}
```

## Security

For production applications, consider:
- **TLS/SSL**: Encrypt communication between client and server
- **Authentication**: Implement authentication mechanisms (JWT, OAuth, etc.)
- **Authorization**: Control access to specific methods

## Performance Considerations

- **Connection Reuse**: gRPC channels are designed to be reused
- **Streaming**: Use streaming for large datasets or real-time communication
- **Compression**: gRPC supports message compression
- **Load Balancing**: Use gRPC-aware load balancers

## Next Steps

After understanding this basic example, explore:

1. **Streaming**: Implement server streaming for real-time data
2. **Authentication**: Add JWT or certificate-based authentication
3. **Interceptors**: Add logging, metrics, or custom middleware
4. **Load Balancing**: Implement client-side load balancing
5. **Health Checks**: Add gRPC health checking
6. **Testing**: Write unit tests for gRPC services

## Troubleshooting

### Common Issues:

1. **Port Already in Use**: Change the port in `Program.cs`
2. **Build Errors**: Ensure all NuGet packages are restored
3. **Connection Refused**: Verify the server is running before starting the client

### Debugging:

- Use tools like [gRPCui](https://github.com/fullstorydev/grpcui) for testing
- Enable detailed logging in your application
- Use browser dev tools for HTTP/2 inspection

## Resources

- [gRPC Official Documentation](https://grpc.io/docs/)
- [.NET gRPC Documentation](https://docs.microsoft.com/en-us/aspnet/core/grpc/)
- [Protocol Buffers Guide](https://developers.google.com/protocol-buffers/docs/overview)
- [gRPC Best Practices](https://grpc.io/docs/guides/best-practices/)

## License

This project is for educational purposes. Feel free to use and modify as needed. 