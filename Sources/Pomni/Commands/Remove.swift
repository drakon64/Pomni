import ArgumentParser

extension Pomni {
  struct Remove: ParsableCommand {
    static let configuration = CommandConfiguration(abstract: "Remove a pin")
  }
}
