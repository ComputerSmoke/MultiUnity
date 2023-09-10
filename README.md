# MultiUnity
A library for creating 2D multiplayer unity games. Multiunity abstracts away the networking difficulties of creating multiplayer games,
 making the process nearly as simple as creating singleplayer games.

This project is in its very early stages, and I wouldn't recommend building unity apps with it yet unless you are prepared to fix the problems which will arise. The project contains bugs, is not optimized, and is missing features. Hopefully I'll get around to improving the documentation eventually.

# Philosophy
When a unity app would create a GameObject, it instead makes a call to this package. The package creates the object and manages coordinating changes to it with other unity clients in the same "room" by routing tcp messages through the server. The package also
spawns in objects owned by other clients and such. A user of this package shouldn't have to deal with tcp messages directly, and should be able to avoid implementing as much REST stuff as possible.

Since the server must distribute each update message from each client to each other connected client, it can be thought of as taking O(k\*n\*n) network operations in some timeframe, where k is the number of networked objects owned by each client which must be updated the timeframe, and n the number of connected clients. Bandwidth is relatively expensive, so rooms are divided into shards, which likely have to be limited to just a few clients (10 maybe? has not been tested yet, and depends on the app. Likely higher with low collisions/jerk, lower with lots of jerk). Client-side prediction minimizes the number of updates necessary, but this could be undermined by GameObjects which constantly experience jerk (change in acceleration) independently of changes in user input.

# Components

## MultiunityServer 
Server-side library. 

## Shared
Classes depended on by both the client and server-side libraries.

## ClientLib
Client-side library which must be precompiled and placed the Runtime directory of the unity package (Client/Runtime). 
Note that unity projects do not depend on this directly, but instead depend on the unity package.

## Client
The unity package that user's unity projects will interact with.

## ClientTest
A simple program to test the client and server libraries.

## ServerExample
A simple server library usage example.

## Multiunity_example
A simple example unity project which uses the Client package and interfaces with ServerExample.
