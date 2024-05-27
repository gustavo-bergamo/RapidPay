using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using RapidPay.CrossProject.Exceptions;
using System.Text.RegularExpressions;

namespace RapidPay.Data.Exceptions;

public class ForeignKeyException : UserFriendlyException
{
    private const string ErrorMessagePattern = "The (?:DELETE|INSERT) statement conflict with the (?:FOREIGN KEY|REFERECE) constraint \"(?<constraintName>.+?)\".";
    private const string GenericMessage = "Key already in use";

    public ForeignKeyException()
    {

    }

    public ForeignKeyException(ILogger logger, Exception exception)
    {
        if (exception.InnerException is SqlException sqlException)
        {
            var message = sqlException.Message;
            var matches = Regex.Match(message, ErrorMessagePattern);

            if (matches.Groups.Count == 2)
            {
                var constraintName = matches.Groups["constraintName"].Value;

                if (!string.IsNullOrEmpty(constraintName))
                {
                    AddMessage("", $"Cannot perform action due dependency between {constraintName}.");
                    return;
                }
            }

            logger.LogWarning($"Either the excpetion message or the index names have been changed, user will no loger get nice error messages: {message}");
            AddMessage("", GenericMessage);

            return;
        }

        logger.LogWarning($"Application was not able to get the correct error message. {exception.Message}");
        AddMessage("", GenericMessage);
    }
}