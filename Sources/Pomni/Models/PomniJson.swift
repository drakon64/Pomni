struct PomniJson: Codable {
  let version: Int
  let pins: [String: Pin]
}

struct Pin: Codable {
  let forge: Forge
  let repository: String
  let pinType: PinType?
  let branch: String?
  let frozen: Bool?
}

enum Forge: Codable {
  case Forgejo
  case GitHub
}

enum PinType: Codable {
  case Branch
  case Release
}
