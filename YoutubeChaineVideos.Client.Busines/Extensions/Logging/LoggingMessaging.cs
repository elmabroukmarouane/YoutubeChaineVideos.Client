namespace YoutubeChaineVideos.Client.Busines.Extensions.Logging;
public static class LoggingMessaging
{
    public static string LoggingMessageCritical(string? level = "", string nameSpaceName = "", int statusCodeInt = 0, string statusCode = "", string httpContextRequestMethod = "", string controllerName = "", string actionName = "", Exception? exception = null)
    {
        return "[" + level + "]" + " [" + DateTimeOffset.UtcNow.ToString("dd/MM/yyyy HH:mm:ss+00:00") + "] [" + statusCodeInt + " - " + statusCode + "] [" + httpContextRequestMethod + "] [" + nameSpaceName + "." + controllerName + actionName + "]: Exception => " + exception + " : " + exception?.Message;
    }
    public static string LoggingMessageError(string? level = "", string nameSpaceName = "", int statusCodeInt = 0, string statusCode = "", string httpContextRequestMethod = "", string controllerName = "", string actionName = "", Exception? exception = null)
    {
        return "[" + level + "]" + " [" + DateTimeOffset.UtcNow.ToString("dd/MM/yyyy HH:mm:ss+00:00") + "] [" + statusCodeInt + " - " + statusCode + "] [" + httpContextRequestMethod + "] [" + nameSpaceName + "." + controllerName + actionName + "]: Exception => " + exception + " : " + exception?.Message;
    }
    public static string LoggingMessageWarning(string? level = "", string nameSpaceName = "", int statusCodeInt = 0, string statusCode = "", string httpContextRequestMethod = "", string controllerName = "", string actionName = "", object? message = null)
    {
        return "[" + level + "]" + " [" + DateTimeOffset.UtcNow.ToString("dd/MM/yyyy HH:mm:ss+00:00") + "] [" + statusCodeInt + " - " + statusCode + "] [" + httpContextRequestMethod + "] [" + nameSpaceName + "." + controllerName + actionName + "]: " + message;
    }
    public static string LoggingMessageInformation(string? level = "", string nameSpaceName = "", int statusCodeInt = 0, string statusCode = "", string httpContextRequestMethod = "", string controllerName = "", string actionName = "", object? message = null)
    {
        return "[" + level + "]" + " [" + DateTimeOffset.UtcNow.ToString("dd/MM/yyyy HH:mm:ss+00:00") + "] [" + statusCodeInt + " - " + statusCode + "] [" + httpContextRequestMethod + "] [" + nameSpaceName + "." + controllerName + actionName + "]: " + message;
    }
    public static string LoggingMessageDebug(string? level = "", string nameSpaceName = "", int statusCodeInt = 0, string statusCode = "", string httpContextRequestMethod = "", string controllerName = "", string actionName = "", object? message = null)
    {
        return "[" + level + "]" + " [" + DateTimeOffset.UtcNow.ToString("dd/MM/yyyy HH:mm:ss+00:00") + "] [" + statusCodeInt + " - " + statusCode + "] [" + httpContextRequestMethod + "] [" + nameSpaceName + "." + controllerName + actionName + "]: " + message;
    }
    public static string LoggingMessageTrace(string? level = "", string nameSpaceName = "", int statusCodeInt = 0, string statusCode = "", string httpContextRequestMethod = "", string controllerName = "", string actionName = "", object? message = null)
    {
        return "[" + level + "]" + " [" + DateTimeOffset.UtcNow.ToString("dd/MM/yyyy HH:mm:ss+00:00") + "] [" + statusCodeInt + " - " + statusCode + "] [" + httpContextRequestMethod + "] [" + nameSpaceName + "." + controllerName + actionName + "]: " + message;
    }
}
