# MAUI Demo (Teaching Repository)

This repository is an example .NET MAUI application intended to show a clean project structure:

- `Models` for domain/data shapes
- `Services` for infrastructure and data access abstractions (your Data Access Layer classes should go here)
- `ViewModels` for UI state and commands
- `Views` for XAML pages
- `Navigation` for route constants and navigation parameters

## Important Note About Data Access

This project does **not** include a real data access layer.

The current implementation in `LundUniversity.MauiExample/Services/InMemoryUniversityRepository.cs` is an in-memory sample used only for demonstration.

For your project, you should provide your own data access implementation under `LundUniversity.MauiExample/Services` (or the corresponding directory in your own project) and wire it up to `LundUniversity.MauiExample/MauiProgram.cs`.
