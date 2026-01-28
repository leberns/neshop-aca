using OpenAI.Chat;
using Azure.Identity;
using Azure.AI.OpenAI;

var endpoint = new Uri("https://cog-wiok5xjx2aqja.openai.azure.com/");
var deploymentName = "chat-model";

AzureOpenAIClient azureClient = new(
    endpoint,
    new DefaultAzureCredential());

ChatClient chatClient = azureClient.GetChatClient(deploymentName);

var requestOptions = new ChatCompletionOptions()
{
    MaxOutputTokenCount = 4096,
    Temperature = 1.0f,
    TopP = 1.0f,

};

List<ChatMessage> messages = new List<ChatMessage>()
{
    new SystemChatMessage("You are a helpful assistant."),
    new UserChatMessage("I am going to Paris, what should I see?"),
};

var response = chatClient.CompleteChat(messages, requestOptions);
System.Console.WriteLine(response.Value.Content[0].Text);