namespace Peng.SharedKernel.Exceptions;

public class DomainException(string message) : Exception(message);
public class NotFoundException(string message) : Exception(message);
public class ForbiddenException(string message = "Access denied.") : Exception(message);
public class ConflictException(string message) : Exception(message);
