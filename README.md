# Picross
 
## About
This is part of my game clone series, where I take a video game and recreate it in Unity, challenging myself to write all the code and create all the graphical assets (except fonts) from scratch.

This is a clone of the puzzle game Picross (a.k.a. [Nonograms](https://en.wikipedia.org/wiki/Nonogram))

---

## Objectives

### Primary objective
* learn how to handle loading and saving many files through BinaryFormatter

### Secondary objective(s)
* improve code structure
* create a simple box blur shader

---

## Notes + other details

* Somehow I didn't hit many roadblocks while coding. Maybe it comes with having solved 3000+ puzzles over the years, but getting the overall game logic down went pretty smoothly.
* Level files are stored as plain .txt files ([ex.](https://github.com/Nic1493/Picross/blob/main/Assets/Scripts/Game/Level%20Files/20x20/%5B20x20%5D%20Yin-yang.txt)), where `0` represents an empty cell, and `1` represents a filled cell.

### Graphics 
* main menu background - reused the grid lines in the Game scene, and animated them to enter the screen in random order
* fonts - 100% free fonts found on dafont.com
* buttons + icons - done in Photoshop
