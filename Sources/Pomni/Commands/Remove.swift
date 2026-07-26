import ArgumentParser
import Foundation

extension Pomni {
  struct Remove: ParsableCommand {
    static let configuration = CommandConfiguration(abstract: "Remove a pin")

    @Argument(help: "The name of the pin")
    var name: String

    mutating func run() throws {
      // Read the existing `pomni.json` file
      var pomniJson = try JSONDecoder().decode(
        PomniJson.self, from: Data(try String(contentsOfFile: "pomni/pomni.json").utf8))

      // `removeValue` returns the removed key/value, or nil if the key isn't found
      // Exit with an error if `nil` is returned, otherwise continue
      if pomniJson.pins.removeValue(forKey: name) == nil {
        Pomni.Remove.exit(withError: RemoveError.PinNotFound(name: name))
      }

      // Write the updated `pomni.json` file
      jsonEncoder.outputFormatting = [.prettyPrinted, .sortedKeys]
      FileManager.default.createFile(
        atPath: "pomni/pomni.json", contents: try! jsonEncoder.encode(pomniJson.self))
    }

    enum RemoveError: Error {
      case PinNotFound(name: String)
    }
  }
}
