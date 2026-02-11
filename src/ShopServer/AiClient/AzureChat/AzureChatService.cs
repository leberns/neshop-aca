using AiClient.Interfaces;
using Azure.AI.OpenAI;
using Contracts;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using ChatMessage = OpenAI.Chat.ChatMessage;

namespace AiClient.AzureChat;

public partial class AzureChatService(
    AzureOpenAIClient clientFactory,
    ILogger<AzureChatService> logger
    ) : IChatService
{
    public async Task<string> RespondAsync(
        string systemMessage,
        string userQuery,
        string assistantMessage,
        CancellationToken cancellationToken)
    {
        var client = clientFactory.GetChatClient(Constants.AiAzureChat.DeploymentName);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemMessage),
            new UserChatMessage(userQuery),
            new AssistantChatMessage(assistantMessage),
        };

        var options = new ChatCompletionOptions()
        {
            MaxOutputTokenCount = 128,
            Temperature = 0.2f
        };

        var completion = await client.CompleteChatAsync(messages, options, cancellationToken);

        var response = completion.Value ?? throw new Exception("The chat response is null");

        LogResponseStats(logger, $"{response.FinishReason}", response.Usage.InputTokenCount, response.Usage.OutputTokenCount);

        return response.Content[0].Text;
    }
}