// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace Wpf.Ui.Gallery.ViewModels.Pages.OpSystem;

public partial class FilePickerViewModel : ObservableObject
{
    public ObservableCollection<string> SelectedFiles { get; } = new ObservableCollection<string>();

    [RelayCommand]
    public void OnnOpenMultiple()
    {

        OpenFileDialog openFileDialog =
            new()
            {
                Multiselect = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "All files (*.*)|*.*"
            };

        if (openFileDialog.ShowDialog() != true)
        {
            return;
        }

        SelectedFiles.Clear();
        foreach (var filePath in openFileDialog.FileNames)
        {
            SelectedFiles.Add(System.IO.Path.GetFileName(filePath)); // Add only the file name
        }

    }

    [RelayCommand]
    public void RemoveFile(string fileName)
    {
        if (SelectedFiles.Contains(fileName))
        {
            SelectedFiles.Remove(fileName);
        }
    }
}
