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

  projectFile = "Pomni.csproj";
  nugetDeps = ./deps.json;

  buildInputs = [ stdenv.cc ] ++ lib.optional stdenv.hostPlatform.isDarwin darwin.ICU;

  dotnet-sdk = dotnetCorePackages.sdk_10_0;
  dotnet-runtime = null;

  executables = [ "pomni" ];

  selfContainedBuild = true;

  meta = {
    license = lib.licenses.eupl12;
    mainProgram = "pomni";
    maintainers = with lib.maintainers; [ drakon64 ];
  };
})
