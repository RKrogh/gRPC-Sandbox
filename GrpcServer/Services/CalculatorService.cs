using Calculator;
using Grpc.Core;

namespace GrpcServer.Services;

public class CalculatorServiceImpl : CalculatorService.CalculatorServiceBase
{
    private readonly ILogger<CalculatorServiceImpl> _logger;

    public CalculatorServiceImpl(ILogger<CalculatorServiceImpl> logger)
    {
        _logger = logger;
    }

    public override Task<AddResponse> Add(AddRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Add operation requested: {A} + {B}", request.A, request.B);

        var result = request.A + request.B;
        _logger.LogInformation("Add result: {Result}", result);

        return Task.FromResult(new AddResponse { Result = result });
    }

    public override Task<SubtractResponse> Subtract(
        SubtractRequest request,
        ServerCallContext context
    )
    {
        _logger.LogInformation("Subtract operation requested: {A} - {B}", request.A, request.B);

        var result = request.A - request.B;
        _logger.LogInformation("Subtract result: {Result}", result);

        return Task.FromResult(new SubtractResponse { Result = result });
    }

    public override Task<MultiplyResponse> Multiply(
        MultiplyRequest request,
        ServerCallContext context
    )
    {
        _logger.LogInformation("Multiply operation requested: {A} * {B}", request.A, request.B);

        var result = request.A * request.B;
        _logger.LogInformation("Multiply result: {Result}", result);

        return Task.FromResult(new MultiplyResponse { Result = result });
    }

    public override Task<DivideResponse> Divide(DivideRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Divide operation requested: {A} / {B}", request.A, request.B);

        if (request.B == 0)
        {
            _logger.LogWarning("Division by zero attempted");
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Cannot divide by zero"));
        }

        var result = (double)request.A / request.B;
        _logger.LogInformation("Divide result: {Result}", result);

        return Task.FromResult(new DivideResponse { Result = result });
    }
}
