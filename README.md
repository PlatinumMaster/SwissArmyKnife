# SwissArmyKnife [![SwissArmyKnife.Avalonia](https://github.com/PlatinumMaster/SwissArmyKnife/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/PlatinumMaster/SwissArmyKnife/actions/workflows/dotnet-desktop.yml)

SwissArmyKnife is a cross-platform ROM editor tailored to the Generation V Pokémon games, written in C#.

The goal of SwissArmyKnife is to allow editing of the following games (and ONLY the following games):
- Pokémon White Version (IRA0)
- Pokémon Black Version (IRB0)
- Pokémon White 2 Version (IRD0)
- Pokémon Black 2 Version (IRE0)

***Please note that, at present, only Pokémon Black 2 Version and Pokémon White 2 Version are fully supported! Pokémon Black Version and Pokémon White Version also work, but support for them is limited.***

Currently, SwissArmyKnife supports editing:
- Map Containers
- Text Containers
- Event and Initialization Scripts
- Zone Headers
- Zone Entities (Interactables, NPCs, Triggers, Warps)
- Encounters (Trainer and Wild)

SwissArmyKnife uses the Hotswap patching system under the hood. 
Hotswap is a project-based patching engine which allows for ROM modifications without having to extract a ROM directly. 
This is done by utilizing the concept of a base ROM, and "swapping" any modified files into a separate serialized ROM. 
This not only allows us to have a smaller disk footprint, but it also allows for easy collaboration on ROM projects while 
providing a backbone for stability. This should also allow for a future backup system.

# Release Pattern
Stable builds will be marked as such under the `Releases` tab. Nightly builds, currently, are available through the Actions tab. **You must make a GitHub account to download the nightly builds, for now.**

# Usage
For usage of the tool, see [here](https://github.com/PlatinumMaster/SwissArmyKnife/wiki). For details on the formats,
see [here](https://github.com/PlatinumMaster/BeaterLibrary/wiki).
**Keep in mind, documentation is a work in progress.**

# Known Issues:
- "Some editors are not fully functional/look scuffed!" - I am aware of this. At the moment, I am rewriting the entire program to provide a better user experience.

Any other issues? Open an issue on this page, and I will look into it.

# Building

* Clone [BeaterLibrary](https://github.com/PlatinumMaster/BeaterLibrary), [NitroSharp](https://github.com/PlatinumMaster/NitroSharp), [Hotswap](https://github.com/PlatinumMaster/Hotswap), and this repository into separate folders,
  in the same root. So, the folder structure should look something like this:
```
root
| BeaterLibrary
| Hotswap
| NitroSharp
| SwissArmyKnife
```
* Build SwissArmyKnife using Visual Studio, JetBrains Rider, or dotnet. **Note: you will need to install NuGet dependencies
  for this project to build. Rider and Visual Studio should do this for you automatically.**

# Contributing
If you like to help in anyway, fork and submit a pull request, and I'll deal with the rest. 

# Credits:
- PlatinumMaster: Tool programming, research.
- [Hello007](https://github.com/HelloOO7), [Gonhex](https://github.com/Gonhex), [Bond697](https://github.com/Bond697), [Kaphotics](https://github.com/kwsch): Research.
- Bromley, recordreader, various members in *Kingdom of DS Hacking*: Testing and feedback.
