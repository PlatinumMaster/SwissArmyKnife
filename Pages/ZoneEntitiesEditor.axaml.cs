using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages {
    public class ZoneEntitiesEditor : ReactiveUserControl<ZoneEntitiesViewModel> {
        public ZoneEntitiesEditor() {
            InitializeComponent();
            if (!Design.IsDesignMode)
                DataContext = new ZoneEntitiesViewModel();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}