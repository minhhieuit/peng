using MediatR;

namespace Peng.SharedKernel.Application;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
