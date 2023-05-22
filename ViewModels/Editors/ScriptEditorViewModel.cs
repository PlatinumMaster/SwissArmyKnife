using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaEdit.Document;
using BeaterLibrary.Formats.Scripts;
using Material.Dialog;
using ReactiveUI;
using SwissArmyKnife.Handlers;
using SwissArmyKnife.ViewModels.Base;

namespace SwissArmyKnife.ViewModels.Editors; 

public class ScriptEditorViewModel : EditorViewModelBase {
    public bool IsScriptEditorVisible => Documents.Count > 0;
    public int FontSize { get; set; }
    private List<TextDocument?> Documents;
    private TextDocument? _Current;
    public ObservableCollection<TabItem> Tabs { get; set; }
    public TextDocument? Current {
        get => _Current;
        set => this.RaiseAndSetIfChanged(ref _Current, value);
    }
    
    public ScriptEditorViewModel() {
        Current = new TextDocument();
        Tabs = new ObservableCollection<TabItem>();
        Documents = new List<TextDocument?>();
        FontSize = 12;
    }

    private ScriptContainer GetScriptFromARC(int ID) {
        return new ScriptContainer(
            GameController.PatcherInstance.GetARCFile(GameController.Project.GameInfo.ARCs["Events"], ID),
            Path.Combine("Resources", "Scripts"),
            "",
            -1);
    }

    private async Task<TextDocument> TryOpenOrCreateDocument() {
        TextDocument Existing = Documents.Find(x => x.FileName.Equals(SelectedIndex.ToString()));
        if (Existing != null) {
            if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
                DialogResult Result = await Messages.YesNoMessage(Desktop, "Reload Script", "This script is already open. Would you like to reload it anyway? All unsaved changes will be lost.");
                if (Result.GetResult.Equals("No")) {
                    return null;
                }
            }
        }
        return new TextDocument();
    }

    public override void OnAddNew() {
        throw new System.NotImplementedException();
    }

    public override void OnRemoveSelected() {
        throw new System.NotImplementedException();
    }

    public override async void OnLoadFile() {
        if (GameController.PatcherInstance != null && SelectedIndex >= 0 && SelectedIndex <= Max) {
            TextDocument Document = await TryOpenOrCreateDocument();
            if (Document != null) {
                ScriptContainer Script = GetScriptFromARC(SelectedIndex);
                Document.FileName = SelectedIndex.ToString();
                Document.Text = String.Empty;                
                Script.Scripts.ForEach(s => {
                    Document.Text += ScriptHandler.PrintMethod(s);
                });                
                Script.Calls.ForEach(s => {
                    Document.Text += ScriptHandler.PrintMethod(s.Data, true);
                });                
                Script.Jumps.ForEach(s => {
                    Document.Text += ScriptHandler.PrintMethod(s.Data, true);
                });

                int DocIndex = Documents.IndexOf(Document);
                if (DocIndex == -1) {
                    Tabs.Add(new TabItem() {
                        Header = $"Event Container {SelectedIndex}"
                    });
                    DocIndex = Tabs.Count - 1;
                }
                CurrentTab = DocIndex;
            }
        }
    }

    public override void OnSaveChanges() {
        throw new NotImplementedException();
    }

    protected override void TryShowTabControl() {
        this.RaisePropertyChanged(nameof(IsScriptEditorVisible));
        if (IsScriptEditorVisible) {
            Current = Documents[CurrentTab];
        }
    }
}