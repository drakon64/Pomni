{
  lib,
  buildDotnetModule,
  dotnetCorePackages,
  stdenv,
}:

buildDotnetModule {
  pname = "pomni";
  version = "0.0.1";

  src = ./src;

  projectFile = "Pomni.csproj";
  nugetDeps = ./deps.json;

  buildInputs = if stdenv.hostPlatform.isLinux then [ stdenv.cc ] else [ ];

  dotnet-sdk = dotnetCorePackages.sdk_10_0;
  dotnet-runtime = if stdenv.hostPlatform.isDarwin then dotnetCorePackages.runtime_10_0 else null;

  executables = [ "pomni" ];

  dotnetFlags = if stdenv.hostPlatform.isDarwin then [ "-p:PublishAot=false" ] else [ ];
  selfContainedBuild = stdenv.hostPlatform.isLinux;
}
