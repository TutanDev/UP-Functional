# Optional in Unity

`Optional<T>` represents a value that might exist (`Some`) or might be missing (`None`).
In Unity projects this is especially useful for replacing `null` checks with explicit handling.

## Why `Optional<T>` is useful

- **Makes absence explicit**: APIs clearly communicate that a value can be missing.
- **Reduces null-reference bugs**: You must decide what to do in the `None` case.
- **Composes cleanly**: Chain logic with `Then`, `Filter`, and `Or` instead of nested `if` statements.
- **Unity-friendly**: `Some` treats destroyed `UnityEngine.Object` references as `None`, handling Unity's fake-null behavior.

## Unity-tailored examples

## 1) Safe component lookup

```csharp
using UnityEngine;
using Tutan.Functional;
using static Tutan.Functional.F;

public class HealthReader : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private void Start()
    {
        Optional<Health> maybeHealth = Some(target)
            .Then(go => go.GetComponent<Health>());

        int hp = maybeHealth
            .Then(h => h.Current)
            .Or(0);

        Debug.Log($"Current HP: {hp}");
    }
}

public class Health : MonoBehaviour
{
    public int Current = 100;
}
```

## 2) UI fallback when data is missing

```csharp
using TMPro;
using UnityEngine;
using Tutan.Functional;
using static Tutan.Functional.F;

public class PlayerNameView : MonoBehaviour
{
    [SerializeField] private TMP_Text label;

    public void Render(Optional<string> playerName)
    {
        label.text = playerName
            .Filter(name => name.Length > 0)
            .Then(name => $"Pilot: {name}")
            .Or("Pilot: Guest");
    }
}
```

## 3) Branching with `Match`

```csharp
using UnityEngine;
using Tutan.Functional;
using static Tutan.Functional.F;

public class SpawnPointResolver : MonoBehaviour
{
    [SerializeField] private Transform fallbackSpawn;

    public Vector3 ResolveSpawn(Optional<Transform> maybeSpawn)
    {
        return maybeSpawn.Match(
            onNone: () => fallbackSpawn.position,
            onSome: t => t.position);
    }
}
```

## 4) Save data read

```csharp
using Tutan.Functional;
using static Tutan.Functional.F;

public static class SaveReader
{
    public static Optional<int> TryReadLevel(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return None;

        return int.TryParse(raw, out var level)
            ? Some(level)
            : None;
    }
}

// Usage
// int levelToLoad = SaveReader.TryReadLevel(playerPrefsValue).Or(1);
```

## Practical guidance

- Use `Optional<T>` for values that are truly optional (component may be absent, save field may be unset, query may return nothing).
- Keep `Optional` at API boundaries where missing data is expected.
- Prefer `Match`, `Or`, and `Then` over `ValueUnsafe` unless you've already guaranteed `Some`.
