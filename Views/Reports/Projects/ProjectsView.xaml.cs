using HFPMapp.ViewModels.Reports.Projects;

namespace HFPMapp.Views.Reports.Projects;

public partial class ProjectsView : ContentPage
{
	public ProjectsView(ProjectsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    private void yAxis_LabelCreated(object sender, Syncfusion.Maui.Charts.ChartAxisLabelEventArgs e)
    {
        double position = e.Position;

        if(position >= 1000000)
        {
            string text = (position / 1000000).ToString("C0");
            e.Label = $"{text}M";
        }
        else if (position >= 1000 && position <= 999999)
        {
            string text = (position / 1000).ToString("C0");
            e.Label = $"{text}K";
        }
        else
        {
            e.Label = $"{position:C0}K";
        }
    }
}