using Azure;
using Azure.AI.OpenAI;
using Microsoft.Agents.AI.DevUI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
if (string.IsNullOrEmpty(token))
{
    throw new InvalidOperationException("GITHUB_TOKEN is not set.");
}

var endpoint = new Uri("https://models.github.ai/inference");
var credential = new AzureKeyCredential(token);
var model = "openai/gpt-5-mini";

var chatClient = new AzureOpenAIClient(endpoint, credential).GetChatClient(model).AsIChatClient();

// Set up the chat client
builder.Services.AddChatClient(chatClient);

// Register your agents
builder.AddAIAgent("my-agent", "You are a helpful assistant.");

// Register services for OpenAI responses and conversations (also required for DevUI)
builder.Services.AddOpenAIResponses();
builder.Services.AddOpenAIConversations();

var app = builder.Build();

// Map endpoints for OpenAI responses and conversations (also required for DevUI)
app.MapOpenAIResponses();
app.MapOpenAIConversations();

// if (builder.Environment.IsDevelopment())
// {
    // Map DevUI endpoint to /devui
    app.MapDevUI();
// }

app.Run();