with import (import ./lon.nix).nixpkgs { };

mkShell {
  packages = [ dotnetCorePackages.sdk_10_0 ];
}
