using Microsoft.Extensions.Logging;

namespace AiClient.AzureChat;

public partial class AzureChatService
{
    [LoggerMessage(LogLevel.Information, "Generated response, finishReason {finishReason}, inputTokenCount {inputTokenCount}, outputTokenCount {outputTokenCount}")]
    static partial void LogResponseStats(ILogger<AzureChatService> logger, string finishReason, int inputTokenCount, int outputTokenCount);
}