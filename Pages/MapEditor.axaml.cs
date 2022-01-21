﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages; 

public class MapEditor : ReactiveUserControl<MapEditorViewModel> {
    public MapEditor() {
        if (!Design.IsDesignMode)
            DataContext = new MapEditorViewModel();
        initializeComponent();
    }

    private void initializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}