# CLAUDE.md

Guidance for Claude Code when working in this repository.

## `docs/` is a git submodule

`docs/` is a separate git repository (`sillsdev/saymore-doc`) checked out as a submodule. Do not create or edit files there. Notes, specs, and other Claude-authored working documents belong in `superpowers/` instead.

## Installer analytics consent

The Windows installer's analytics opt-in/opt-out UI (`PrivacyDlg`, registry read/write, dialog navigation) comes entirely from the `SIL.Installer` NuGet package (`src/Installer/Installer.wixproj`), not from custom code in this repo. `src/Installer/Installer.wxs` only references it via `<ComponentGroupRef Id="SilAnalyticsComponents" />`. See `superpowers/specs/2026-07-02-saymore-analytics-installer-design.md` for the design rationale.

## Keeping SIL package versions in sync

`SIL.libpalaso.l10ns` is referenced in `src/SayMore/SayMore.csproj` and again indirectly in `build/SayMore.proj` (which locates the restored package's `.xlf` content files). `build/SayMore.proj`'s `ResolvePalasoL10nsVersion` target reads the version straight from the csproj's `PackageReference` via `XmlPeek`, so it can't drift out of sync — don't reintroduce a hardcoded version there.

The other `SIL.*` packages (`SIL.Core`, `SIL.Archiving`, `SIL.Media`, `SIL.Windows.Forms*`, `SIL.WritingSystems`, `SIL.Installer`) are released together from the `libpalaso` monorepo and should be bumped together to the same version — mixing betas from that family risks assembly mismatches.
