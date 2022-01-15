using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.ViewModels.Editors;

namespace SwissArmyKnife.Avalonia.Pages {
    public class TrainerEditor : ReactiveUserControl<TrainerEditorViewModel> {
        public TrainerEditor() {
            initializeComponent();
            if (!Design.IsDesignMode)
                DataContext = new TrainerEditorViewModel();
        }

        private void initializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}