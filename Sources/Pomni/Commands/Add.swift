import ArgumentParser

extension Pomni {
  struct Add: ParsableCommand {
    static let configuration = CommandConfiguration(abstract: "Add a new pin")
  }
}
