import ArgumentParser

extension Pomni {
  struct Bot: ParsableCommand {
    static let configuration = CommandConfiguration(
      abstract: "Raise a pull request for pin updates")
  }
}
