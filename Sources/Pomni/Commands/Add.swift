import ArgumentParser
import Foundation

extension Pomni {
  struct Add: ParsableCommand {
    static let configuration = CommandConfiguration(abstract: "Add a new pin")

    @Argument(help: "The name of the pin")
    var name: String

    @Argument(help: "The Git forge of the pin")
    var forge: Forge

    @Argument(help: "The Git repository of the pin")
    var repository: String

    @Option(name: .shortAndLong, help: "Whether the pin should track a Git branch or releases")
    var type: PinType = PinType.branch

    @Option(name: .shortAndLong, help: "The branch of the Git repository to use for the pin")
    var branch: String?

    @Flag(name: .shortAndLong, help: "Prevent the pin being updated by the `update` command")
    var frozen: Bool = false

    mutating func run() throws {
      // Read the existing `pomni.json` file
      var pomniJson = try! JSONDecoder().decode(
        PomniJson.self, from: Data(try! String(contentsOfFile: "pomni/pomni.json").utf8))

      // Add the new pin
      pomniJson.pins.updateValue(
        Pin(forge: forge, repository: repository, type: type, branch: branch, frozen: frozen),
        forKey: name)

      // Write the updated `pomni.json` file
      jsonEncoder.outputFormatting = [.prettyPrinted, .sortedKeys]
      FileManager.default.createFile(
        atPath: "pomni/pomni.json", contents: try! jsonEncoder.encode(pomniJson.self))
    }
  }
}
