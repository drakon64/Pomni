{
  lib,
  buildDotnetModule,
  dotnetCorePackages,
  #stdenv,
}:

buildDotnetModule {
  pname = "pomni";
  version = "0.0.1";

  src = ./src;

  projectFile = "Pomni.csproj";
  nugetDeps = ./deps.json;

  #buildInputs = [ stdenv.cc ];

  dotnet-sdk = dotnetCorePackages.sdk_10_0;
  #dotnet-runtime = null;
  dotnet-runtime = dotnetCorePackages.runtime_10_0;

  executables = [ "pomni" ];

  dotnetFlags = [ "-p:PublishAot=false" ];
  #selfContainedBuild = true;
}
