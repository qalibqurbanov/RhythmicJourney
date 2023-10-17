using System;
using MediatR;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.PipelineBehaviors;

/// <summary>
/// ('command' ve ya 'query' temsilcisi olan) Request handler iwlemezden evvel Logging tetbiq eden behavior-dur.
/// </summary>
/// <typeparam name="TRequest">Pipeline-dan kecen ('command' ve ya 'query' temsilcisi olan) Request tipimizi/sinifimizi temsil edir.</typeparam>
/// <typeparam name="TResponse">('command' ve ya 'query' temsilcisi olan) Request-in geriye dondurduyu responsun tipini temsil edir.</typeparam>
public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : AuthenticationResult
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;
    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger) => this._logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        {
            _logger.LogInformation($"\n************************* [{DateTime.UtcNow}] Starting {typeof(TRequest).Name} request. *************************\n");

            Type myType = request.GetType();
            IList<PropertyInfo> requestClassProperties = new List<PropertyInfo>(myType.GetProperties());
            foreach (PropertyInfo prop in requestClassProperties)
            {
                object propValue = prop.GetValue(request, null);
                _logger.LogInformation($"************************* {prop.Name} : {propValue}. *************************");
            }
        }

        var response = await next(); /* Iwleyiwi yonlendiririk varsa siradaki 'Pipeline Behavior'-a, sirada triggerlenmeyi gozleyen 'Pipeline Behavior' yoxdursa birbawa uygun handlere */

        {
            if (!response.IsSuccess)
            {
                _logger.LogInformation($"\n************************* [{DateTime.UtcNow}] Request failure for {typeof(TRequest).Name}: {response.Errors}. *************************\n");
            }

            _logger.LogInformation($"\n************************* [{DateTime.UtcNow}] Request {typeof(TRequest).Name} completed. *************************\n");
        }

        return response;
    }
}