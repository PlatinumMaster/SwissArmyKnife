using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SwissArmyKnife.Avalonia.Views;


public partial class Flow : Window {
    public Flow() {
        InitializeComponent();
        NavDrawerSwitch = this.Get<ToggleButton>(nameof(NavDrawerSwitch));
        DrawerList = this.Get<ListBox>(nameof(DrawerList));
        DrawerList.PointerReleased += DrawerSelectionChanged;
        PageCarousel = this.Get<Carousel>(nameof(PageCarousel));
        CurrentTabName = this.Get<TextBlock>(nameof(CurrentTabName));
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
    
    public void DrawerSelectionChanged(object sender, RoutedEventArgs args)
    {
        var listBox = sender as ListBox;
        if (!listBox.IsFocused && !listBox.IsKeyboardFocusWithin) {
            return;
        }

        PageCarousel.SelectedIndex = listBox.SelectedIndex;
        CurrentTabName.Text = ((listBox.SelectedItem as DockPanel).Children[1] as TextBlock).Text;
        NavDrawerSwitch.IsChecked = false;
    }
}