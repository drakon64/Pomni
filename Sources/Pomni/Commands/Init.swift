import ArgumentParser
import Foundation

extension Pomni {
  struct Init: ParsableCommand {
    static let configuration = CommandConfiguration(abstract: "Create Pomni files")

    mutating func run() throws {
      let fileManager = FileManager.default
      let path = fileManager.currentDirectoryPath + "/pomni"
      try? fileManager.createDirectory(atPath: path, withIntermediateDirectories: false)

      try! "{}".write(toFile: path + "/pomni.lock.json", atomically: true, encoding: .utf8)

      try! String(decoding: Data(PackageResources.default_nix), as: UTF8.self).write(
        toFile: path + "/default.nix", atomically: true, encoding: .utf8)

      let jsonEncoder = JSONEncoder()
      jsonEncoder.outputFormatting = .prettyPrinted
      let pomniJson = try! jsonEncoder.encode(PomniJson(version: 1, pins: [:]).self)
      fileManager.createFile(atPath: path + "/pomni.json", contents: pomniJson)
    }
  }
}
