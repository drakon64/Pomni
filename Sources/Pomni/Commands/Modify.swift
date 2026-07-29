import ArgumentParser
import Foundation

extension Pomni {
  struct Modify: ParsableCommand {
    static let configuration = CommandConfiguration(abstract: "Modify an existing pin")

    @Argument(help: "The name of the pin")
    var name: String

    @Option(help: "The Git forge of the pin")
    var forge: Forge?

    @Option(help: "The Git repository of the pin")
    var repository: String?

    @Option(name: .shortAndLong, help: "Whether the pin should track a Git branch or releases")
    var type: PinType?

    @Option(name: .shortAndLong, help: "The branch of the Git repository to use for the pin")
    var branch: String?

    @Option(name: .shortAndLong, help: "Prevent the pin being updated by the `update` command")
    var frozen: Bool?

    mutating func run() throws {
      // Read the existing `pomni.json` file
      var pomniJson = try JSONDecoder().decode(
        PomniJson.self, from: Data(try String(contentsOfFile: "pomni/pomni.json").utf8))

      if forge != nil { pomniJson.pins[name]!.forge = forge! }
      if repository != nil { pomniJson.pins[name]!.repository = repository! }
      if type != nil { pomniJson.pins[name]!.type = type }
      if branch != nil { pomniJson.pins[name]!.branch = branch! }
      if frozen != nil { pomniJson.pins[name]!.frozen = frozen! }

      // Write the updated `pomni.json` file
      jsonEncoder.outputFormatting = [.prettyPrinted, .sortedKeys]
      FileManager.default.createFile(
        atPath: "pomni/pomni.json", contents: try! jsonEncoder.encode(pomniJson.self))
    }
  }
}
