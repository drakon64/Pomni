{
  lib,
  buildDotnetModule,
  dotnetCorePackages,
  stdenv,
  darwin,
}:

buildDotnetModule (finalAttrs: {
  pname = "pomni";
  version = "1.0.0";

  src = ./src;

  strictDeps = true;
  __structuredAttrs = true;

  projectFile = "Pomni.csproj";
  nugetDeps = ./deps.json;

  nativeBuildInputs = [ stdenv.cc ];
  buildInputs = lib.optional stdenv.hostPlatform.isDarwin darwin.ICU;

  dotnet-sdk = dotnetCorePackages.sdk_10_0;
  dotnet-runtime = null;

  executables = [ "pomni" ];

  selfContainedBuild = true;

  meta = {
    description = "Nix dependency locking and updating";
    homepage = "https://github.com/drakon64/Pomni";
    license = lib.licenses.eupl12;
    mainProgram = "pomni";
    maintainers = with lib.maintainers; [ drakon64 ];
  };
})
