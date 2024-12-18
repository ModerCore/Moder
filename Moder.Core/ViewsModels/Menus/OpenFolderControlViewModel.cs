using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Moder.Core.Messages;
using Moder.Core.Services.Config;
using Moder.Core.Helper;

namespace Moder.Core.ViewsModels.Menus;

public sealed partial class OpenFolderControlViewModel : ObservableObject
{
    private readonly GlobalSettingService _globalSettings;

    public OpenFolderControlViewModel(GlobalSettingService globalSettings)
    {
        _globalSettings = globalSettings;
    }

    [RelayCommand]
    private async Task OpenModFolderAsync()
    {
        var folderPicker = WindowHelper.CreateFolderPicker();

        var result = await folderPicker.PickSingleFolderAsync();
        if (result is null)
        {
            return;
        }

        _globalSettings.ModRootFolderPath = result.Path;
        SendCompleteMessageIfReady();
    }

    [RelayCommand]
    private async Task OpenGameRootFolderAsync()
    {
        var folderPicker = WindowHelper.CreateFolderPicker();

        var result = await folderPicker.PickSingleFolderAsync();
        if (result is null)
        {
            return;
        }

        _globalSettings.GameRootFolderPath = result.Path;
        SendCompleteMessageIfReady();
    }

    private void SendCompleteMessageIfReady()
    {
        if (
            !string.IsNullOrEmpty(_globalSettings.ModRootFolderPath)
            && !string.IsNullOrEmpty(_globalSettings.GameRootFolderPath)
        )
        {
            WeakReferenceMessenger.Default.Send(new CompleteWorkFolderSelectMessage());
        }
    }
}
