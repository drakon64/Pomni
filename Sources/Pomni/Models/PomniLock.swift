struct PomniLock: Codable {
  let url: String
  let hash: String
}

typealias PomniLocks = [String: PomniLock]
