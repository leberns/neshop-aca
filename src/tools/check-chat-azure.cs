// Check the Azure OpenAI connection from the local machine.
// Update userEmail and accessToken after first run, see below for details.
// Execute the program with:
// dotnet run check-chat-azure.cs

#:package Azure.AI.OpenAI@2.1.0
#:package Azure.Identity@1.17.1

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;
using OpenAI.Chat;

using static System.Environment;
using System.Text.Json;
using System.Text.Json.Serialization;

async Task RunAsync()
{
    // Retrieve the OpenAI endpoint from environmen t variables
    var endpoint = GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? "https://swedencentral.api.cognitive.microsoft.com/openai/deployments/chat-model/chat/completions?api-version=2025-01-01-preview";
    if (string.IsNullOrEmpty(endpoint))
    {
        Console.WriteLine("Please set the AZURE_OPENAI_ENDPOINT environment variable.");
        return;
    }

    // Use DefaultAzureCredential for Entra ID authentication
    var credential = new DefaultAzureCredential();

    // Initialize the AzureOpenAIClient
    var azureClient = new AzureOpenAIClient(new Uri(endpoint), credential);

    // Initialize the ChatClient with the specified deployment name
    ChatClient chatClient = azureClient.GetChatClient("chat-model");
    
    // Create a list of chat messages
    var messages = new List<ChatMessage>
    {
        new SystemChatMessage(@"You are an AI assistant that helps people find information."),
    };

    // Create chat completion options
    var options = new ChatCompletionOptions{
        Temperature = (float)0.7,
        MaxOutputTokenCount = 6553,
        
        TopP=(float)0.95,
        FrequencyPenalty=(float)0,
        PresencePenalty=(float)0
    };

    try
    {
        Console.WriteLine("Sending request to Azure OpenAI...");

        // Create the chat completion request
        ChatCompletion completion = await chatClient.CompleteChatAsync(messages, options);

        // Print the response
        if (completion != null)
        {
            Console.WriteLine($"Response: {completion.Content[0].Text}");
            Console.WriteLine($"Model: {completion.Model}");
            Console.WriteLine($"Finish Reason: {completion.FinishReason}");
            Console.WriteLine($"Tokens - Prompt: {completion.Usage.InputTokenCount}, Completion: {completion.Usage.OutputTokenCount}, Total: {completion.Usage.TotalTokenCount}");
        }
        else
        {
            Console.WriteLine("No response received.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

await RunAsync();
