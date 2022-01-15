using System.Reactive;
using ReactiveUI;

namespace SwissArmyKnife.Avalonia.ViewModels.Editors {
    public abstract class ViewModelTemplate : ReactiveObject {
        public abstract int selectedIndex { get; set; }
        public ReactiveCommand<Unit, Unit> addNew => ReactiveCommand.Create(() => onAddNew());

        public ReactiveCommand<Unit, Unit> removeSelected =>
            ReactiveCommand.Create(() => onRemoveSelected(selectedIndex));

        public ReactiveCommand<Unit, Unit> saveChanges => ReactiveCommand.Create(() => onSaveChanges());

        public abstract void onAddNew();
        public abstract void onRemoveSelected(int index);
        public abstract void onIndexChange(int newValue);
        public abstract void onSaveChanges();
    }
}