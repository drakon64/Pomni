import Foundation
import GitHubClient
import OpenAPIRuntime
import OpenAPIURLSession

struct GitHub {
  let client = Client(serverURL: try! Servers.Server1.url(), transport: URLSessionTransport())

  static func Update(pin: Pin) -> PomniLock {
    let repo = pin.repository.components(separatedBy: "/")
    let sha: String

    switch pin.type {
    case .branch?, .none: let branch: String
    case .release?: let tag: String
    }

    let url = ""

    return PomniLock(url: url, hash: "")
  }
}
