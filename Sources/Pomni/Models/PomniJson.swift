import ArgumentParser

struct PomniJson: Codable {
  let version: Int
  var pins: [String: Pin] = [:]
}

struct Pin: Codable {
  let forge: Forge
  let repository: String
  let type: PinType?
  let branch: String?
  let frozen: Bool
}

enum Forge: String, Codable, ExpressibleByArgument {
  case forgejo, github
}

enum PinType: String, Codable, ExpressibleByArgument {
  case branch, release
}
