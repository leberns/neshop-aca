using OpenAI.Chat;
using Azure.Identity;
using Azure.AI.OpenAI;

var endpoint = new Uri("https://cog-neshop1.openai.azure.com/");
const string deploymentName = "chat-model"; // deployment name as per Bicep infrastructure

AzureOpenAIClient azureClient = new(
    endpoint,
    new DefaultAzureCredential());

ChatClient chatClient = azureClient.GetChatClient(deploymentName);

var requestOptions = new ChatCompletionOptions()
{
    MaxOutputTokenCount = 32,
    Temperature = 0.2f,
    TopP = 1.0f
};

List<ChatMessage> messages =
[
    new SystemChatMessage(
        """
        You are a helpful assistant.
        You respond to questions about the outdoor products online shop called NeShop.
        Respond questions in one short sentence.
        If you do not know an answer, respond 'I don't know.'
        """),
    new UserChatMessage("Which kind of products does NeShop sell?")
];

var completion = await chatClient.CompleteChatAsync(messages, requestOptions);

var response = completion.Value ?? throw new Exception("Response is null");

Console.WriteLine($"Response: {response.Content[0].Text}");
Console.WriteLine($"Model: {response.Model}");
Console.WriteLine($"Finish Reason: {response.FinishReason}");
Console.WriteLine($"Tokens:");
Console.WriteLine($"  prompt: {response.Usage.InputTokenCount}");
Console.WriteLine($"  completion: {response.Usage.OutputTokenCount}");
Console.WriteLine($"  total: {response.Usage.TotalTokenCount}");
Console.WriteLine($"Done.");

// Output example:
// Response: NeShop sells a variety of outdoor products.
// Model: gpt-4o-mini-2024-07-18
// Finish Reason: Stop
// Tokens:
//   prompt: 61
//   completion: 10
//   total: 71
// Done.
