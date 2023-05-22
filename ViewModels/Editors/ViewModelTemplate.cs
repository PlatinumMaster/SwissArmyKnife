using System.Reactive;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors {
    public abstract class ViewModelTemplate : ReactiveObject {
        public abstract int SelectedIndex { get; set; }
        public ReactiveCommand<Unit, Unit> AddNew => ReactiveCommand.Create(() => OnAddNew());

        public ReactiveCommand<Unit, Unit> RemoveSelected =>
            ReactiveCommand.Create(() => OnRemoveSelected(SelectedIndex));

        public ReactiveCommand<Unit, Unit> SaveChanges => ReactiveCommand.Create(() => OnSaveChanges());

        public abstract void OnAddNew();
        public abstract void OnRemoveSelected(int index);
        public abstract void OnIndexChange(int newValue);
        public abstract void OnSaveChanges();
    }
}