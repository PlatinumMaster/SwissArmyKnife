using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using BeaterLibrary.GameInfo;
using SwissArmyKnife.Avalonia.ViewModels;

namespace SwissArmyKnife.Avalonia.Pages
{
    public class TrainerEditor : ReactiveUserControl<TrainerEditorViewModel>
    {
        public TrainerEditor()
        {
            InitializeComponent();
            if (!Design.IsDesignMode)
                DataContext = new TrainerEditorViewModel(
                    UIUtil.FetchText(UIUtil.CurrentGameInformation.SystemsText,
                        (int) B2W2.ImportantSystemText.TrainerNames, false, false),
                    UIUtil.FetchText(UIUtil.CurrentGameInformation.SystemsText,
                        (int) B2W2.ImportantSystemText.TrainerClasses, false, false),
                    UIUtil.FetchText(UIUtil.CurrentGameInformation.SystemsText,
                        (int) B2W2.ImportantSystemText.BattleTypes, false, false),
                    UIUtil.FetchText(UIUtil.CurrentGameInformation.SystemsText,
                        (int) B2W2.ImportantSystemText.ItemNames, false, false),
                    UIUtil.FetchText(UIUtil.CurrentGameInformation.SystemsText,
                        (int) B2W2.ImportantSystemText.MoveNames, false, false),
                    UIUtil.FetchText(UIUtil.CurrentGameInformation.SystemsText,
                        (int) B2W2.ImportantSystemText.PokémonNames, false, false),
                    new List<string>
                    {
                        "Random",
                        "Primary",
                        "Secondary",
                        "Hidden"
                    },
                    new List<string>
                    {
                        "Random",
                        "Male",
                        "Female"
                    }
                );
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}