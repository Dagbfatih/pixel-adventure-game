# Pixel Adventure Game

This project is a 2D pixel adventure game developed with the Godot Engine.

---

## Assets

All pixel assets used in the game are meticulously crafted by "Pixel Frog." We are incredibly grateful for this fantastic resource!

**Source:** [Pixel Adventure 1 by Pixel Frog](https://pixelfrog-assets.itch.io/pixel-adventure-1)

---

## Project Structure and Folder Organization

The fundamental directory structure of the project is organized as follows to ensure a clean and manageable workflow:

### Folder Descriptions:

* **`assets/`**
    * This folder contains all **optimized and selected assets** (images, sounds, etc.) that are directly used within the game.
    * It only includes files that need to be part of the game and have been processed for performance.

* **`asset_source/`**
    * This folder holds **all original asset files** provided by "Pixel Frog" and downloaded from the link above.
    * **Important Note:** This folder is ignored by Git via the `.gitignore` file and is not included in the GitHub repository or the final build. This approach optimizes the project size while ensuring local access to all original assets.

* **`scripts/`**
    * All **C# scripts** for the game are organized under this folder.
    * Game mechanics, components, and other logical structures are managed here.

* **`scenes/`**
    * This folder contains all **custom Godot scenes (`.tscn` files)** and reusable nodes for the game.
    * Scenes for characters, enemies, levels, and user interface elements are located here.

---