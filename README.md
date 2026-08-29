[![](https://img.shields.io/nuget/v/soenneker.utils.datetime.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.utils.datetime/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.datetime/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.utils.datetime/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.utils.datetime.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.utils.datetime/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.datetime/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.utils.datetime/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Utils.DateTime
A utility library for helpful DateTime related operations.

## Installation

```bash
dotnet add package Soenneker.Utils.DateTime
```

## Quick start

```csharp
using Soenneker.Utils.DateTime;
```

Call the static `DateTimeUtil` methods directly; no dependency-injection registration is required.

## Common operations

- `CreateUtcDateTime()` - Builds a new `System.DateTime` instance representing a UTC date and time, with optional year, month, day, hour, minute, and second parameters. If any of these parameters are not provided, the current UTC date and time values are used as defaults. The current UTC date and time is used to fill in any parameters not provided.
- `CreateTzDateTime()` - Builds a new `System.DateTime` instance representing a date and time adjusted to a specific time zone, with optional year, month, day, hour, minute, and second parameters. If any parameter is not provided, the current UTC date and time values are used as defaults, and then converted to the specified time zone.
