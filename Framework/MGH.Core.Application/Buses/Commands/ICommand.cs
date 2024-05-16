using MediatR;
using MGH.Core.Application.Pipelines.Logging;

namespace MGH.Core.Application.Buses.Commands;
public interface ICommand : IRequest
{
}
public interface ICommand<out TResult> : IRequest<TResult>
{
}


