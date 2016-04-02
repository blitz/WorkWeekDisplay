using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Globalization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Background;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WWDisplay
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Update();

            // Update our tile and schedule a timer to update it every day.
            Tasks.TileUpdateTask.UpdateTile();
            RegisterBackgroundTask();
        }

        public void Update()
        {
            var ci = new CultureInfo("en-US");
            var cal = ci.Calendar;

            wwTextBlock.Text = "It's WW" + Tasks.WorkWeek.Current().ToString() + ".";
        }

        private async void RegisterBackgroundTask()
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            if (!(result == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                  result == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)) {
                // User denied us to run a background task.
                return;
            }

            var taskname = "TileUpdateTask";

            // Remove any previously registered tasks
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == taskname)
                {
                    task.Value.Unregister(true);
                }
            }

            var builder = new BackgroundTaskBuilder();
            builder.Name = taskname;
            builder.TaskEntryPoint = typeof(Tasks.TileUpdateTask).FullName;
            builder.SetTrigger(new TimeTrigger(60 * 24 /* every day */, false /* periodic */));
            builder.Register();
        }

    }
}
