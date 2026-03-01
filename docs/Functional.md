[Home](index) · **Why this library** · [Optional](Optional) · [Result](Result) · [Error](Error) · [Validation](Validation) · [Utilities](Utilities) · [Async](Async) · [API Reference](API-Reference)

---

# Functional

A functional programming toolkit for Unity. It gives you explicit, composable representations of optionality and failure so you can stop writing defensive `if (x == null)` guards and `try/catch` blocks scattered across your codebase.

---

## The Problem

Unity projects routinely suffer from three categories of pain:

**1. Null-reference exceptions**
`GetComponent<T>()` returns null. `Resources.Load<T>()` returns null. Serialized fields that weren't wired up return null. The standard fix is defensive null checks at every call site — which means the checks are inconsistent and the errors are still runtime surprises.

**2. Exception-driven control flow**
`int.Parse()`, `JsonUtility.FromJson<T>()`, and many third-party APIs signal failure by throwing. Catching these in normal game logic mixes "control flow" with "error recovery" and makes pipelines impossible to read or test.

**3. Validation that accumulates no context**
When a config file has five bad fields you typically only learn about the first one. Without a way to collect failures, each fix-and-rerun cycle wastes time.

---

## The Approach

This library uses **railway-oriented programming**: every step in a data pipeline is either on the happy track (the value keeps moving forward) or on the error track (errors short-circuit or accumulate, depending on the strategy you choose). The key ideas are:

- **Explicit absence** — `Optional<T>` replaces `null` with a type that forces you to handle the missing case.
- **Explicit failure** — `Result<T>` replaces exceptions with a value that carries either a result or an `Error`.
- **Composable pipelines** — `Then`, `Map`, `Bind`, and `Match` let you chain steps without nested conditionals.
- **Async parity** — every sync operator has a `UniTask` counterpart so async pipelines look identical to sync ones.

---

## Core Types

### `Optional<T>`
Represents a value that **might exist** (`Some`) or **might be absent** (`None`). `Some()` treats a destroyed `UnityEngine.Object` as `None`, so Unity's fake-null problem is handled at the boundary. Use it wherever "not found" is a normal, non-exceptional outcome.

### `Result<T>`
Represents the **outcome of an operation** — either `Success(value)` or an `Error`. `Success()` treats a null reference-type value as an error automatically. Use it wherever a failure needs to be explained (config parsing, asset loading, service calls).

### `Error`
An immutable value carrying a human-readable `Message`, an optional inner `Error` for nesting, or a composite of multiple errors from validation. Use `Error.AsEnumerable()` to flatten composite errors for logging.

---

## Quick Install

Add the package to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.tutan.functional": "file:../Packages/com.tutan.functional"
  }
}
```

Add an assembly reference to `com.tutan.functional` in your `.asmdef`.

Then add this using at the top of any file (or once in a `GlobalUsings.cs`):

```csharp
using Tutan.Functional;
using static Tutan.Functional.F;   // brings Some, None, Success, Try, etc. into scope
```

> The package ships `global using static Tutan.Functional.F` automatically for assemblies that reference it, so in most cases these module-level helpers are already in scope.

---

## 📚 Documentation

| | Guide | What it covers |
|---|---|---|
| 📖 | [Why this library](Functional) | The problem, the approach, quick install |
| ❓ | [Optional\<T\>](Optional) | Construction, `Then`, `Or`, `Filter`, `Match`, Unity examples |
| ⚠️ | [Result\<T\>](Result) | Construction, `Then`, `Filter`, `Match`, pipeline patterns |
| 🔴 | [Error](Error) | Simple, nested, composite errors; logging; converting exceptions |
| ✅ | [Validation](Validation) | `Validator<T>`, `FailFast`, `HarvestErrors`, combining validators |
| 🔧 | [Utilities](Utilities) | `F` module, `IEnumerable` extensions, Unity lookup helpers |
| ⚡ | [Async](Async) | `ThenAsync`, `TryAsync`, mixing sync/async pipelines |
| 📋 | [API Reference](API-Reference) | Every public member — signature and one-line description |