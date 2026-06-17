using System.CommandLine;

namespace Pomni;

class Program
{
    private static int Main(string[] args)
    {
        var rootCommand = new RootCommand("Nix dependency pinning");
        
        var initCommand = new Command("init");
        rootCommand.Add(initCommand);
        
        var addCommand = new Command("add");
        rootCommand.Add(addCommand);
        
        var updateCommand = new Command("update");
        rootCommand.Add(updateCommand);
        
        var modifyCommand = new Command("modify");
        rootCommand.Add(modifyCommand);
        
        var removeCommand = new Command("remove");
        rootCommand.Add(removeCommand);
        
        var freezeCommand = new Command("freeze");
        rootCommand.Add(freezeCommand);
        
        var unfreezeCommand = new Command("unfreeze");
        rootCommand.Add(unfreezeCommand);
        
        var botCommand = new Command("bot");
        rootCommand.Add(botCommand);
        
        return rootCommand.Parse(args).Invoke();
    }
}
