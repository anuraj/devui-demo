using System.ClientModel;
using Microsoft.Agents.AI.DevUI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Extensions.AI;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddUserSecrets<Program>();

var token = builder.Configuration["GITHUB_TOKEN"];
if (string.IsNullOrEmpty(token))
{
    throw new InvalidOperationException("GITHUB_TOKEN is not set.");
}

var credential = new ApiKeyCredential(token);
var openAIOptions = new OpenAIClientOptions()
{
    Endpoint = new Uri("https://models.inference.ai.azure.com")
};

var githubModelsClient = new OpenAIClient(credential, openAIOptions);
var chatClient = githubModelsClient.GetChatClient("gpt-4o-mini").AsIChatClient();

// Set up the chat client
builder.Services.AddChatClient(chatClient);

// Register your agents
builder.AddAIAgent("assistant", "You are a helpful assistant.");

// Register services for OpenAI responses and conversations (also required for DevUI)
builder.Services.AddOpenAIResponses();
builder.Services.AddOpenAIConversations();

var app = builder.Build();

// Map endpoints for OpenAI responses and conversations (also required for DevUI)
app.MapOpenAIResponses();
app.MapOpenAIConversations();

// Map DevUI endpoint to /devui
app.MapDevUI();

app.Run();