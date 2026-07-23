{
  pkgs ? import (import ./pomni).nixpkgs { },
}:

pkgs.mkShellNoCC {
  packages = with pkgs; [
    nixfmt
  ];

  passthru.pomni = pkgs.mkShellNoCC {
    packages = [ (pkgs.callPackage ./package.nix { }) ];
  };
}
