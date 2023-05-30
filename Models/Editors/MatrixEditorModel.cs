using System.Data;
using ReactiveUI;

namespace SwissArmyKnife.Models.Editors; 

public class MatrixEditorModel : ReactiveObject {
    public int Index { get; set; }
    public DataView MapFiles { get; set; }
    public DataView MapHeaders { get; set; }
}