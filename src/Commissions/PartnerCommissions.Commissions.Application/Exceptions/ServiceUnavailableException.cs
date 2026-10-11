namespace PartnerCommissions.Commissions.Application.Exceptions;

public class ServiceUnavailableException(string message, Exception? innerException = null) : Exception(message, innerException);
