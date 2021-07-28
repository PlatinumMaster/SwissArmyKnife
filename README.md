# SwissArmyKnife
SwissArmyKnife aims to serve as an interim editor for various formats in Generation V.

Currently, this tool can edit and create: 
* Scripts (HGSS*, BW*, B2W2) 
* Text Banks (BW, B2W2)
* Zone Properties (BW*, B2W2)
* Zone Entities (BW*, B2W2)
* Wild Encounters (BW, B2W2)
* Map Containers (BW, B2W2)

(*) = This means that this game was not fully tested, and may still be problematic.

# Usage
For usage of the tool, see [here](https://github.com/PlatinumMaster/SwissArmyKnife/wiki).
For details on the formats, see [here](https://github.com/PlatinumMaster/BeaterLibrary/wiki). 
**Keep in mind, both are work in progress.**

To edit files, extract a file you want to edit from its respective NARC using Tinke (I highly recommend [TinkeDSi](https://github.com/R-YaTian/TinkeDSi/releases/tag/V0.9.3)).
Once you are done editing, save the file, and replace it using either [TinkeDSi](https://github.com/R-YaTian/TinkeDSi/releases/tag/V0.9.3) (making sure to pack the NARC), or your Hotswap project (coming soon). Then, save your ROM, and test.

# Known Issues:
- "This tool does not work with a ROM. 0/10.": The tool will be updated to do this soon. I wrote my own project based format that needs to be implemented into this tool (see: Hotswap).
- "The trainer editor is disabled!": This is intentional. Until I implement the project based format, I will keep it this way. 

Any other issues? Open an issue on this page, and I will look into it.

# Building
* Clone [BeaterLibrary](https://github.com/PlatinumMaster/BeaterLibrary) and this repository into two separate folders, in the same root.
* Build SwissArmyKnife using Visual Studio, JetBrains Rider, or dotnet. **Note: you will need to install dependencies for this project to build, such as Avalonia, ReactiveUI, Avalonia.AvaloniaEdit, and MessageBox.Avalonia**.

# Contributing
If you like to help in anyway, fork and submit a pull request, and I'll deal with the rest.

# Credits:
- PlatinumMaster: Tool programming, research.
- Hello007: Research, help with optimizations.
- Gonhex: Research, bug testing.
- Bond697: Research (documented IDA database).
- Bromley: Testing.
- recordreader: Testing.
