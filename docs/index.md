---
title: com.tutan.functional
---

# com.tutan.functional

A functional programming toolkit for Unity that gives you explicit, composable representations of optionality and failure — so you can stop writing defensive `null` checks and scattered `try/catch` blocks.

## What it solves

Unity projects routinely suffer from null-reference exceptions, exception-driven control flow, and validation that surfaces only one error at a time. This library replaces those patterns with **railway-oriented programming**: every step in a data pipeline is either on the happy track or the error track, handled explicitly at compile time.

Core types: `Optional<T>` for values that might be absent, `Result<T>` for operations that might fail, and `Error` for rich, composable failure payloads.

## Documentation

| Guide | Description |
|---|---|
| [Why this library](Functional) | The problem, the approach, quick install |
| [Optional\<T\>](Optional) | `Optional<T>` — construction, `Then`, `Or`, `Filter`, `Match`, Unity examples |
| [Result\<T\>](Result) | `Result<T>` — construction, `Then`, `Filter`, `Match`, pipeline patterns, Unity examples |
| [Error](Error) | `Error` — simple, nested, composite construction; logging; converting exceptions |
| [Validation](Validation) | `Validator<T>`, `FailFast`, `HarvestErrors`, combining validators, Unity examples |
| [Utilities](Utilities) | `F` module (`Try`, `Tee`, `Pipe`, `Curry`, `CurryFirst`), `IEnumerable` extensions, Unity lookup helpers |
| [Async](Async) | `ThenAsync`, `MatchAsync`, `TryAsync`, mixing sync/async pipelines, UniTask patterns |
| [API Reference](API-Reference) | Full scannable reference — every public member with signature and one-line description |
