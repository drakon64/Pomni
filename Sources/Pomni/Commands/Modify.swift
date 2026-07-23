import ArgumentParser

extension Pomni {
  struct Modify: ParsableCommand {
    static let configuration = CommandConfiguration(abstract: "Modify an existing pin")
  }
}
