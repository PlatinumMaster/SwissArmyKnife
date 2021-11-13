using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BeaterLibrary.Formats.Maps;
using BeaterLibrary.Formats.Nitro;

namespace SwissArmyKnife.Avalonia.Pages
{
    public class MapEditor : UserControl
    {
        private readonly CheckBox BuildingPosCheck;
        private MapContainer Current;
        private readonly ComboBox MapType;
        private readonly TextBlock NSBMD_Name;
        private readonly CheckBox Perms2Check;
        private readonly CheckBox PermsCheck;

        public MapEditor()
        {
            Current = new MapContainer(0x4257);
            InitializeComponent();
            NSBMD_Name = this.FindControl<TextBlock>("ModelName");
            MapType = this.FindControl<ComboBox>("MapType");
            PermsCheck = this.FindControl<CheckBox>("PermsCheck");
            Perms2Check = this.FindControl<CheckBox>("SecondaryPermsCheck");
            BuildingPosCheck = this.FindControl<CheckBox>("BuildingPosCheck");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void HandleContainer(bool Saving)
        {
            void BinaryToMapContainer(string path)
            {
                Current = new MapContainer(new BinaryReader(File.OpenRead(path)));
                switch (Current.ContainerType)
                {
                    case MapContainer.MagicLabels.WB:
                        MapType.SelectedIndex = 0;
                        break;
                    case MapContainer.MagicLabels.GC:
                        MapType.SelectedIndex = 1;
                        break;
                    case MapContainer.MagicLabels.NG:
                        MapType.SelectedIndex = 2;
                        break;
                    case MapContainer.MagicLabels.RD:
                        MapType.SelectedIndex = 3;
                        break;
                }

                UpdateNSBMDName();
                UpdateChecks();
            }

            void MapContainerToBinary(string path)
            {
                Current.Serialize(path);
            }

            UIUtil.HandleFile(Saving, BinaryToMapContainer, MapContainerToBinary, new List<FileDialogFilter>());
        }

        public void HandleModel(bool Saving)
        {
            void BinaryToModel(string path)
            {
                Current.Model = new NitroSystemBinaryModel(File.ReadAllBytes(path));
                UpdateNSBMDName();
            }

            void ModelToBinary(string path)
            {
                File.WriteAllBytes(path, Current.Model.Data);
            }

            UIUtil.HandleFile(Saving, BinaryToModel, ModelToBinary, new List<FileDialogFilter>());
        }

        private void HandlePermissions(bool Saving)
        {
            void BinaryToPermissions(string path)
            {
                Current.Permissions = new List<byte>(File.ReadAllBytes(path));
                UpdateChecks();
            }

            void PermissionsToBinary(string path)
            {
                File.WriteAllBytes(path, Current.Permissions.ToArray());
            }

            UIUtil.HandleFile(Saving, BinaryToPermissions, PermissionsToBinary, new List<FileDialogFilter>());
        }

        private void HandlePermissions2(bool Saving)
        {
            void BinaryToPermissions2(string path)
            {
                Current.Permissions2 = new List<byte>(File.ReadAllBytes(path));
                UpdateChecks();
            }

            void Permissions2ToBinary(string path)
            {
                File.WriteAllBytes(path, Current.Permissions2.ToArray());
            }

            UIUtil.HandleFile(Saving, BinaryToPermissions2, Permissions2ToBinary, new List<FileDialogFilter>());
        }

        private void HandleBuildingPositions(bool Saving)
        {
            void BinaryToBuildingPositions(string path)
            {
                Current.Permissions = new List<byte>(File.ReadAllBytes(path));
                UpdateChecks();
            }

            void BuildingPositionsToBinary(string path)
            {
                File.WriteAllBytes(path, Current.BuildingPositions.ToArray());
            }

            UIUtil.HandleFile(Saving, BinaryToBuildingPositions, BuildingPositionsToBinary,
                new List<FileDialogFilter>());
        }

        private void ImportModelOnClick(object? sender, RoutedEventArgs e)
        {
            HandleModel(false);
        }

        private void ExportModelOnClick(object? sender, RoutedEventArgs e)
        {
            HandleModel(true);
        }

        private void RemoveModelOnClick(object? sender, RoutedEventArgs e)
        {
            Current.Model = new NitroSystemBinaryModel();
            UpdateNSBMDName();
        }

        private void ImportPermissionsOnClick(object? sender, RoutedEventArgs e)
        {
            HandlePermissions(false);
            UpdateChecks();
        }

        private void ExportPermissionsOnClick(object? sender, RoutedEventArgs e)
        {
            HandlePermissions(true);
            UpdateChecks();
        }

        private void RemovePermissionsOnClick(object? sender, RoutedEventArgs e)
        {
            Current.Permissions = new List<byte>();
            UpdateChecks();
        }

        private void ImportPermissions2OnClick(object? sender, RoutedEventArgs e)
        {
            HandlePermissions2(false);
            UpdateChecks();
        }

        private void ExportPermissions2OnClick(object? sender, RoutedEventArgs e)
        {
            HandlePermissions2(true);
            UpdateChecks();
        }

        private void RemovePermissions2OnClick(object? sender, RoutedEventArgs e)
        {
            Current.Permissions2 = new List<byte>();
            UpdateChecks();
        }

        private void ImportBuildingPositionsOnClick(object? sender, RoutedEventArgs e)
        {
            HandleBuildingPositions(false);
            UpdateChecks();
        }

        private void ExportBuildingPositionsOnClick(object? sender, RoutedEventArgs e)
        {
            HandleBuildingPositions(true);
            UpdateChecks();
        }

        private void RemoveBuildingPositionsOnClick(object? sender, RoutedEventArgs e)
        {
            Current.BuildingPositions = new List<byte>();
            UpdateChecks();
        }

        private void UpdateNSBMDName()
        {
            NSBMD_Name.Text = $"({Current.Model.Name})";
        }

        private void UpdateChecks()
        {
            PermsCheck.IsChecked = Current.Permissions.Count > 0;
            Perms2Check.IsChecked = Current.Permissions2.Count > 0;
            BuildingPosCheck.IsChecked = Current.BuildingPositions.Count > 0;
        }

        private void MapType_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (MapType == null)
                return;

            switch (MapType.SelectedIndex)
            {
                case 0:
                    Current.Magic = (ushort) MapContainer.MagicLabels.WB;
                    break;
                case 1:
                    Current.Magic = (ushort) MapContainer.MagicLabels.GC;
                    break;
                case 2:
                    Current.Magic = (ushort) MapContainer.MagicLabels.NG;
                    break;
                case 3:
                    Current.Magic = (ushort) MapContainer.MagicLabels.RD;
                    break;
            }
        }
    }
}