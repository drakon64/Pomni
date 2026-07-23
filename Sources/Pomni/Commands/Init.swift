import ArgumentParser
import Foundation

extension Pomni {
  struct Init: ParsableCommand {
    static let configuration = CommandConfiguration(abstract: "Create Pomni files")

    mutating func run() throws {
      let fileManager = FileManager.default
      let path = fileManager.currentDirectoryPath + "/pomni"
      try? fileManager.createDirectory(atPath: path, withIntermediateDirectories: false)

      // Write an empty JSON document to `pomni.lock.json`, the `update` subcommand will populate it later
      try! "{}".write(toFile: path + "/pomni.lock.json", atomically: true, encoding: .utf8)

      // Write the `default.nix` file from the one embedded by SwiftPM stored at `Resources/default.nix`
      try! String(decoding: Data(PackageResources.default_nix), as: UTF8.self).write(
        toFile: path + "/default.nix", atomically: true, encoding: .utf8)

      // Write the `pomni.json` file by JSON encoding the `PomniJson` class with some default values
      // `pins` will be populated by the `add` command once the file is written
      let jsonEncoder = JSONEncoder()
      jsonEncoder.outputFormatting = .prettyPrinted
      let pomniJson = try! jsonEncoder.encode(PomniJson(version: 1, pins: [:]).self)
      fileManager.createFile(atPath: path + "/pomni.json", contents: pomniJson)
    }
  }
}
