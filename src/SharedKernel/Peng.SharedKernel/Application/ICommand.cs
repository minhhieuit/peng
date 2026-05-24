using MediatR;

namespace Peng.SharedKernel.Application;

public interface ICommand : IRequest<Result> { }
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
