using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using UIKit;
using Foundation;

namespace BackgroundExecution
{
    public partial class HomeScreen : UIViewController
    {

        private nint _taskID;

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need
        // to be able to be created from a xib rather than from managed code

        public HomeScreen(IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public HomeScreen(NSCoder coder) : base(coder)
        {
        }

        public HomeScreen() : base("HomeScreen", null)
        {
        }

        #endregion

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Background Execution";

            BtnStartLongRunningTask.TouchUpInside += (s, e) =>
            {
                Task.Factory.StartNew(DoSomething);
            };
        }

        private void DoSomething()
        {
            // register our background task
            _taskID = UIApplication.SharedApplication.BeginBackgroundTask(() =>
            {

                var taskId = _taskID;
                Console.WriteLine("Running out of time to complete you background task!");

                UIApplication.SharedApplication.EndBackgroundTask(taskId);
            });

            Console.WriteLine("Starting background task {0}", _taskID);

            // sleep for five seconds
            Thread.Sleep(5000);

            Console.WriteLine("Background task {0} completed.", _taskID);

            // mark our background task as complete
            UIApplication.SharedApplication.EndBackgroundTask(_taskID);
        }

    }
}

