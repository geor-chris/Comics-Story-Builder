# Comics Story Builder

Comics Story Builder is a Windows desktop app for making simple comic strips. You pick characters, speech bubbles and backgrounds from a built-in library, place them on a page, type dialogue into the bubbles, and export each page as a PNG. The finished comic can then be read page by page in the built-in viewer.

Built with C# and Windows Forms on .NET Framework 4.7.2.

---

## Features

- **Create, load and view comics** from a start screen (`NEW PROJECT`, `LOAD PROJECT`, `VIEW COMIC`).
- **Asset library** with three categories, chosen from the top menu:
  - **Superheroes**: character images
  - **Bubbles**: speech, thought, shocked and angry bubbles
  - **Backgrounds**: full-page scenes (one per page)
- **Click to place**: click an asset in the left sidebar, then click on the canvas to drop it there.
- **Move and resize**: drag an item to move it, or drag its bottom-right corner to resize it. The selected item has a red outline.
- **Mirror**: flip the selected image horizontally so a character faces the other way.
- **Dialogue text**: right-click on a bubble to add an editable text box (Comic Sans by default). The text moves and resizes with its bubble, and its formatting is kept when you save.
- **Panel layouts**: split a page into **1, 2 or 4 panels** from the `Layout` menu.
- **Multiple pages**: add pages with `New page` and move between them with the ⏪ / ⏩ buttons.
- **Export to PNG**: `Save page` renders the current page to an image file.
- **Project files**: the whole comic (every page, item position, size, text and layout) is saved to a `.b` file, so you can reopen and keep editing it later.
- **Comic viewer**: read the exported pages one at a time with `<<` / `>>` buttons.

---

## How to use

### 1. Start a new comic
1. Launch the app and click **NEW PROJECT**.
2. Type a name for the comic (up to 20 characters) and click **START**.
3. This creates a folder `comics/<name>/` next to the executable and opens the editor.

### 2. Build a page
The editor window has three areas:

| Left sidebar | Centre | Right side |
|---|---|---|
| Asset thumbnails for the category picked in the menu | The comic page (canvas) | Action buttons and page navigation |

- Use the **Superheroes / Bubbles / Backgrounds** menu to fill the sidebar.
- Click a thumbnail, then click on the page to place it. A background fills the whole page and replaces any previous one.
- Drag to move, drag the bottom-right corner to resize, and **right-click a bubble** to type text into it.
- Use **Layout → 1 / 2 / 4 Panels** to draw panel borders.

### 3. Editor buttons

| Button | What it does |
|---|---|
| **Clear Screen** | Removes everything from the current page |
| **Mirror image** | Flips the selected item horizontally |
| **New page** | Saves the current page (PNG and project file) and starts a blank page |
| **Save page** | Exports the current page as a PNG and saves the project file |
| **Display comic** | Opens the viewer for this comic |
| **⏪ / ⏩** | Go to the previous or next page |
| **🏠** (menu) | Saves the project and goes back to the start screen |

### 4. Continue an existing comic
Click **LOAD PROJECT** on the start screen and choose the comic's `.b` file in `comics/<name>/`.

### 5. Read a comic
Click **VIEW COMIC** on the start screen (or **Display comic** in the editor) and choose a comic folder. The viewer shows every `.png` in that folder.

---

## Project structure

```
Comics Story Builder/
├── Comics Story Builder.sln
└── Comics Story Builder/
    ├── Program.cs              # Entry point, opens the Home screen
    ├── Home.cs                 # Start screen: new / load / view
    ├── Canvas.cs               # The comic editor
    ├── Viewer.cs               # Page-by-page comic viewer
    ├── AssetData.cs            # Serializable data for each item on a page
    ├── DoubleBufferedPanel.cs  # Flicker-free panel used as the canvas
    └── bin/Debug/
        ├── images/
        │   ├── Superheroes/    # Character images
        │   ├── Bubbles/        # Speech bubble images
        │   └── Backgrounds/    # Background images
        └── comics/             # Saved comics (created by the app)
            └── <ComicName>/
                ├── <ComicName>.b       # Project file
                └── <ComicName>_N.png   # Exported pages
```

### How saving works
Each item on a page is stored as an `AssetData` object: its type (image or background), position, size, image path, and the text box's content (plain text and RTF), position and size. A comic is a `List<List<AssetData>>` (one list per page) plus a list with each page's panel layout. Both are written to the `.b` file with `BinaryFormatter`.

---

## Requirements and building

- Windows
- .NET Framework 4.7.2
- Visual Studio 2019 or later, with the **.NET desktop development** workload

1. Open `Comics Story Builder.sln` in Visual Studio.
2. Build and run (F5).

The app reads assets from `images/` and saves comics to `comics/`, both **relative to the executable**. If you build to a different output folder (Release, for example), copy the `images` folder there.

### Adding your own assets
Put `.png` or `.jpg` files into `images/Superheroes`, `images/Bubbles` or `images/Backgrounds`. They show up in the editor the next time it opens.

---

