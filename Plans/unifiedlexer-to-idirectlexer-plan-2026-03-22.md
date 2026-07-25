# UnifiedLexer To IDirectLexer Migration Summary

Updated: 2026-03-22

## Status

Completed on 2026-03-22.

The migration away from `UnifiedLexer` is complete, and the direct parser abstraction has now been renamed from `IUdmfLexer` to `IDirectLexer`.

## Final State

- UDMF parsing uses `UdmfParser(IDirectLexer)` with `DirectLexer`
- UWMF parsing uses `UwmfParser(IDirectLexer)` with handwritten `PlaneMaps` handling and generated normal-block parsing
- MapInfo and Xlat remain token-stream based and now use `TokenScanner` through `ITokenScanner`
- `TokenSource` expands includes through `Func<TextReader, ITokenScanner>`
- `UnifiedLexer` and its related helper types were removed

## What Was Completed

### 1. UDMF benchmark harness migrated

- [src/ConsoleApps/Benchmarks/UdmfParsingBenchmark.cs](/Users/davidaramant/Documents/src/tiledriver/src/ConsoleApps/Benchmarks/UdmfParsingBenchmark.cs) now benchmarks the direct parser path via `DirectLexer`
- legacy `UnifiedLexer` lex-only benchmark cases were removed

### 2. UWMF production path migrated

- [src/Tiledriver/FormatModels/Uwmf/Reading/UwmfReader.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Uwmf/Reading/UwmfReader.cs) now uses `new UwmfParser(new DirectLexer(textReader)).Parse()`
- the old AST-plus-semantic-analyzer pipeline was removed

### 3. Shared token-stream infrastructure generalized

- [src/Tiledriver/FormatModels/Common/Reading/TokenSource.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Common/Reading/TokenSource.cs) depends on `Func<TextReader, ITokenScanner>` instead of a concrete lexer type

### 4. MapInfo and Xlat scanner path migrated

- [src/Tiledriver/FormatModels/MapInfo/Reading/MapInfoLexer.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/MapInfo/Reading/MapInfoLexer.cs) now uses `TokenScanner`
- [src/Tiledriver/FormatModels/Xlat/Reading/XlatLexer.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Xlat/Reading/XlatLexer.cs) now uses `TokenScanner`
- the implementation details are captured in [Plans/mapinfo-xlat-scanner-implementation-plan-2026-03-22.md](/Users/davidaramant/Documents/src/tiledriver/Plans/mapinfo-xlat-scanner-implementation-plan-2026-03-22.md)

### 5. Legacy lexer stack removed

Removed files included:

- `UnifiedLexer.cs`
- `IUnifiedLexer.cs`
- `IUnifiedValueLexer.cs`
- `UnifiedTokenValue.cs`
- legacy `UnifiedLexer` tests

### 6. Direct parser interface renamed

- `IUdmfLexer` was renamed to `IDirectLexer`
- the interface file is now [src/Tiledriver/FormatModels/Udmf/Reading/IDirectLexer.cs](/Users/davidaramant/Documents/src/tiledriver/src/Tiledriver/FormatModels/Udmf/Reading/IDirectLexer.cs)

## Validation Snapshot

The migration completed with focused parser and scanner coverage passing, including the post-cleanup run:

```bash
dotnet test Tiledriver.Tests/Tiledriver.Tests.csproj -c Release --filter "TokenScannerTests|MapInfo|Xlat|TokenSource|UwmfParserTests|UdmfParserTests"
```

Result:

- 37 passed
- 0 failed

## Remaining Follow-Up

The `UnifiedLexer` migration itself is done.

Possible future cleanup work:

- continue updating historical notes where the old `IUdmfLexer` name appears for chronology only