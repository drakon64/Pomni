struct PomniJson: Codable {
    let version: Int
    let pins: [String: Pin]
}

struct Pin: Codable {
    let forge: Forge
    let repository: String
    let branch: String
}

enum Forge: Codable {
    case Codeberg
    case GitHub
}
