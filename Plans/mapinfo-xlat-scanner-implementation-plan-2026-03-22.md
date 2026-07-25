# MapInfo And Xlat Scanner Implementation Plan

Updated: 2026-03-22

## Goal

Remove the remaining production constructions of `UnifiedLexer` from the MapInfo and Xlat paths while preserving the current token-stream parser model and shared `include` handling.

This plan assumes the current direction stays in place:

- UDMF and UWMF use direct parsers over `IDirectLexer`
- MapInfo and Xlat remain token-stream consumers
- `TokenSource` remains the shared include-expansion layer

## Current State

Status: implemented on 2026-03-22.

The shared include infrastructure has already been decoupled from the concrete lexer type.

- [src/Tiledriver/FormatModels/Common/Reading/TokenSource.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Common/Reading/TokenSource.cs)
  - now depends on `Func<TextReader, ITokenScanner>`
  - recursively expands `include` by rescanning nested files through that abstraction

The remaining production constructions of `UnifiedLexer` were limited to the format-specific scanner factories.

- [src/Tiledriver/FormatModels/MapInfo/Reading/MapInfoLexer.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/MapInfo/Reading/MapInfoLexer.cs)
  - now returns `new TokenScanner(reader, new TokenScannerOptions(ReportNewlines: true))`
- [src/Tiledriver/FormatModels/Xlat/Reading/XlatLexer.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Xlat/Reading/XlatLexer.cs)
  - now returns `new TokenScanner(reader, new TokenScannerOptions(AllowDollarIdentifiers: true, AllowPipes: true))`

The token-based readers and parsers already consume only token streams.

- [src/Tiledriver/FormatModels/MapInfo/Reading/MapDeclarationReader.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/MapInfo/Reading/MapDeclarationReader.cs)
  - creates `TokenSource(MapInfoLexer.Create(reader).Scan(), resourceProvider, MapInfoLexer.Create)`
- [src/Tiledriver/FormatModels/Xlat/Reading/XlatParser.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Xlat/Reading/XlatParser.cs)
  - creates `TokenSource(tokens, resourceProvider, XlatLexer.Create)`

This phase was completed as a scanner replacement behind `ITokenScanner`, without rewriting the token-based parsers.

## Constraints

### Preserve the current parsing model

MapInfo and Xlat parse from `IEnumerable<Token>` and `IEnumerator<Token>` today. There is no immediate value in forcing them onto the direct typed-read API used by UDMF and UWMF.

### Keep `include` token-based

`TokenSource` works because `include` is recognized in the token stream before the rest of the parser consumes it. Recreating that behavior inside direct parsers would complicate the design and duplicate logic.

### Keep lexer changes minimal and focused

The goal is to remove concrete `UnifiedLexer` dependencies from production code, not to redesign every parser in one step.

## Required Feature Parity

The replacement scanner must preserve the token behavior that current consumers rely on.

### Shared baseline behavior

- identifier, number, string, punctuation, and comment handling compatible with current token parsers
- stable token locations for error reporting
- `Scan()` output compatible with existing `TokenSource`, `MapDeclarationParser`, and `XlatParser`

### MapInfo-specific behavior

- newline reporting, because MapInfo currently constructs the lexer with `reportNewlines: true`

### Xlat-specific behavior

- dollar-prefixed identifiers
- pipe tokens

### Include compatibility

- scanner factory must remain usable as `Func<TextReader, ITokenScanner>`
- nested includes must scan with the same format-specific options as the root file

## Recommended Design

### 1. Introduce a new configurable token scanner

Add a new scanner implementation in the common reading layer that implements `ITokenScanner` and is configured by options rather than by constructing `UnifiedLexer` directly.

Recommended shape:

- `TokenScannerOptions` record or sealed options type
- `TokenScanner : ITokenScanner`

Minimum options needed now:

- `ReportNewlines`
- `AllowDollarIdentifiers`
- `AllowPipes`

This keeps the replacement narrow and directly aligned with the two remaining callers.

### 2. Keep `ITokenScanner` unchanged for now

The current `ITokenScanner` interface is sufficient.

It should remain:

- a single-purpose token enumeration abstraction
- separate from `IDirectLexer`, which is still optimized for direct parsing hot paths

There is no need to merge the two abstractions yet.

### 3. Move format-specific configuration into the factories

`MapInfoLexer` and `XlatLexer` should stay as small format-specific factory helpers.

After the new scanner exists, the factories should become thin wrappers over it.

Expected end state:

- `MapInfoLexer.Create(reader)` returns `new TokenScanner(reader, new TokenScannerOptions { ReportNewlines = true })`
- `XlatLexer.Create(reader)` returns `new TokenScanner(reader, new TokenScannerOptions { AllowDollarIdentifiers = true, AllowPipes = true })`

This preserves the current call sites and avoids spreading format flags into readers and parsers.

### 4. Leave `TokenSource` structurally unchanged

`TokenSource` has already reached the right abstraction boundary for this phase.

No behavior change is needed unless the new scanner exposes a bug that requires:

- more explicit include validation
- cycle detection
- better include path diagnostics

Those would be follow-up improvements, not prerequisites for the scanner migration.

## Implemented Work

### 1. Added the new scanner implementation

Completed by adding:

- [src/Tiledriver/FormatModels/Common/Reading/TokenScanner.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Common/Reading/TokenScanner.cs)
- [src/Tiledriver/FormatModels/Common/Reading/TokenScannerOptions.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Common/Reading/TokenScannerOptions.cs)

The new scanner implements `ITokenScanner` and preserves the option set needed by MapInfo and Xlat.

### 2. Added direct scanner tests

Completed by adding [src/Tiledriver.Tests/FormatModels/Common/Reading/TokenScannerTests.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver.Tests/FormatModels/Common/Reading/TokenScannerTests.cs).

Covered behaviors:

- number, string, boolean, newline, dollar-identifier, and pipe scanning
- identifier and string interning behavior

### 3. Switched `MapInfoLexer` to the new scanner

Completed in [src/Tiledriver/FormatModels/MapInfo/Reading/MapInfoLexer.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/MapInfo/Reading/MapInfoLexer.cs).

### 4. Switched `XlatLexer` to the new scanner

Completed in [src/Tiledriver/FormatModels/Xlat/Reading/XlatLexer.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Xlat/Reading/XlatLexer.cs).

### 5. Revalidated `TokenSource`

Completed by updating [src/Tiledriver.Tests/FormatModels/Common/Reading/TokenSourceTests.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver.Tests/FormatModels/Common/Reading/TokenSourceTests.cs) to use `TokenScanner` instead of `UnifiedLexer`.

### 6. Removed the legacy lexer implementation

Because no production code or non-legacy tests still depended on it, the following were removed:

- [src/Tiledriver/FormatModels/Common/Reading/UnifiedLexer.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Common/Reading/UnifiedLexer.cs)
- [src/Tiledriver/FormatModels/Common/Reading/IUnifiedLexer.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Common/Reading/IUnifiedLexer.cs)
- [src/Tiledriver/FormatModels/Common/Reading/IUnifiedValueLexer.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Common/Reading/IUnifiedValueLexer.cs)
- [src/Tiledriver/FormatModels/Common/Reading/UnifiedTokenValue.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Common/Reading/UnifiedTokenValue.cs)
- [src/Tiledriver.Tests/FormatModels/Common/Reading/UnifiedLexerTests.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver.Tests/FormatModels/Common/Reading/UnifiedLexerTests.cs)

The `UnifiedTokenValue`-specific overloads were also removed from [src/Tiledriver/FormatModels/Common/ParsingException.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Common/ParsingException.cs).

## Validation

Focused validation completed successfully:

- `dotnet test Tiledriver.Tests/Tiledriver.Tests.csproj -c Release --filter "TokenScannerTests|TokenSourceTests|MapInfoLexerTests|XlatLexerTests|XlatParserTests"`
  - 23 passed, 0 failed
- `dotnet test Tiledriver.Tests/Tiledriver.Tests.csproj -c Release --filter "MapInfo|Xlat|TokenSource|UwmfParserTests|UdmfParserTests"`
  - 23 passed, 0 failed

## Follow-Up

The remaining related migration work is outside the MapInfo/Xlat scanner phase.

Likely next topics are:

- broader cleanup of older migration notes so they match the now-current state
- deciding whether the parser-facing abstraction names should be generalized later

## Risks

### Biggest risk

The new scanner may look equivalent on happy-path files but differ subtly in token boundaries or locations, which would surface as parser regressions or degraded error reporting.

### Risk control

- add direct scanner tests before switching format factories
- keep parser changes out of scope unless a test proves they are necessary
- switch one format at a time so regressions are easier to isolate

## Out Of Scope

This plan does not include:

- rewriting MapInfo or Xlat to use `IDirectLexer`
- merging `ITokenScanner` and `IDirectLexer`
- redesigning `TokenSource`

Those can be addressed later.

## Success Criteria

This work is complete when:

1. [src/Tiledriver/FormatModels/MapInfo/Reading/MapInfoLexer.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/MapInfo/Reading/MapInfoLexer.cs) no longer constructs `UnifiedLexer`
2. [src/Tiledriver/FormatModels/Xlat/Reading/XlatLexer.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Xlat/Reading/XlatLexer.cs) no longer constructs `UnifiedLexer`
3. `TokenSource` include expansion still works through `Func<TextReader, ITokenScanner>`
4. focused MapInfo, Xlat, and `TokenSource` tests pass unchanged or with only scanner-specific test additions
5. the legacy `UnifiedLexer` implementation and its helper types are removable without breaking production parsing paths