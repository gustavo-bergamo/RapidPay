using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using RapidPay.CrossProject.Exceptions;
using System.Text.RegularExpressions;

namespace RapidPay.Data.Exceptions;

public class DuplicatedKeyException : UserFriendlyException
{
    private const string ErrorMessagePattern = @"Cannot insert duplicated key row in object \'(?<tableName>.+)\' with unique index \'?<indexName>\'. Duplicated key value is \((?<values>.+)\)\.>";
    private const string GenericMessage = "Key already in use";

    public DuplicatedKeyException()
    {

    }

    public DuplicatedKeyException(ILogger logger, Exception exception)
    {
        if (exception.InnerException is SqlException sqlException)
        {
            var message = sqlException.Message;
            var matches = Regex.Match(message, ErrorMessagePattern);

            if (matches.Groups.Count >= 3)
            {
                var tableName = matches.Groups["tableName"].Value;
                var indexName = matches.Groups["indexName"].Value;
                var values = matches.Groups["values"].Value;
                var individualValues = values.Split(", ");

                if (!string.IsNullOrEmpty(tableName) &&
                   !string.IsNullOrEmpty(indexName) &&
                   !string.IsNullOrEmpty(values))
                {
                    var displayValues = ExtractDuplicatedKeyValue(individualValues);
                    AddMessage(indexName, displayValues);

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

    private string ExtractDuplicatedKeyValue(string[] values)
    {
        if (values.Length == 0)
        {
            return string.Empty;
        }

        if (values.Length == 1)
        {
            return values[0];
        }

        return string.Join(", ", values);
    }
}
