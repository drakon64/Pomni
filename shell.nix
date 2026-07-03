{
  pkgs ? import (import ./pomni).nixpkgs { },
}:

pkgs.mkShellNoCC {
  packages = with pkgs; [
    dotnetCorePackages.sdk_10_0

    nixfmt
  ];

  passthru.pomni = pkgs.mkShellNoCC {
    packages = [ (pkgs.callPackage ./package.nix { }) ];
  };
}
