using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using BeaterLibrary.Formats.Maps;
using BeaterLibrary.Formats.Nitro;
using Material.Dialog;
using ReactiveUI;
using SwissArmyKnife.Handlers;
using SwissArmyKnife.Models.Editors;
using SwissArmyKnife.ViewModels.Base;

namespace SwissArmyKnife.ViewModels.Editors; 

public class MapEditorViewModel : EditorViewModelBase {
    public override void OnAddNew() {
        throw new NotImplementedException();
    }

    public override void OnRemoveSelected() {
        throw new NotImplementedException();
    }

    public override void OnLoadFile() {
        throw new NotImplementedException();
    }

    public override void OnSaveChanges() {
        throw new NotImplementedException();
    }

    protected override void TryShowTabControl() {
        throw new NotImplementedException();
    }
}