{
  lib,
  buildDotnetModule,
  dotnetCorePackages,
  stdenv,
  darwin,
}:

buildDotnetModule (finalAttrs: {
  pname = "pomni";
  version = "1.1.0";

  src = ./src;

  strictDeps = true;
  __structuredAttrs = true;

  projectFile = "Pomni.csproj";
  nugetDeps = ./deps.json;

  # Required for Native AOT
  nativeBuildInputs = [ stdenv.cc ];
  buildInputs = lib.optional stdenv.hostPlatform.isDarwin darwin.ICU;
  selfContainedBuild = true;

  dotnet-sdk = dotnetCorePackages.sdk_10_0;
  dotnet-runtime = null; # No runtime required for Native AOT

  executables = [ "pomni" ];

  meta = {
    description = "Nix dependency locking and updating";
    homepage = "https://github.com/drakon64/Pomni";
    license = lib.licenses.eupl12;
    mainProgram = "pomni";
    maintainers = with lib.maintainers; [ drakon64 ];
  };
})
