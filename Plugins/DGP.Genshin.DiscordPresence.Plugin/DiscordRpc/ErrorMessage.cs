using Newtonsoft.Json;
/// <summary>
/// Created when a error occurs within the ipc and it is sent to the client.
/// </summary>
public class ErrorMessage : IMessage
{
    /// <summary>
    /// The type of message received from discord
    /// </summary>
    public override MessageType Type { get { return MessageType.Error; } }

    /// <summary>
    /// The Discord error code.
    /// </summary>
    [JsonProperty("code")]
    public ErrorCode Code { get; internal set; }

    /// <summary>
    /// The message associated with the error code.
    /// </summary>
    [JsonProperty("message")]
    public string Message { get; internal set; }

}
