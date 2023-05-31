using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    private List<TextDocument?> Documents;
    private TextDocument? _Current;
    public bool AnyDocuments => Documents.Count > 0;
    public int FontSize { get; set; }
    private int ScriptPlugin = -1;
    public ObservableCollection<TabItem> Tabs { get; set; }
    public TextDocument? Current {
        get => _Current;
        set => this.RaiseAndSetIfChanged(ref _Current, value);
    }
    
    public ScriptEditorViewModel() {
        Max = GameWork.Patcher.GetARCMax(GameWork.Project.GameInfo.ARCs["Events"]);
        Current = new TextDocument();
        Tabs = new ObservableCollection<TabItem>();
        Documents = new List<TextDocument?>();
        FontSize = 12;
    }

    public override void OnAddNew() {
        throw new NotImplementedException();
    }

    public override void OnRemoveSelected() {
        throw new NotImplementedException();
    }

    public override async void OnLoadFile() {
        if (GameWork.Patcher != null && SelectedIndex >= 0 && SelectedIndex <= Max) {
            TextDocument Document = await TryOpenOrCreateDocument();
            ScriptContainer Script = GetScriptFromARC(SelectedIndex);
            Document.FileName = SelectedIndex.ToString();
            Document.Text = String.Empty;                
            Scripts.Reset();
            
            Script.Scripts.ForEach(s => {
                Document.Text += Scripts.PrintMethod(s, ScriptIndex: Script.Scripts.IndexOf(s) + 1);
            });  
            
            Script.Calls.ForEach(s => {
                Document.Text += Scripts.PrintMethod(s.Data, IsAnonymous: true);
            });
            
            Script.Actions.ForEach(s => {
                Document.Text += $"ActionSequence {s.GetDataToString()}";
            });

            CurrentTab = Documents.IndexOf(Document);
        }
    }

    public override void OnSaveChanges() {
        if (AnyDocuments && CurrentTab >= 0 && CurrentTab < Tabs.Count) {
            int FileIndex = int.Parse(Current.FileName);
            GameWork.Patcher.WriteARCViaVFS(GameWork.Project.GameInfo.ARCs["Events"], FileIndex, Path => {
                Scripts.Assemble(Path, Current.Text, ScriptPlugin);
            });
        }
    }

    protected override void TryShowTabControl() {
        this.RaisePropertyChanged(nameof(AnyDocuments));
        if (AnyDocuments) {
            Current = Documents[CurrentTab];
        }
    }
    
    private ScriptContainer GetScriptFromARC(int ID) {
        ScriptPlugin = -1;
        foreach (KeyValuePair<int, int[]> KVP in GameWork.Project.GameInfo.ScriptPlugins) {
            if (KVP.Value.Contains(ID)) {
                // Script plugins enabled for this script container.
                ScriptPlugin = KVP.Key;
                break;
            }
        }
        return new ScriptContainer(
            GameWork.Patcher.GetARCFile(GameWork.Project.GameInfo.ARCs["Events"], ID),
            Path.Combine("Resources", "Scripts"),
            GameWork.Project.GameInfo.Abbreviation,
            ScriptPlugin);
    }

    private async Task<TextDocument> TryOpenOrCreateDocument() {
        TextDocument Existing = Documents.Find(x => x.FileName.Equals(SelectedIndex.ToString()));
        if (Existing != null) {
            if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
                DialogResult Result = await Messages.YesNo(Desktop, "Reload Script", "This script is already open. Would you like to reload it anyway? All unsaved changes will be lost.");
                if (Result.GetResult.Equals("No")) {
                    return null;
                }
            }
        } else {
            Existing = new TextDocument() {
                FileName = SelectedIndex.ToString()
            };
            Tabs.Add(new TabItem {
                Header = $"Script Container {SelectedIndex}",
            });
            Documents.Add(Existing);
        }
        return Existing;
    }
}