using FluentValidation;
using MediatR;
using Peng.SharedKernel.Application;

namespace Peng.SharedKernel.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f is not null).ToList();

        if (failures.Count != 0)
        {
            var error = new Error("Validation.Failed", string.Join("; ", failures.Select(f => f.ErrorMessage)));

            // Result<T> requires generic Failure<T>(); plain Result uses non-generic Failure().
            var responseType = typeof(TResponse);
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var valueType = responseType.GetGenericArguments()[0];
                var result = typeof(Result)
                    .GetMethod(nameof(Result.Failure), 1, [typeof(Error)])!
                    .MakeGenericMethod(valueType)
                    .Invoke(null, [error])!;
                return (TResponse)result;
            }

            return (TResponse)(object)Result.Failure(error);
        }

        return await next(cancellationToken);
    }
}
