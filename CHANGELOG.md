# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial game loop and mechanics.
- Player ship movement and shooting.
- Alien enemies and wave logic.
- Title screen with background music.
- Level 1 and Level 2 with distinct background music.
- Fullscreen toggle (`Ctrl + F`).
- Asset processing scripts (Python) for sprite transparency and enhancement.
- GitHub Actions workflow (planned).

### Changed
- Migrated to .NET 9.0.
- Updated asset pipeline to use `Content.mgcb` for audio.

### Fixed
- Visual artifacts on alien sprites using flood-fill and sharpening scripts.
- Audio loading issues by properly registering assets in the content pipeline.
