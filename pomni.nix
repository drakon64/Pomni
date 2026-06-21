let
  pins = builtins.fromJSON (builtins.readFile ./pomni.lock.json);
in
builtins.mapAttrs (
  name: args:
  (builtins.fetchTarball {
    url = args.url;
    sha256 = args.hash;
  })
) pins
