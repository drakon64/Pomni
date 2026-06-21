{
  pkgs ? import (import ./pomni).nixpkgs { },
}:

pkgs.mkShell {
  packages = with pkgs; [
    dotnetCorePackages.sdk_10_0

    nixfmt
  ];
}
