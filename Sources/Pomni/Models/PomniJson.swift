import ArgumentParser

struct PomniJson: Codable {
  let version: Int
  var pins: [String: Pin] = [:]
}

struct Pin: Codable {
  var forge: Forge
  var repository: String
  var type: PinType?
  var branch: String?
  var frozen: Bool
}

enum Forge: String, Codable, ExpressibleByArgument {
  case forgejo, github
}

enum PinType: String, Codable, ExpressibleByArgument {
  case branch, release
}
