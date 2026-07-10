using System.CommandLine;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Pomni.Authorization;
using Pomni.Client.GitHub;
using Pomni.Commands;
using Pomni.Commands.Bot;
using Pomni.Commands.Update;
using Pomni.Model;

namespace Pomni;

class Program
{
    private static readonly BearerAuthenticationProvider AuthProvider = new()
    {
        ApiKey = Environment.GetEnvironmentVariable("BEARER_TOKEN"),
    };

    private static readonly HttpClientRequestAdapter Adapter = new(AuthProvider);
    internal static readonly Lazy<GitHubClient> GitHubClient = new(() => new GitHubClient(Adapter));

    private static int Main(string[] args)
    {
        var rootCommand = new RootCommand("Nix dependency pinning");

        var initCommand = new Command("init", "Create Pomni files");
        initCommand.SetAction(_ => Init.InitPomniJson());
        rootCommand.Add(initCommand);

        var nameArgument = new Argument<string>("name") { Description = "The name of the pin" };

        var forgeArgument = new Argument<Forge>("forge")
        {
            Description = "The Git forge of the pin",
        };

        var repositoryArgument = new Argument<string>("repository")
        {
            Description = "The Git repository of the pin",
        };

        var branchOption = new Option<string>("-b", "--branch")
        {
            Description = "The branch of the Git repository to use for the pin",
        };

        var referenceTypeOption = new Option<ReferenceType?>("-t", "--type")
        {
            Description = "Whether the pin should track a Git branch or releases",
        };

        var frozenOption = new Option<bool>("-f", "--frozen")
        {
            Description = "Prevent the pin being updated by the `update` command",
        };

        var addCommand = new Command("add", "Add a new pin");
        addCommand.Arguments.Add(nameArgument);
        addCommand.Arguments.Add(forgeArgument);
        addCommand.Arguments.Add(repositoryArgument);
        addCommand.Options.Add(branchOption);
        addCommand.Options.Add(referenceTypeOption);
        addCommand.Options.Add(frozenOption);
        addCommand.SetAction(parseResult =>
            Add.AddRepository(
                parseResult.GetRequiredValue(nameArgument),
                parseResult.GetRequiredValue(forgeArgument),
                parseResult.GetRequiredValue(repositoryArgument),
                parseResult.GetValue(branchOption),
                parseResult.GetValue(referenceTypeOption),
                parseResult.GetValue(frozenOption)
            )
        );
        rootCommand.Add(addCommand);

        var updateCommand = new Command("update", "Update pins to their latest commit or release");
        updateCommand.SetAction(_ => Update.UpdateRepositories());
        rootCommand.Add(updateCommand);

        var modifyCommand = new Command("modify", "Modify an existing pin");

        var forgeOption = new Option<Forge>("-f", "--forge")
        {
            Description = "The Git forge of the pin",
        };

        var repositoryOption = new Option<string>("-r", "--repository")
        {
            Description = "The Git repository of the pin",
        };

        modifyCommand.Arguments.Add(nameArgument);
        modifyCommand.Options.Add(forgeOption);
        modifyCommand.Options.Add(repositoryOption);
        modifyCommand.Options.Add(branchOption);
        modifyCommand.Options.Add(referenceTypeOption);
        modifyCommand.Options.Add(frozenOption);
        modifyCommand.SetAction(parseResult =>
            Modify.ModifyRepository(
                parseResult.GetRequiredValue(nameArgument),
                parseResult.GetValue(forgeOption),
                parseResult.GetValue(repositoryOption),
                parseResult.GetValue(branchOption),
                parseResult.GetValue(referenceTypeOption),
                parseResult.GetValue(frozenOption)
            )
        );
        rootCommand.Add(modifyCommand);

        var removeCommand = new Command("remove", "Remove a pin");
        removeCommand.Arguments.Add(nameArgument);
        removeCommand.SetAction(parseResult =>
            Remove.RemoveRepository(parseResult.GetRequiredValue(nameArgument))
        );
        rootCommand.Add(removeCommand);

        var botCommand = new Command("bot", "Raise a pull request for pin updates");
        botCommand.Arguments.Add(forgeArgument);
        botCommand.SetAction(parseResult =>
            Bot.BotCommand(parseResult.GetRequiredValue(forgeArgument))
        );
        rootCommand.Add(botCommand);

        return rootCommand.Parse(args).Invoke();
    }
}
