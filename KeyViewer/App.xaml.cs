using System.Threading.Tasks;
using System;
using System.Windows;
using LogSystem;
using System.Windows.Threading;

namespace KeyViewer.View
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Handling uncovered UI exceptions
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            //Global exception
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //Handling Uncaptured Exceptions in Task
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "TaskScheduler_UnobservedTaskException");
            Logger.Instance.WriteError($"{e.Exception.GetType()}: {e.Exception.Message}");
            //Exceptions are marked as "detected" so that the program does not crash
            e.SetObserved();
        }

        /// <summary>
        /// Handling exceptions to uncovered non-UI threads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Instance.WriteError($"{e.GetType()}: {e}");
            MessageBox.Show("something is wrong.", "CurrentDomain_UnhandledException");
        }

        /// <summary>
        /// Handling uncovered UI exceptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //Identification exception has been handled
            e.Handled = true;

            Logger.Instance.WriteError($"{e.Exception.GetType()}: {e.Exception.Message}");
            MessageBox.Show(e.Exception.Message, "DispatcherUnhandledException");
        }
    }
}
