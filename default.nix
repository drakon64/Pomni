{
  pkgs ? import (import ./pomni).nixpkgs { },
}:

pkgs.callPackage ./package.nix { }
