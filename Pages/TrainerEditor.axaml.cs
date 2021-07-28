using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SwissArmyKnife.Avalonia.Controls.ViewModels;

namespace SwissArmyKnife.Avalonia.Controls.Pages
{
    public class TrainerEditor : ReactiveUserControl<UserControl>
    {
        public TrainerEditor()
        {
            InitializeComponent();
            DataContext = new TrainerEditorViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}