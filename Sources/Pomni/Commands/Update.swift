import ArgumentParser
import Foundation

extension Pomni {
  struct Update: ParsableCommand {
    static let configuration = CommandConfiguration(
      abstract: "Update pins to their latest commit or release")

    mutating func run() throws {
      // Read the `pomni.json` file
      let pomniPins = try! JSONDecoder().decode(
        PomniJson.self, from: Data(try! String(contentsOfFile: "pomni/pomni.json").utf8))

      // Read the `pomni.lock.json` file
      let pomniLocks = try! JSONDecoder().decode(
        PomniLocks.self, from: Data(try! String(contentsOfFile: "pomni/pomni.lock.json").utf8))

      for pin in pomniPins.pins {
        if !pin.value.frozen! {
          switch pin.value.forge {
          case .forgejo:
            print("")
          case .github:
            GitHub.Update(pin: pin.value)
          }
        }
      }
    }
  }
}
