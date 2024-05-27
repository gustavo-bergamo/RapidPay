using System.Collections.Immutable;
using System.Diagnostics.Contracts;

namespace RapidPay.CrossProject.Exceptions;

public class UserFriendlyException : Exception
{
    private readonly Dictionary<string, IList<string>> _messages = new();
    public UserFriendlyException() { }
    public UserFriendlyException(string message)
    {
        AddMessage(string.Empty, message);
    }

    public UserFriendlyException(string key, string message)
    {
        AddMessage(key, message);
    }

    public UserFriendlyException(string key, IEnumerable<string> messages)
    {
        foreach (var message in messages)
        {
            AddMessage(key, message);
        }
    }

    public void AddMessage(KeyValuePair<string, string> message)
    {
        AddMessage(message.Key, message.Value);
    }

    public void AddMessage(string key, string message)
    {
        Contract.Requires(key != null);
        Contract.Requires(!string.IsNullOrEmpty(message));

        if (_messages.ContainsKey(key))
        {
            _messages[key].Add(message);
        }
        else
        {
            _messages.Add(key, [message]);
        }
    }

    public IDictionary<string, IList<string>> GetMessages()
    {
        return _messages;
    }

    public ImmutableDictionary<string, string> GetMessage()
    {
        var message = _messages.First();
        var returnMessage = new Dictionary<string, string>
        {
            { message.Key, message.Value.First() }
        }.ToImmutableDictionary();

        return returnMessage;
    }
}