import ArgumentParser

extension Pomni {
  struct Update: ParsableCommand {
    static let configuration = CommandConfiguration(
      abstract: "Update pins to their latest commit or release")
  }
}
