using System.Collections.Generic;
using BeaterLibrary.Formats.Text;
using BeaterLibrary.GameInfo;
using SwissArmyKnife.Avalonia.Utils;

namespace SwissArmyKnife.Avalonia.Models {
    public class SpeciesNames : List<string> {
        public SpeciesNames() {
            new TextContainer(UI.patcher.fetchFileFromNarc(UI.gameInfo.systemsText, (int) B2W2.ImportantSystemText.PokémonNames)).fetchTextAsStringArray().ForEach(e => this.Add(e));
        }
    }
}