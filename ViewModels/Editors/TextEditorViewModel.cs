using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaEdit.Document;
using BeaterLibrary.Formats.Scripts;
using BeaterLibrary.Formats.Text;
using Material.Dialog;
using ReactiveUI;
using SwissArmyKnife.Handlers;
using SwissArmyKnife.ViewModels.Base;

namespace SwissArmyKnife.ViewModels.Editors; 

public class TextEditorViewModel : EditorViewModelBase  {
    private List<TextDocument?> Documents;
    private TextDocument? _Current;
    private List<GFText> GFTextData;
    public bool AnyDocuments => Documents.Count > 0;
    public int FontSize { get; set; }
    public int TextGroup { get; set;}
    private int ARC;
    public TextDocument? Current {
        get => _Current;
        set => this.RaiseAndSetIfChanged(ref _Current, value);
    }

    public TextEditorViewModel() {
        Max = GameWork.Patcher.GetARCMax(GameWork.Project.GameInfo.ARCs["Events"]);
        GFTextData = new List<GFText>();
        Current = new TextDocument();
        Tabs = new ObservableCollection<TabItem>();
        Documents = new List<TextDocument?>();
        FontSize = 16;
        this.WhenAnyValue(vm => vm.TextGroup).Subscribe(async newGroup => {
            if (newGroup >= 0) {
                ARC = GameWork.Project.GameInfo.ARCs[newGroup == 0 ? "System Text" : "Event Text"];
                Max = GameWork.Patcher.GetARCMax(ARC);
            }
        });
    }
    
    public override void OnAddNew() {
        throw new System.NotImplementedException();
    }

    public override void OnRemoveSelected() {
        throw new System.NotImplementedException();
    }

    public override async void OnLoadFile() {
        if (GameWork.Patcher != null && SelectedIndex >= 0 && SelectedIndex <= Max) {
            TextDocument Document = await TryOpenOrCreateDocument();
            GFText Text = GetTextFromARC(SelectedIndex);
            Document.FileName = SelectedIndex.ToString();
            Document.Text = Text.FetchTextAsString(true, true);
            GFTextData.Add(Text);
            CurrentTab = Documents.IndexOf(Document);
        }
    }
    
    public override void OnSaveChanges() {
        if (AnyDocuments && CurrentTab >= 0 && CurrentTab < Tabs.Count) {
            int FileIndex = int.Parse(Current.FileName);
            GameWork.Patcher.WriteARCViaVFS(ARC, FileIndex, Path => {
                GFTextData[CurrentTab].Serialize(Current.Text, Path);
            });
        }
    }

    protected override void TryShowTabControl() {
        this.RaisePropertyChanged(nameof(AnyDocuments));
        if (AnyDocuments) {
            Current = Documents[CurrentTab];
        }
    }
    
    private async Task<TextDocument> TryOpenOrCreateDocument() {
        TextDocument Existing = Documents.Find(x => x.FileName.Equals(SelectedIndex.ToString()));
        if (Existing != null) {
            if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
                DialogResult Result = await Messages.YesNo(Desktop, "Reload Text Container", "This text container is already open. Would you like to reload it anyway? All unsaved changes will be lost.");
                if (Result.GetResult.Equals("No")) {
                    return null;
                }
            }
        } else {
            Existing = new TextDocument() {
                FileName = SelectedIndex.ToString()
            };
            Tabs.Add(new TabItem {
                Header = $"Text Container {SelectedIndex}",
            });
            Documents.Add(Existing);
        }
        return Existing;
    }
    
    private GFText GetTextFromARC(int ID) {
        return new GFText(GameWork.Patcher.GetARCFile(ARC, ID));
    }
}