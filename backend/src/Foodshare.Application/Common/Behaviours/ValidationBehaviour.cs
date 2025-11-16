using FluentValidation;
using Foodshare.Application.Common.Models;
using MediatR;

namespace Foodshare.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(
                    new ValidationContext<TRequest>(request), 
                    cancellationToken))
            );

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count > 0)
            {
                var errors = failures.Select(f => $"{f.ErrorMessage}").ToArray();

                var resultType = typeof(TResponse);

                if (resultType.IsGenericType &&
                    resultType.GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var dataType = resultType.GetGenericArguments()[0];
                    var failureMethod = typeof(Result<>)
                        .MakeGenericType(dataType)
                        .GetMethod("Failure", [typeof(IEnumerable<string>)]);

                    return (TResponse)failureMethod!.Invoke(null, [errors])!;
                }

                throw new ValidationException(failures);
            }
        }

        return await next(cancellationToken);
    }
}