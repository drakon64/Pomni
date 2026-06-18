using System.CommandLine;
using Pomni.Commands;

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
        var urlArgument = new Argument<string>("url");

        var addCommand = new Command("add");
        addCommand.Arguments.Add(nameArgument);
        addCommand.Arguments.Add(urlArgument);
        addCommand.SetAction(parseResult =>
            Add.AddRepository(parseResult.GetValue(nameArgument), parseResult.GetValue(urlArgument))
        );
        rootCommand.Add(addCommand);

        var updateCommand = new Command("update");
        rootCommand.Add(updateCommand);

        var botCommand = new Command("bot");
        rootCommand.Add(botCommand);

        return rootCommand.Parse(args).Invoke();
    }
}
