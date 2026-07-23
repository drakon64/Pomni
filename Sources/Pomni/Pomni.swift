import ArgumentParser

@main
struct Pomni: ParsableCommand {
    static let configuration = CommandConfiguration(subcommands: [
        Init.self,
        Add.self,
        Update.self,
        Modify.self,
        Remove.self,
        Bot.self,
    ])
}
