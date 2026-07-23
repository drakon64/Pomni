import ArgumentParser

extension Pomni {
    struct Init: ParsableCommand {
        static let configuration = CommandConfiguration(abstract: "Create Pomni files")
    }
}
