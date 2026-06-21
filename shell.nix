{
  pkgs ? import (import ./pomni).nixpkgs { },
}:

pkgs.mkShell {
  packages = [ pkgs.dotnetCorePackages.sdk_10_0 ];
}
