namespace PartnerCommissions.Wallets.Application.Exceptions;

public class ServiceUnavailableException(string message, Exception? innerException = null) : Exception(message, innerException);
