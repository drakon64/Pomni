using System.Diagnostics;
using Pomni.Model;
using static Pomni.Commands.Update.Update;

namespace Pomni.Commands.Bot;

internal static partial class Bot
{
    private const string Title = "pomni: update pins";

    public static async Task BotCommand(Forge forge)
    {
        var gitCheckout = new ProcessStartInfo
        {
            FileName = "git",
            ArgumentList = { "checkout", "-B", "pomni/bot" },
            RedirectStandardError = true,
        };

        var gitCheckoutProcess = Process.Start(gitCheckout);

        await gitCheckoutProcess.WaitForExitAsync();

        if (gitCheckoutProcess.ExitCode != 0)
            throw new Exception(await gitCheckoutProcess.StandardError.ReadToEndAsync());

        await UpdateRepositories();

        var gitDiff = new ProcessStartInfo
        {
            FileName = "git",
            ArgumentList = { "diff", "--quiet", "pomni/pomni.lock.json" },
        };

        var gitDiffProcess = Process.Start(gitDiff);

        await gitDiffProcess.WaitForExitAsync();

        if (gitDiffProcess.ExitCode == 0)
        {
            await Console.Out.WriteLineAsync("All pins up to date.");

            return;
        }

        var gitAdd = new ProcessStartInfo
        {
            FileName = "git",
            ArgumentList = { "add", "pomni/pomni.lock.json" },
            RedirectStandardError = true,
        };

        var gitAddProcess = Process.Start(gitAdd);

        await gitAddProcess.WaitForExitAsync();

        if (gitAddProcess.ExitCode != 0)
            throw new Exception(await gitAddProcess.StandardError.ReadToEndAsync());

        var gitCommit = new ProcessStartInfo
        {
            FileName = "git",
            ArgumentList = { "commit", "-m", "pomni: update pins" },
            RedirectStandardError = true,
        };

        var gitCommitProcess = Process.Start(gitCommit);

        await gitCommitProcess.WaitForExitAsync();

        if (gitCommitProcess.ExitCode != 0)
            throw new Exception(await gitCommitProcess.StandardError.ReadToEndAsync());

        var gitPush = new ProcessStartInfo
        {
            FileName = "git",
            ArgumentList = { "push", "--set-upstream", "origin", "--force", "pomni/bot" },
            RedirectStandardError = true,
        };

        var gitPushProcess = Process.Start(gitPush);

        await gitPushProcess.WaitForExitAsync();

        if (gitPushProcess.ExitCode != 0)
            throw new Exception(await gitPushProcess.StandardError.ReadToEndAsync());

        switch (forge)
        {
            case Forge.GitHub:
                await GitHub.RaiseOrModifyPullRequest();
                break;
        }
    }
}
