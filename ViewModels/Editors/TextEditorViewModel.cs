using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaEdit.Document;
using BeaterLibrary.Formats.Text;
using Material.Dialog;
using ReactiveUI;
using SwissArmyKnife.Handlers;
using SwissArmyKnife.ViewModels.Base;

namespace SwissArmyKnife.ViewModels.Editors; 

public class TextEditorViewModel : EditorViewModelBase  {
    public bool IsTextEditorVisible => TextDocuments.Count > 0;
    private List<TextDocument?> TextDocuments;
    private TextDocument? _Current;
    public int FontSize { get; set; }
    public int TextGroup { get; set;}
    public TextDocument? Current {
        get => _Current;
        set => this.RaiseAndSetIfChanged(ref _Current, value);
    }

    public TextEditorViewModel() {
        TextDocuments = new List<TextDocument?>();
        FontSize = 12;
        // this.WhenAnyValue(vm => vm.TextGroup).Subscribe(async newGroup => {
        //     if (newGroup >= 0) {
        //         FSPath = newGroup == 0
        //             ? GameController.CurrentGameData.SystemsText
        //             : GameController.CurrentGameData.MapText;
        //         if (GameController.PatcherInstance != null) {
        //             Max = GameController.PatcherInstance.GetMaximumNARCEntryIndex(FSPath);
        //         }
        //     }
        // });
    }
    
    public override void OnAddNew() {
        throw new System.NotImplementedException();
    }

    public override void OnRemoveSelected() {
        throw new System.NotImplementedException();
    }

    public async override void OnLoadFile() {
        if (SelectedIndex >= 0 && SelectedIndex <= Max) {
            bool TextContainerOpenAlready = TextDocuments.Find(x => x.FileName.Equals(SelectedIndex.ToString())) != null;
            if (TextContainerOpenAlready) {
                // This text container is already open.
                if (Application.Current != null &&
                    Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime Desktop) {
                    DialogResult Result = await Messages.YesNoMessage(Desktop, "Reload Text Container",
                        "This text container is already open. Would you like to reload it anyway? All unsaved changes will be lost.");
                    if (Result.GetResult.Equals("No")) {
                        return;
                    }
                }
            }

            // GFText? Text = new GFText(GameController.PatcherInstance.GetARCFile(FSPath, SelectedIndex));
            // TextDocument? NewTextDocument = TextContainerOpenAlready ? TextDocuments.Find(x => x.FileName.Equals(SelectedIndex.ToString())) : new TextDocument();
            // if (NewTextDocument != null) {
            //     NewTextDocument.FileName = SelectedIndex.ToString();
            //     NewTextDocument.Text = Text.FetchTextAsString(true, true);
            //     if (!TextContainerOpenAlready) {
            //         TextDocuments.Add(NewTextDocument);
            //         Tabs.Add(new TabItem {
            //             Header = $"{(TextGroup == 0 ? "System" : "Event")} Text Container {SelectedIndex}",
            //         });
            //     }
            // }

            // CurrentTab = TextDocuments.IndexOf(NewTextDocument);
        }
    }
    
    public override void OnSaveChanges() {
        throw new System.NotImplementedException();
    }

    protected override void TryShowTabControl() {
        this.RaisePropertyChanged(nameof(IsTextEditorVisible));
        if (IsTextEditorVisible) {
            Current = TextDocuments[CurrentTab];
        }
    }
}