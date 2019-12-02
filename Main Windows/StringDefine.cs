using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main_Windows
{
    public class StringDefine : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaiseEventHandler(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string value;
        public string Value
        {
            get => value;
            set
            {
                this.value = value;
                RaiseEventHandler("Value");
            }
        }
    }

    class FileName : StringDefine
    {
        private string error;
        public string Error
        {
            get => error;
            set
            {
                error = value;
                RaiseEventHandler("Error");
            }
        }

        private string path;
        public string Path
        {
            get => path;
            set
            {
                path = value;
                RaiseEventHandler("Path");
            }
        }

        private string previewFilename;
        public string PreviewFilename
        {
            get => previewFilename;
            set
            {
                previewFilename = value;
                RaiseEventHandler("NewFileName");
            }
        }

        private string batchState;
        public string BatchState
        {
            get => batchState;
            set
            {
                batchState = value;
                RaiseEventHandler("BatchState");
            }
        }


        public string FailedActions { get; set; } = "";

        public void ClearState()
        {
            BatchState = "";
            FailedActions = "";
        }
    }

    public class Foldername : StringDefine
    {
        private string error;
        public string Error
        {
            get => error;
            set
            {
                error = value;
                RaiseEventHandler("Error");
            }
        }

        private string path;
        public string Path
        {
            get => path;
            set
            {
                path = value;
                RaiseEventHandler("Path");
            }
        }

        private string previewFoldername;
        public string PreviewFoldername
        {
            get => previewFoldername;
            set
            {
                previewFoldername = value;
                RaiseEventHandler("NewFoldername");
            }
        }


        private string batchState;
        public string BatchState
        {
            get => batchState;
            set
            {
                batchState = value;
                RaiseEventHandler("BatchState");
            }
        }


        public string FailedActions { get; set; } = "";

        public void ClearState()
        {
            BatchState = "";
            FailedActions = "";
        }
    }

    public static class TypeEventHandler
    {
        /// <summary>
        /// event when new argument comes
        /// </summary>
        /// <param name="arguments"></param>
        public delegate void NewArgumentEventHandler(List<string> arguments);

        /// <summary>
        /// event when new instance of action comes
        /// </summary>
        /// <param name="action"></param>
        public delegate void NewActionEventHandler();
    }


}
