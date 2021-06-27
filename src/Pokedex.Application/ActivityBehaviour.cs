using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Pokedex.Application
{
    public class ActivityBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using var activity = Activity.Current?.Source.StartActivity(typeof(TRequest).Name);
            activity?.Start();
            var result = await next();
            activity?.Stop();
            return result;
        }
    }
}