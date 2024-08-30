using HFPMapp.ViewModels.Settings;

namespace HFPMapp.Views.Settings;

public partial class SettingsView : ContentPage
{
	public SettingsView(SettingsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}