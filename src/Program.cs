using System.CommandLine;
using Pomni.Commands;
using pomni.Model;

namespace Pomni;

class Program
{
    private static int Main(string[] args)
    {
        var rootCommand = new RootCommand("Nix dependency pinning");

        var initCommand = new Command("init");
        initCommand.SetAction(_ => Commands.Init.InitPomniJson());
        rootCommand.Add(initCommand);

        var nameArgument = new Argument<string>("name");
        var forgeArgument = new Argument<Forge>("forge");
        var repositoryArgument = new Argument<string>("repository");

        var addCommand = new Command("add");
        addCommand.Arguments.Add(nameArgument);
        addCommand.Arguments.Add(forgeArgument);
        addCommand.Arguments.Add(repositoryArgument);
        addCommand.SetAction(parseResult =>
            Add.AddRepository(
                parseResult.GetValue(nameArgument),
                parseResult.GetValue(forgeArgument),
                parseResult.GetValue(repositoryArgument)
            )
        );
        rootCommand.Add(addCommand);

        var updateCommand = new Command("update");
        rootCommand.Add(updateCommand);

        var botCommand = new Command("bot");
        rootCommand.Add(botCommand);

        return rootCommand.Parse(args).Invoke();
    }
}
