[![](https://img.shields.io/nuget/v/soenneker.utils.datetime.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.utils.datetime/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.datetime/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.utils.datetime/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.utils.datetime.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.utils.datetime/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.datetime/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.utils.datetime/actions/workflows/codeql.yml)

# Soenneker.Utils.DateTime

Static helpers for constructing UTC timestamps and splitting ranges into timezone-aligned weeks or months.

## Installation

```bash
dotnet add package Soenneker.Utils.DateTime
```

## Construct timestamps

`CreateUtcDateTime()` builds a UTC `DateTime`. Any omitted component is taken from one snapshot of `DateTime.UtcNow`:

```csharp
DateTime start = DateTimeUtil.CreateUtcDateTime(
    year: 2026,
    month: 4,
    day: 15,
    hour: 9,
    minute: 30,
    second: 0);
```

`CreateTzDateTime()` interprets the components as wall-clock time in a timezone and returns the corresponding UTC instant:

```csharp
TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");

DateTime utc = DateTimeUtil.CreateTzDateTime(
    zone,
    year: 2026,
    month: 4,
    day: 15,
    hour: 9,
    minute: 30,
    second: 0);
```

Omitted components use the current wall-clock values in that timezone. Standard `TimeZoneInfo` rules apply: invalid local times during a daylight-saving transition throw, and ambiguous local times use the platform's normal conversion behavior.

## Calendar ranges

```csharp
List<(DateTime startAt, DateTime endAt)> weeks =
    DateTimeUtil.GetWeeklyDateTimesBetween(startAt, endAt, zone);

List<(DateTime startAt, DateTime endAt)> months =
    DateTimeUtil.GetMonthlyDateTimesBetween(startAt, endAt, zone);
```

Each method first expands `startAt` to the timezone-aligned start and end of its containing calendar period, then adds complete periods until one contains `endAt`. The returned ranges are calendar buckets rather than intersections with the input range, so the first start may precede `startAt` and the final end may follow `endAt`.

Both methods return at least the period containing `startAt`, including when `endAt` is earlier than that period's end. Validate range ordering before calling when an inverted range should be rejected.
