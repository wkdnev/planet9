# Planet9 Game Development Plan

## Project Overview
**Title:** Planet9  
**Genre:** Modern Space Invaders / Shoot 'em up  
**Framework:** C# / MonoGame  
**Platform:** Desktop (GL)

## Core Features
- [x] **Game Loop & State Management**: Title Screen, Gameplay, Shop, GameOver.
- [x] **Levels (Planets)**: 9 distinct levels with increasing difficulty.
- [x] **Player Controller**: Movement (Arrow keys), Firing (A, S, D).
- [x] **Enemy System**: Various enemy types, formations, and behaviors.
- [x] **Economy System**: Money drops, persistent currency.
- [x] **Shop System**: Buy upgrades (Weapons, Shields, Lives) between levels.
- [x] **Scoring & Leaderboard**: Persistent score, Top 50 local leaderboard.
- [ ] **Audio**: Sound effects and background music.

## Task List

### Phase 1: Setup & Infrastructure
- [x] **Project Initialization**: Create MonoGame project structure.
- [x] **Asset Management**: Set up Content Pipeline (placeholder graphics/audio).
- [x] **Game State Manager**: Implement a system to switch between screens (Menu, Game, Shop).
- [x] **Input Manager**: Abstract keyboard input for movement and actions.

### Phase 2: Core Gameplay (The "Toy")
- [x] **Player Ship**: Render ship, implement movement physics.
- [x] **Basic Shooting**: Implement projectile system (pooling).
- [x] **Basic Enemy**: Create a simple enemy that moves and can be destroyed.
- [x] **Collision Detection**: Player-Enemy, Bullet-Enemy, EnemyBullet-Player.
- [x] **Advanced Enemy AI**: Swarm behavior (move in top half), Enemy Firing.

### Phase 3: Level Design & Progression
- [x] **Level Manager**: System to load level configurations (Planet 1-9).
- [x] **Wave System**: Define enemy waves for each planet.
- [x] **Difficulty Scaling**: Increase enemy speed/health/fire rate per level.

### Phase 4: Economy & Power-ups
- [x] **Loot System**: Enemies drop "Money" items.
- [x] **Inventory**: Track player money and current upgrades.
- [x] **Shop UI**: Interface to spend money on upgrades.
- [x] **Power-up Implementation**:
    - [x] Better Guns (Spread, Rapid fire).
    - [x] Forcefields (Shield health).
    - [x] Extra Lives.

### Phase 5: UI & Polish
- [x] **HUD**: Display Score, Lives, Money, Current Level.
- [x] **Title Screen**: "Play", "High Scores", "Exit".
- [x] **Leaderboard System**: Save/Load top 50 scores to file.
- [ ] **Audio Integration**: Add SFX for shooting, explosions, pickups. Add BGM. (Skipped: No assets)
- [x] **Visual Effects**: Particle systems for explosions, thrusters.

## Current Status
- [x] Project Setup Complete.
- [x] Core Gameplay Implemented.
- [x] Level System Implemented.
- [x] Economy & Shop Implemented.
- [x] UI & Polish (Visuals) Implemented.

