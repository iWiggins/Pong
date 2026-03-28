# Pong

A simple game built on the [GameFrame](https://github.com/iWiggins/GameFrame) framework.

This is designed as an example for the usage of the GameFrame framework, and is commented with more details than usual code for the purpose of explaining how the game works at a tutorial level.

## Design

Following the GameFrame design pattern, Pong is made from Frames and Components.

### Game

The PongGame class shows how to implement a ManagedGame GameFrame class.

Other utility classes like Palette and ResourceManager show common GameFrame design practices.

### Frames

Pong consists of three frames.

#### Main Menu

The entry point to the game.

Shows menu design using Layout and Button components, Frame navigation, a custom cursor, and handling music.

#### Options Menu

A menu to change game settings.

Shows menu design using complex menu components.

#### Pong Core

The main gameplay.

Shows UI design using Layout components and designing gameplay with custom components.

### Components

Pong uses custom components to implement gameplay objects.

#### Ball

A component that draws and updates to move around the screen, and triggers effects when collisions are detected.

#### Field

A component collecting the other custom components and implementing gameplay.

#### Paddle

A component that can be controled by user input or a simple AI

#### PongAI

A simple math based controller for a Paddle to implement a basic AI.