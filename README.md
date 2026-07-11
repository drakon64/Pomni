# Pomni

Nix dependency locking and updating, inspired by [Lon](https://github.com/nikstur/lon) and [tack](https://github.com/manic-systems/tack).

## Features

- Management via CLI or hand-editable `pomni.json` file
- Resolves tarball URLs and Git revisions via GitHub API
- Supports pinning Git branches or GitHub releases

## Usage

```
Description:
  Lock & update Nix dependencies

Usage:
  pomni [command] [options]

Options:
  -?, -h, --help  Show help and usage information
  --version       Show version information

Commands:
  init                              Create Pomni files
  add <name> <GitHub> <repository>  Add a new pin
  update                            Update pins to their latest commit or release
  modify <name>                     Modify an existing pin
  remove <name>                     Remove a pin
  bot <GitHub>                      Raise a pull request for pin updates
```

### Adding a pin

```shell
pomni add nixpkgs github NixOS/nixpkgs --branch nixpkgs-unstable
pomni add lix github lix-project/lix # Uses the default branch of the repository
pomni add crane github ipetkov/crane --type release # Uses the latest GitHub release within the repository

pomni update # Required to sync `pomni.json` with `pomni.lock.json`
```

### Updating pins

```shell
pomni update
```

### Updating pins and raising a pull request via GitHub Actions

```yaml
- run: pomni bot github
  env:
    BEARER_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

## Using from Nix

```nix
let
  pomni = import ./pomni;
in
let
  pkgs = import pomni.nixpkgs { };
  lix = import pomni.lix;
  crane = import pomni.crane;
in
```

## License

EUPL v. 1.2 only.
