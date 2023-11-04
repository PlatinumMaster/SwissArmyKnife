using System.Data;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using SwissArmyKnife.ViewModels.Editors;

namespace SwissArmyKnife.Pages.Field; 

public partial class MatrixEditor : UserControl {
    public MatrixEditor() {
        InitializeComponent();
        DataContext = new MatrixEditorViewModel();
        // MapMatrixHeaders = this.Get<DataGrid>("MapMatrixHeaders");
        // MapMatrixMaps = this.Get<DataGrid>("MapMatrixMaps");
        // MapMatrixHeaders.PropertyChanged += MapMatrix_PropertyChange;
        // MapMatrixMaps.PropertyChanged += MapMatrix_PropertyChange;
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }

    private void MapMatrixMaps_OnLoadingRow(object? sender, DataGridRowEventArgs e) {
        e.Row.Header = e.Row.GetIndex() + 1;
    }

    private void MapMatrix_PropertyChange(object? sender, AvaloniaPropertyChangedEventArgs e) {
        DataGrid? Current = sender as DataGrid;
        if (Current == null) {
            return;
        }

        if (e.Property.Name.Equals("Items")) {
            Current.Columns.Clear();
            DataView? Table = e.NewValue as DataView;
            if (Table.Table != null) {
                DataColumnCollection Columns = Table.Table.Columns;
                for (int y = 0; y < Columns.Count; ++y) {
                    Current.Columns.Add(new DataGridTextColumn {
                        Header = Columns[y].ColumnName,
                        Binding = new Binding($"Row.ItemArray[{y}]"),
                        IsReadOnly = false
                    });
                }
            }
        }
    }
}