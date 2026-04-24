# Spreadsheet Application
> A fully-featured spreadsheet built in C# with WinForms, implementing expression trees, the command pattern, XML persistence, and reactive cell dependencies.

---

## Table of Contents
1. [Overview](#overview)
2. [Features](#features)
   - [Cell Editing](#cell-editing)
   - [Formula Evaluation](#formula-evaluation)
   - [Supported Operators](#supported-operators)
   - [Cell Background Color](#cell-background-color)
   - [Undo / Redo](#undo--redo)
   - [Save / Load](#save--load)
   - [Clear](#clear)
3. [Error Handling](#error-handling)
4. [Unit Testing](#unit-testing)
5. [Getting Started](#getting-started)

---

## Overview

This project was developed for **CPT_S 321** at Washington State University under Professor Vanera Arnaoudova. It is a Windows Forms spreadsheet application written in **C# (.NET 10)** that demonstrates a range of software engineering concepts:

- Expression trees with Shunting-yard algorithm (infix → postfix)
- Operator extensibility via **reflection**
- Reactive cell dependency graph with circular reference detection
- Undo/redo via the **Command design pattern**
- XML-based save/load with robust error recovery
- Unit-tested engine (NUnit) decoupled from the UI layer

---

## Features

### Cell Editing

Click any cell to select it; double-click (or begin typing) to enter edit mode. Each cell maintains two distinct values:

- **Text** — the raw input as entered by the user (e.g. `=A1+B1` or `hello`)
- **Value** — the evaluated display value (e.g. `42`)

While editing, the cell displays its raw **Text**. On commit, it switches back to showing the computed **Value**.

---

### Formula Evaluation

Prefix a cell's content with `=` to enter formula mode:

```
=A1 + B2 * 3
=(C3 - 1) / D4
```

Formulas are evaluated using a **binary expression tree**:

1. The infix expression (after stripping `=`) is tokenized and converted to **postfix notation** using the [Shunting-yard algorithm](https://en.wikipedia.org/wiki/Shunting_yard_algorithm).
2. A tree of `ConstantNode`, `VariableNode`, and `OperatorNode` objects is constructed from the postfix token stream.
3. The tree is evaluated recursively.

Cell references (e.g. `B3`) are treated as variables. When a referenced cell's value changes, all dependent cells **automatically re-evaluate** via a `PropertyChanged` subscription graph.

> Cell names are **case-sensitive**. Whitespace inside formulas is ignored.

---

### Supported Operators

| Symbol | Operation | Precedence |
|---|---|---|
| `+` | Addition | 1 |
| `-` | Subtraction | 1 |
| `*` | Multiplication | 2 |
| `/` | Division | 2 |

Operators are registered at runtime using **C# reflection** — the factory scans the assembly for all `OperatorNode` subclasses and maps their `Operation` character automatically. Adding a new operator (e.g. `^` for exponentiation) requires only a new class; no factory changes needed.

---

### Cell Background Color

Select one or more cells, then navigate to **Cell → Change Background Color** to open a color picker. The chosen color is applied to all selected cells simultaneously. Color changes are fully undoable.

---

### Undo / Redo

| Action | Shortcut | Menu |
|---|---|---|
| Undo | — | Edit → Undo |
| Redo | — | Edit → Redo |

Undo/redo is implemented with the **Command design pattern**. Every mutating action (text edit, color change) is wrapped in an `ICommand` object and pushed onto an undo stack. Each command knows how to produce its own reverse via `GetReverse()`.

- **Undo** — pops from the undo stack, executes the reverse, pushes to redo stack.
- **Redo** — pops from the redo stack, executes the reverse, pushes back to undo stack.

The menu items dynamically display the action name (e.g. *Undo text change*, *Redo color change*) and are disabled when the respective stack is empty. The **Clear** operation resets both stacks and cannot be undone.

---

### Save / Load

Navigate to **File → Save** or **File → Load** to persist and restore spreadsheet state as an XML file.

Only cells that differ from their defaults (non-empty text or non-white background) are written, keeping files compact:

```xml
<spreadsheet>
  <cell>
    <name>B3</name>
    <text>=A1+1</text>
    <color>FFFF0000</color>
  </cell>
</spreadsheet>
```

On load, the spreadsheet is first cleared, then each cell is restored in document order. Malformed or unrecognized XML is handled gracefully with user-facing error dialogs rather than crashes.

---

### Clear

**File → Clear** resets every cell to its default text and color, and empties both undo and redo stacks.

---

## Error Handling

When a formula cannot be evaluated, the cell's display value is set to a descriptive error string rather than throwing an unhandled exception.

| Display Value | Cause |
|---|---|
| `invalid variable` | A token in the formula is not a valid cell name (e.g. `=hello`, `=g6` due to case sensitivity) |
| `self reference` | The formula directly or indirectly references its own cell |
| `circular reference` | A cycle exists in the cell dependency graph (e.g. A1 → B2 → C3 → A1) |
| `unsupported operator` | The formula contains an operator that is not registered in the factory |
| `unknown error` | Any other unexpected exception during evaluation |

When a circular reference is resolved (e.g. by editing one of the cells in the chain), all affected cells re-evaluate automatically.

---

## Unit Testing

Tests are located in `Spreadsheet_Testing/` (NUnit, targeting `net10.0-windows`) and `ExpressionTreeTesting/` (NUnit, targeting `net10.0`).

**SpreadsheetEngine tests cover:**
- Default `BGColor` is `0xFFFFFFFF`
- `PropertyChanged` fires on color mutation but not on no-op assignment

**ExpressionTree tests cover:**
- Constants, single variables, and multi-variable expressions
- All four arithmetic operators with correct precedence
- Parenthesis grouping and nesting
- Left-to-right associativity for same-precedence chains
- Variable re-assignment (latest value wins)
- Empty expression evaluates to `0.0`
- Mismatched parentheses evaluate to `0.0`

---

## Getting Started

**Requirements:** .NET 10 SDK, Windows (WinForms)

```bash
# Clone the repository
git clone <repo-url>
cd Spreadsheet_Thomas_Nguyen

# Build and run
dotnet run --project Spreadsheet_Thomas_Nguyen/Spreadsheet_Thomas_Nguyen.csproj

# Run tests
dotnet test
```

Or open `Spreadsheet_Thomas_Nguyen.slnx` in Visual Studio 2022+ and press **F5**.

---

*Copyright © 2026 Thomas Nguyen · MIT License*
