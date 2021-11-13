using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Hotswap.Configuration;
using SwissArmyKnife.Avalonia.ViewModels;

namespace SwissArmyKnife.Avalonia.Views
{
    public class NewProjectWindow : ReactiveWindow<NewProjectViewModel>
    {
        public NewProjectWindow()
        {
            InitializeComponent();
            if (!Design.IsDesignMode)
                DataContext = new NewProjectViewModel(
                    new BaseROMConfiguration("BaseROM.yml").Games,
                    this.FindControl<TextBox>("ProjectName"),
                    this.FindControl<TextBox>("ProjectPath"),
                    this
                );
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}