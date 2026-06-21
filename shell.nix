with import (import ./pomni).nixpkgs { };

mkShell {
  packages = [ dotnetCorePackages.sdk_10_0 ];
}
