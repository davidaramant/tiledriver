# UDMF Parsing Findings

Updated: 2026-03-22

## Scope

This note captures the current state of the UDMF parsing performance work, the changes tried so far, the changes that were kept, the changes that were reverted, and the next steps that look worth testing.

The current benchmark input is the largest `.txt` UDMF file under `src/ConsoleApps/Benchmarks/Udmf`, which is presently `zdcmp2.txt` at roughly 24 MB.

Earlier benchmark numbers for this work were captured on different machines and under earlier code shapes. Because of that, this note treats the latest fresh rerun on this macOS machine as the current baseline and keeps the older sections only as historical context. Absolute times should only be compared within the same machine/run setup.

## Current Baseline

Latest full local BenchmarkDotNet run from `src/ConsoleApps/Benchmarks/BenchmarkDotNet.Artifacts/20260322-102925/Benchmarks.UdmfParsingBenchmark-report-github.md` on this machine:

- macOS Tahoe 26.3.1
- Apple M4 Max
- .NET SDK 10.0.201
- .NET runtime 10.0.5
- Current `IDirectLexer` / `DirectLexer` code shape with direct typed reads and generated custom `HasFlag` helpers

| Benchmark | Mean | Notes |
| --- | ---: | --- |
| ParseUdmf | 102.560 ms | End-to-end parse from stream via `DirectLexer` |
| ParseUdmfFromPreLexedTokens | 87.768 ms | Parser-only cost via old `IUnifiedValueLexer` adapter |
| ParseUdmfFromPreLexedTokensDirect | 76.324 ms | Parser-only cost via new `IDirectLexer` adapter |
| ParseLineDefsFromPreLexedTokens | 23.951 ms | Linedef parser-only via old adapter |
| ParseLineDefsFromPreLexedTokensDirect | 17.315 ms | Linedef parser-only via new adapter |
| ParseThingsFromPreLexedTokens | 5.813 ms | Thing parser-only via old adapter |
| ParseThingsFromPreLexedTokensDirect | 3.516 ms | Thing parser-only via new adapter |
| LexUdmfFromStream | 418.128 ms | Legacy object-token lexing baseline |
| LexUdmfFromString | 403.184 ms | Legacy object-token lexing from string |
| LexUdmfCountTokens | 99.526 ms | Token counting without materializing a list |

Current allocation picture:

- End-to-end parse (`ParseUdmf`) remains around 42.79 MB allocated.
- Parser-only over pre-lexed tokens remains around 42.63 MB for both old and new adapter paths.
- The new `IDirectLexer` path is allocation-neutral versus the old `IUnifiedValueLexer` path — the improvement is purely CPU.

Parser-only A/B comparison (same run, same machine):

| Benchmark | Old adapter | New adapter | Speedup |
| --- | ---: | ---: | ---: |
| Full parse (pre-lexed) | 87.768 ms | 76.324 ms | 1.15x |
| Linedef block (pre-lexed) | 23.951 ms | 17.315 ms | 1.38x |
| Thing block (pre-lexed) | 5.813 ms | 3.516 ms | 1.65x |

Useful ratios from the current local run:

- `ParseUdmfFromPreLexedTokensDirect` is about 74% of end-to-end parse time.
- `ParseLineDefsFromPreLexedTokensDirect` is about 4.92x heavier than `ParseThingsFromPreLexedTokensDirect`.
- `LexUdmfFromStream` is about 4.08x slower than full `ParseUdmf`.
- `LexUdmfFromStream` and `LexUdmfFromString` are within about 3.7% of each other on this machine.

BenchmarkDotNet warnings from this run:

- `ParseUdmf` still dips below the 100 ms minimum iteration-time guidance because of low-end outliers.
- `ParseThingsFromPreLexedTokensDirect` is still just under the 100 ms minimum iteration-time guidance and could use a slightly higher `OperationsPerInvoke` if that warning becomes distracting.

## Earlier External Baseline

Older BenchmarkDotNet rerun from `src/ConsoleApps/Benchmarks/BenchmarkDotNet.Artifacts/20260320-rerun-full-6/results/Benchmarks.UdmfParsingBenchmark-report-github.md`, preserved here as historical context from the document's earlier edits:

- Different machine/runtime context from the current macOS baseline
- Older parser code shape with `IUnifiedValueLexer` and generated custom `HasFlag` helper

| Benchmark | Mean | Notes |
| --- | ---: | --- |
| ParseUdmf | 806.80 ms | End-to-end parse from stream |
| ParseUdmfFromPreLexedTokens | 269.68 ms | Parser-only cost over value tokens |
| ParseLineDefsFromPreLexedTokens | 64.10 ms | Main parser hotspot remains `linedef` |
| ParseThingsFromPreLexedTokens | 19.14 ms | Secondary block microbenchmark |
| LexUdmfFromStream | 1,207.04 ms | Legacy object-token lexing baseline |
| LexUdmfFromString | 1,199.64 ms | Legacy object-token lexing from string |
| LexUdmfCountTokens | 327.07 ms | Token counting without materializing a list |

Historical before/after comparison from that earlier context:

| Benchmark | Before | After | Speedup |
| --- | ---: | ---: | ---: |
| ParseUdmf | 806.80 ms | 183.01 ms | 4.41x |
| ParseLineDefsFromPreLexedTokens | 64.10 ms | 36.31 ms | 1.77x |
| ParseThingsFromPreLexedTokens | 19.14 ms | 11.19 ms | 1.71x |
| LexUdmfFromStream | 1,207.04 ms | 639.14 ms | 1.89x |
| LexUdmfCountTokens | 327.07 ms | 183.60 ms | 1.78x |

The dramatic 4.41x improvement in `ParseUdmf` reflects both the new `DirectLexer` (which bypasses `UnifiedTokenValue` entirely for lexing) and the new `IDirectLexer`-based parser. The lexer-only benchmarks also improved significantly, which suggests the previous run may have been affected by background load or thermal conditions. These cross-machine, cross-run comparisons are preserved only as historical context and should not be compared directly against the current macOS baseline above.

## Historical Prior-Machine Baseline

Older full BenchmarkDotNet run from `src/ConsoleApps/Benchmarks/benchmark.log` on a different machine using the `uint`-backed per-block flags enums and the standard enum `HasFlag` calls:

| Benchmark | Mean | Notes |
| --- | ---: | --- |
| ParseUdmf | 137.836 ms | End-to-end parse from stream |
| ParseUdmfFromPreLexedTokens | 80.138 ms | Parser-only cost over value tokens |
| ParseLineDefsFromPreLexedTokens | 19.229 ms | Main parser hotspot remained `linedef` |
| ParseThingsFromPreLexedTokens | 3.994 ms | Secondary block microbenchmark |
| LexUdmfFromStream | 420.363 ms | Legacy object-token lexing baseline |
| LexUdmfFromString | 406.074 ms | Legacy object-token lexing from string |
| LexUdmfCountTokens | 99.929 ms | Token counting without materializing a list |

Those historical results are still useful for one conclusion: on that machine, the standard enum `HasFlag` path regressed parser throughput versus the same readable named-flags shape with manual bitwise checks. They should not be read as a before/after comparison against the current local rerun above.

## Current Interpretation

The current `IDirectLexer` / `DirectLexer` code shape continues to deliver a clear parser-side improvement:

- In the fresh same-run A/B comparison (pre-lexed token adapters), the new `IDirectLexer` path is 1.15x–1.65x faster than the old `IUnifiedValueLexer` path, with no change in allocations.
- The biggest relative win in the current run is the `thing` microbenchmark; the most important absolute parser-side win is still `linedef`.

The dominant cost is still lexing when measured through the legacy object-token API. The `LexUdmfFromStream` benchmark at 418 ms is 4.08x slower than end-to-end `ParseUdmf` at 103 ms, which now uses `DirectLexer` instead of the legacy lexing path.

On the parser side, the remaining hot path is not evenly distributed across all block types.

The `linedef` block reader remains the clearest parser hotspot:

- `ParseLineDefsFromPreLexedTokensDirect` is about 4.92x heavier than `ParseThingsFromPreLexedTokensDirect` on the current machine.
- That makes `linedef` the best acceptance benchmark for parser-side CPU work.
- The `thing` microbenchmark is still useful as a guardrail.

The parser-side win from `IDirectLexer` comes from eliminating `UnifiedTokenValue` struct creation/dispatch on the parser hot path. The parser now tells the lexer (or adapter) what type to produce, which avoids the kind/value union overhead and simplifies the generated code.

The allocation neutrality of the adapter-based A/B confirms that the remaining allocations are dominated by the output object graph (`MapData`, `LineDef`, `Thing`, etc.), not by the token interface.

## Changes Tried So Far

### Kept changes

1. Added parser-only benchmarking.

- Added `ParseUdmfFromPreLexedTokens` so parser cost can be measured separately from lexing.
- This made it possible to stop guessing where time was going.

2. Cleaned up BenchmarkDotNet configuration.

- Added `OperationsPerInvoke` to the short benchmarks.
- This reduced noisy `MinIterationTime` warnings and made the block microbenchmarks more usable.

3. Switched identifier comparison to ordinal ignore-case.

- `Identifier` now uses `StringComparer.OrdinalIgnoreCase`.
- This was a cheap correctness-preserving improvement and stayed.

4. Added string literal caching in `UnifiedLexer`.

- Repeated quoted strings in the benchmark map justified caching.
- This produced a modest speed and memory improvement.
- It helped, but it was not the primary allocation source.

5. Introduced the low-allocation parser-facing token model.

- Added `IUnifiedValueLexer`.
- Added `UnifiedTokenValue` and `UnifiedTokenKind`.
- Reworked `UnifiedLexer` so the core parser path can consume value tokens instead of allocating token objects.
- Reworked `UdmfParser` to consume `IUnifiedValueLexer`, with fallback from `IUnifiedLexer` for compatibility.

This was the major architectural win. It cut a large amount of token-object churn, but initially caused a noticeable parser CPU regression that needed follow-up work.

6. Generated typed field readers instead of routing through more generic conversion logic.

- `ParseIntFieldValue`
- `ParseBooleanFieldValue`
- `ParseStringFieldValue`
- `ParseDoubleFieldValue`
- `ParseTextureFieldValue`

This recovered some CPU cost after the value-token refactor and stayed.

7. Removed nullable token flow from the hot parser loop.

- Replaced nullable-style token handling with explicit `TryGetNext(out UnifiedTokenValue token)`.
- Added explicit EOF handling.

This was one of the better parser-side CPU recoveries after the low-allocation token refactor.

8. Split the legacy object-token lexer path from the value-token core.

- The legacy `Token` path no longer round-trips through `UnifiedTokenValue.ToToken()` on the hot path.
- This substantially improved lex-only benchmarks and slightly improved end-to-end parse time.

9. Added block-only microbenchmarks.

- Added `ParseLineDefsFromPreLexedTokens`.
- Added `ParseThingsFromPreLexedTokens`.

These are now essential. They make it much easier to reject parser changes that help one path but hurt the true hotspot.

10. Replaced per-field duplicate tracking booleans with a bitmask-backed `seenFields` tracking mechanism.

- This reduced local-state overhead in generated block readers.
- The win was modest but real, especially for `linedef`.

11. Improved generated `seenFields` readability with per-block named flags enums.

- The generator now emits a private `[Flags]` enum per block instead of raw anonymous shift constants.
- The emitted parser now uses human-readable names such as `ThingFields.X` and `LineDefFields.SideFront`.
- This keeps the same underlying bitmask model while making the generated code far easier to inspect.

12. Reduced the generated flags enum backing type from `ulong` to `uint`.

- All current normal blocks have fewer than 32 scalar fields, so `uint` is enough for today.
- The generator now guards against blocks with more than 32 scalar properties.
- This is mainly a simplification/readability choice, not a demonstrated performance win.

13. Switched generated field dispatch from a flat chain to length-bucketed dispatch.

- The generator now emits `switch (identifierText.Length)` and then compares only within that bucket.
- This provided a small but measurable improvement and is still the best current generated dispatch shape.

14. Added a generated custom `HasFlag` helper per seen-fields enum.

- The generator now emits a per-enum `HasFlag` helper instead of calling the standard enum `HasFlag` implementation directly.
- The helper is marked with `MethodImplOptions.AggressiveInlining`.
- This code shape has now been remeasured locally and the current-machine numbers are captured in the baseline above.

15. Introduced `IDirectLexer` interface and `DirectLexer` to eliminate `UnifiedTokenValue` from the parser hot path.

- Added `IDirectLexer` with direct typed-read methods: `ReadIdentifier()`, `ReadInteger()`, `ReadDouble()`, `ReadBoolean()`, `ReadString()`, `ExpectEquals()`, `ExpectSemicolon()`, `ExpectOpenBrace()`, `ExpectCloseBrace()`, `TryReadIdentifier(out Identifier)`, `TryExpectEquals()`, `TryExpectOpenBrace()`, `TryExpectCloseBrace()`, `SkipValueAndSemicolon()`.
- Added `DirectLexer` that wraps a `TextReader` and returns typed values directly without round-tripping through `UnifiedTokenValue`. Reuses the same character-level logic as `UnifiedLexer` (number parsing, string caching, identifier caching, comment skipping).
- Added `ValueLexerBackedUdmfLexer` adapter so the old `IUnifiedLexer` constructor on `UdmfParser` still works.
- Rewrote `UdmfParser` to consume `IDirectLexer`. The current end state keeps only the direct constructor.
- Updated `UdmfParserGenerator` and regenerated `UdmfParser.Generated.cs` to use `IDirectLexer` methods directly.
- Updated `UdmfReader` to use `new DirectLexer(textReader)` instead of `new UnifiedLexer(textReader)`.
- Added `DirectLexerTokenArrayAdapter : IDirectLexer` in the benchmark harness for parser-only A/B comparison.

Same-run parser-only A/B results: 1.25x faster for full parse, 1.35x for `linedef`, 1.49x for `thing`. Allocations unchanged.

### Tried and reverted

1. Lookup-table or dictionary-style field dispatch in generated block readers.

- Hypothesis: replace branch chains with a lookup structure.
- Result: regressed parser throughput.
- Outcome: reverted.

2. Fully inlined typed assignment parsing inside each generated field branch.

- Hypothesis: remove helper call overhead in hot generated branches.
- Result: clearly regressed the hotspot benchmark, especially `ParseLineDefsFromPreLexedTokens`.
- Outcome: reverted.

3. Replaced manual bitwise required/duplicate checks with standard `HasFlag` calls on the generated flags enums.

- Hypothesis: keep the named enum readability improvement and make the generated checks even easier to read.
- Result: measurable parser regression versus the same enum-based code using manual bitwise checks.
- Outcome: not a good tradeoff if parser throughput remains the priority.

At the time of writing, the historical regression evidence for the standard `HasFlag` form still stands, but it remains prior-machine evidence rather than the current local baseline.

The regression was large enough that the standard `HasFlag` approach should be considered a dead end unless there is a materially different inlining shape to test.

## Current Code Shape

The best current parser shape is:

- Direct typed-read parser input via `IDirectLexer` (new)
- `DirectLexer` for end-to-end parsing from `TextReader` (new)
- Backward compatibility preserved for `IUnifiedLexer` via adapter chain
- Generated block readers call `_lexer.ReadInteger()`, `_lexer.ReadString()`, etc. directly
- Generated `seenFields` bitmask duplicate tracking
- Generated length-bucketed field dispatch
- Generated per-enum custom `HasFlag` helper marked for aggressive inlining
- `SkipValueAndSemicolon()` for unknown fields

This code shape is checked in and has a clean local BenchmarkDotNet run recorded in this note.

## Main Findings

1. The original biggest problem was token object churn, not purely parser logic.

The value-token parser path addressed that root cause and preserved compatibility for existing callers.

2. After the allocation win, parser-side improvements became code-shape sensitive.

Small structural changes now matter more than large conceptual changes. Several changes that looked attractive on paper were worse in practice.

The latest measured example is the standard `HasFlag` path: it improved readability, but the benchmark run showed that it costs enough parser time to matter on the current workload.

3. `linedef` is the right optimization target for the next round.

If a parser change does not improve `ParseLineDefsFromPreLexedTokens`, it is probably not addressing the most important remaining parser-side cost.

4. Dispatch is improved, but not solved enough to explain the remaining gap by itself.

The kept dispatch changes helped, but the remaining `linedef` cost suggests more work is still happening per handled field than just name matching.

5. The `IDirectLexer` refactor delivered a clear parser-side CPU improvement.

The same-run A/B comparison (pre-lexed token adapters) shows 1.25x–1.49x speedup with zero allocation change. This confirms that eliminating `UnifiedTokenValue` struct creation/dispatch from the parser hot path was a real win, not just a measurement artifact.

## Recommended Next Steps

### 1. Inspect the generated `linedef` branch shape in detail

Focus areas:

- The exact number of same-length identifier comparisons in `ParseLineDefBlock`
- How often the parser falls through same-length groups before matching
- Whether required-field checks or per-field writes are generating avoidable extra work

Reason:

The current dispatch is better than the flat chain, but `linedef` still has enough fields that branch density may still matter.

### 2. Consider frequency-aware ordering within each length bucket

Concrete experiment:

- For `linedef`, reorder comparisons inside a length bucket based on real field frequency from the benchmark map rather than declaration order.

Reason:

The current length-bucketed dispatch reduces the comparison set. The next cheap thing to test is whether the most common fields can be matched earlier within those buckets.

Risk:

- This may only help the current dataset.
- It should be evaluated against both the full parse and the block microbenchmarks before keeping it.

### 3. Inspect unknown-field handling in the generated block loop

Concrete experiment:

- Verify how often `unknownFields` is actually relevant in the benchmark dataset.
- If unknown fields are effectively absent in the benchmark map, consider a cheaper no-unknown fast path shape that does not bias the common case.

Reason:

The current logic is already lazy, but the control flow still pays for the possibility of unknown fields on every identifier branch.

### 4. Keep using the microbenchmarks as acceptance gates

Suggested acceptance order:

1. `ParseLineDefsFromPreLexedTokensDirect`
2. `ParseUdmfFromPreLexedTokensDirect`
3. `ParseUdmf`
4. Full suite sanity check

Reason:

This avoids spending time on changes that look good in end-to-end numbers only because of noise or lexing variance. The `*Direct` variants are now the primary acceptance benchmarks since they exercise the current production code path.

### 5. Clean up old benchmark methods once the new path is validated

The old `ParseUdmfFromPreLexedTokens`, `ParseLineDefsFromPreLexedTokens`, and `ParseThingsFromPreLexedTokens` methods (using the `ValueTokenArrayLexer` / `IUnifiedValueLexer` adapter) served their purpose in the A/B comparison. They can be removed once the new `*Direct` path is accepted as the permanent production path.

### 6. Consider whether `IUnifiedValueLexer` and the old `UdmfParser(IUnifiedLexer)` constructor can be removed

The old `IUnifiedValueLexer` interface, `UnifiedTokenValue`, and the backward-compatible `UdmfParser(IUnifiedLexer)` constructor are no longer used in the production parse path. If no other callers remain, they can be cleaned up to reduce the interface surface.

## Changes That Do Not Look Promising Right Now

- Reintroducing dictionary-based dispatch for fields
- Reintroducing fully inlined typed assignment parsing per generated property branch
- Keeping the standard `HasFlag` call in the hottest generated parser checks if parser throughput is the main constraint
- Chasing lexer string allocation as the primary remaining parser bottleneck
- Trying to reduce allocations further through the adapter path — the same-run A/B shows the `IDirectLexer` adapter is allocation-identical to the old `IUnifiedValueLexer` adapter, confirming the remaining allocations are output objects

Those areas have already been tested enough to treat them as low-priority for now.

## Validation Notes

All 196 unit tests pass after the `IDirectLexer` / `DirectLexer` refactor. The full solution builds with 0 warnings, 0 errors.

Current benchmark artifacts:

- `src/ConsoleApps/Benchmarks/BenchmarkDotNet.Artifacts/20260320-190853/Benchmarks.UdmfParsingBenchmark-report-github.md`
- `src/ConsoleApps/Benchmarks/BenchmarkDotNet.Artifacts/20260320-190853/Benchmarks.UdmfParsingBenchmark-report.csv`
- `src/ConsoleApps/Benchmarks/BenchmarkDotNet.Artifacts/20260320-190853/Benchmarks.UdmfParsingBenchmark-report.html`

Previous local baseline artifacts:

- `src/ConsoleApps/Benchmarks/BenchmarkDotNet.Artifacts/20260320-rerun-full-6/results/Benchmarks.UdmfParsingBenchmark-report-github.md`

Relevant files for follow-up work:

- `src/ConsoleApps/Benchmarks/UdmfParsingBenchmark.cs`
- `src/ConsoleApps/DataModelGenerator/Udmf/UdmfParserGenerator.cs`
- `src/Tiledriver/FormatModels/Udmf/Reading/IDirectLexer.cs`
- `src/Tiledriver/FormatModels/Udmf/Reading/DirectLexer.cs`
- `src/Tiledriver/FormatModels/Udmf/Reading/ValueLexerBackedUdmfLexer.cs`
- `src/Tiledriver/FormatModels/Udmf/Reading/UdmfParser.cs`
- `src/Tiledriver/FormatModels/Udmf/Reading/UdmfParser.Generated.cs`
- `src/Tiledriver/FormatModels/Common/Reading/IUnifiedValueLexer.cs`
- `src/Tiledriver/FormatModels/Common/Reading/UnifiedTokenValue.cs`