# SwissArmyKnife

This tool aims to serve as an interim editor for various formats in Generation V. It is essentially all of my command line editors put together into one tool.

Currently, while basic, this tool can edit: 
* Scripts (HGSS*, BW*, B2W2) 
* Text (BW, B2W2)
* Overworld Placements (BW, B2W2)
* Map Containers (BW, B2W2)

To edit files, extract a file you want to edit from its respective NARC using Tinke (I highly recommend [TinkeDSi](https://github.com/R-YaTian/TinkeDSi/releases/tag/V0.9.3)).
Once you are done editing, save the file, and replace it in [TinkeDSi](https://github.com/R-YaTian/TinkeDSi/releases/tag/V0.9.3), making sure to pack the NARC. Then, save your ROM, and test.

# Bugs/Shortcomings:
- DataGridView is fragile, so the overworld placement editor may be a bit funky. From my testing, however, it seems to be fine, but who knows.
- Cross-platform support cannot be pursued until the editor is rewritten to use the Model-View-ViewModel design pattern.

For details on the formats, and what types of values to put where, see [here](https://github.com/PlatinumMaster/BeaterLibrary/wiki) and [here](https://github.com/PlatinumMaster/SwissArmyKnife/wiki). Work-in-progress.

# Building
* Clone [BeaterLibrary](https://github.com/PlatinumMaster/BeaterLibrary) and this repository into two separate folders, in the same root.
* Build SwissArmyKnife using Visual Studio, JetBrains Rider, or dotnet.

# Contributing
If you like to help in anyway, fork and submit a pull request, and I'll deal with the rest.

# Credits:
- PlatinumMaster -> Programming, research.
- Hello007, Gonhex, Bond697 -> Assistance with research.
- Bromley, recordreader -> Testing.