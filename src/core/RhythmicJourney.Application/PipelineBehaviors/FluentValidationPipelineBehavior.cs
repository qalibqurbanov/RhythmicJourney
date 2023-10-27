using MediatR;
using System.Linq;
using System.Threading;
using FluentValidation;
using System.Threading.Tasks;
using FluentValidation.Results;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.PipelineBehaviors;

/// <summary>
/// ('command' ve ya 'query' temsilcisi olan) Request handler iwlemezden evvel Validation tetbiq eden behavior-dur.
/// </summary>
/// <typeparam name="TRequest">Pipeline-dan kecen ('command' ve ya 'query' temsilcisi olan) Request tipimizi/sinifimizi temsil edir.</typeparam>
/// <typeparam name="TResponse">('command' ve ya 'query' temsilcisi olan) Request-in geriye dondurduyu responsun tipini temsil edir.</typeparam>
public class FluentValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : AuthenticationResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public FluentValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) => this._validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        /* Her hansi bir validator verilmeyibse skiple 'FluentValidationPipelineBehavior'-un 'pre' addimini: */
        if (!_validators.Any())
        {
            /* Appin iwleyiwin otururuk siradaki addima: */
            return await next();
        }

        /* Validasiyanin hansi kontekste tetbiq olundugunu temsil edir ve ya bawqa sozle validasiya edilmek istenen tipi temsil edir: */
        ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);

        List<ValidationFailure> failures = _validators
            /* Her bir ('Command' ve ya 'Query') 'request' sinif obyektini validasiyadan keciririk. Geriye validasiyalarin neticesini 'List<ValidationResult>' olaraq dondurecek: */
            .Select(validator => validator.Validate(context))

            /* Geriye donmuw olan kolleksiyanin her bir elementinin 'Errors'-larini secirem. Geriye validasiyalarin xetalarini 'List<ValidationFailure>' olaraq dondurecek: */
            .SelectMany(validationResult => validationResult.Errors)

            /* Eger xetani temsil eden 'validationFailure' null deyilse elde edirik: hazirda bow olmayan, yeni xeta saxlayan 'ValidationFailure'-lari: */
            .Where(validationFailure => validationFailure != null) /* ve ya "(validationFailure => validationFailure is not null)" */

            /* Tekrarlanan xetaya sahib olanlari silirik: */
            .Distinct()

            /* Son olaraq 'List<ValidationFailure>'-e ceviririk: */
            .ToList();

        /* Eger validasiya zamani her hansi bir xeta var imiwse 'Exception' throw edirik: */
        if (failures.Any())
        {
            List<IdentityError> errors = new List<IdentityError>();

            foreach (ValidationFailure validationFailure in failures)
            {
                errors.Add(new IdentityError() { Description = validationFailure.ErrorMessage, Code = validationFailure.ErrorCode });
            }

            return (TResponse)await AuthenticationResult.FailureAsync(errors);
        }

        /* Appin iwleyiwin otururuk siradaki addima: */
        return await next();
    }
}