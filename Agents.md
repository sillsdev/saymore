# Repository Guidelines

## Project Structure & Module Organization
SayMore is a WinForms client targeting .NET Framework 4.6.2. Application code lives in `src/SayMore`, organised by feature folders such as `Model`, `UI`, `Utilities`, and `Transcription`. NUnit fixtures live in `src/SayMoreTests` and mirror the production namespaces; use categories like `SkipOnTeamCity` for fragile tests. Packaging assets sit in `src/Installer` (WiX), while `build/SayMore.proj` drives CI builds and version stamping. Supporting material is stored under `DistFiles` (release notes), `SampleData` (demo content), `artwork` (branding), and `AutoSegmenter` for native helpers.

## Build, Test, and Development Commands
Run `nuget restore SayMore.sln` after cloning to hydrate build tasks and third-party assemblies. Use `msbuild SayMore.sln /p:Configuration=Debug /m` for daily work; binaries land in `output/Debug`. Execute `nunit3-console output/Debug/SayMoreTests.dll --where "cat != SkipOnTeamCity"` to match the TeamCity matrix. Ship-ready builds come from `msbuild build/SayMore.proj /t:Build /p:Configuration=Release`, and installer assets refresh through `build/getDependencies-windows.sh`.

## Coding Style & Naming Conventions
The root `.editorconfig` enforces tab indentation for `.cs` files; keep braces on new lines in the Visual Studio default style. Stick to PascalCase for types and public members, `_camelCase` for fields, and camelCase locals. Place shared UI controls under the closest `UI/*` folder, and extend existing helpers in `SayMore.Utilities` or `SIL.*` libs before adding new infrastructure. Update localization through `Resources.resx` so designer files stay generated.

## Testing Guidelines
Tests target NUnit 3; annotate fixtures with `[TestFixture]` and name them `<ClassName>Tests`. Express methods as `Method_State_Result` and co-locate helper builders in `SayMoreTests.Utilities`. Tag UI or intermittent tests with `[Category("SkipOnTeamCity")]` so CI filters them automatically. When adding sample files, drop them into `SayMoreTests/Resources` and embed via the generated designer.

## Commit & Pull Request Guidelines
Write imperative, single-sentence commit subjects and reference related JIRA keys (`SP-1234`) when known. Keep logical changes isolated per commit or PR, and avoid bundling installer edits with application logic. Pull requests should summarise motivation, list automated/manual test results, and attach screenshots or sample projects for UI-facing changes. Link the relevant TeamCity build and call out any categories you excluded locally. Request review from a maintainer of the affected module and note follow-up tasks in the PR body.
