using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using Avalonia.Controls;
using Hotswap.Configuration;
using MessageBox.Avalonia;
using MessageBox.Avalonia.BaseWindows.Base;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using ReactiveUI;
using SwissArmyKnife.Avalonia.Views;

namespace SwissArmyKnife.Avalonia.ViewModels
{
    public class NewProjectViewModel : ViewModelBase
    {
        private int _SelectedGame;
        private readonly TextBox ProjectName;
        private readonly TextBox ProjectPath;

        public NewProjectViewModel(List<string> BaseROMs, TextBox ProjectName, TextBox ProjectPath, Window Instance)
        {
            this.BaseROMs = new ObservableCollection<string>(BaseROMs);
            this.ProjectName = ProjectName;
            this.ProjectPath = ProjectPath;
            SetDirectory = ReactiveCommand.Create(HandleFolderChoice);
            CreateProject = ReactiveCommand.Create(() =>
            {
                if (this.ProjectName.Text.Equals(""))
                {
                    MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "Error",
                            ContentMessage = "Project Name cannot be empty!",
                            Icon = Icon.Error,
                            Style = Style.None
                        }).Show();
                    return;
                }

                if (SelectedGame == -1)
                {
                    MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "Error",
                            ContentMessage = "Base game cannot be empty!",
                            Icon = Icon.Error,
                            Style = Style.None
                        }).Show();
                    return;
                }

                if (this.ProjectPath.Text.Equals(""))
                {
                    MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "Error",
                            ContentMessage = "Project Path cannot be empty!",
                            Icon = Icon.Error,
                            Style = Style.None
                        }).Show();
                    return;
                }
                HandleProjectCreation(Instance);
            });
        }

        public ObservableCollection<string> BaseROMs { get; }

        public int SelectedGame
        {
            get => _SelectedGame;
            set => this.RaiseAndSetIfChanged(ref _SelectedGame, value);
        }

        public ReactiveCommand<Unit, Unit> SetDirectory { get; set; }
        public ReactiveCommand<Unit, Unit> CreateProject { get; set; }

        private async void HandleProjectCreation(Window Instance)
        {
            new Generator(ProjectName.Text, ProjectPath.Text, "IRDO");
            
            IMsBoxWindow<ButtonResult> OpenWindow = MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.YesNo,
                    ContentTitle = "Project successfully created.",
                    ContentMessage =
                        $"Your project \"{ProjectName.Text}\" was successfully created at {ProjectPath.Text}.\nWould you like to load this project now?",
                    Icon = Icon.Success,
                    Style = Style.Windows
                });
            Instance.Close();
            ButtonResult Result = await OpenWindow.Show();
            switch (Result)
            {
                case ButtonResult.Yes:
                    UIUtil.InitializePatcher(Path.Combine(ProjectPath.Text, $"{ProjectName.Text}.yml"));
                    new MainWindow().Show();
                    break;
                case ButtonResult.No:
                case ButtonResult.None:
                    break;
            }
        }

        private async void HandleFolderChoice()
        {
            try
            {
                string Result = await new OpenFolderDialog().ShowAsync(ProjectManagementWindow.Instance);
                if (Result.Length > 0)
                    ProjectPath.Text = Result;
            }
            catch (Exception e)
            {
                await MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Error",
                        ContentMessage = $"{e.StackTrace}\n{e.Message}",
                        Icon = Icon.Error,
                        Style = Style.None
                    }).Show();
            }
        }
    }
}