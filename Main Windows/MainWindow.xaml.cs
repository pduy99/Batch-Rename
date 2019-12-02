using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main_Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        bool flagPreviewed = false; //true: previewed, false: have not previewed
        bool flagError = false; //true: there is errors, false: no errors
        string pathPreset = AppDomain.CurrentDomain.BaseDirectory + "//Preset" ;
      
        List<StringMethod> _prototype = new List<StringMethod>();
        BindingList<StringMethod> _actions = new BindingList<StringMethod>();
        BindingList<FileName> _listfilenames = new BindingList<FileName>();
        BindingList<Foldername> _listfoldernames = new BindingList<Foldername>();
        List<StringMethod> _listapplyactions = new List<StringMethod>();
        BindingList<string> _listpreset = new BindingList<string>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get Preset
            Directory.CreateDirectory(pathPreset);
            getPreset();


            //Load methods 
            var movePrototype = new MoveMethod()
            {
                Args = new MoveArgs()
                {
                    FrontToEnd = false,
                }
            };

            var replacePrototype = new ReplaceMethod()
            {
                Args = new ReplaceArgs()
                {
                    From = "",
                    To = ""
                }
            };

            var newCasePrototype = new NewCaseMethod()
            {
                Args = new NewCaseArgs()
                {
                    style = 3,
                }
            };

            var trimPrototype = new TrimMethod()
            {
                Args = new TrimArgs()
                {
                    
                }
            };

            var fullNameNormalizePrototype = new FullnameNormalizeMethod()
            {
                Args = new FullnameNormalizeArgs()
                {
                    
                }
            };

            var uniqueNamePrototype = new UniqueNameMethod()
            {
                Args = new UniqueNameArgs()
                {

                }
            };

            _prototype.Add(movePrototype);
            _prototype.Add(replacePrototype);
            _prototype.Add(newCasePrototype);
            _prototype.Add(trimPrototype);
            _prototype.Add(fullNameNormalizePrototype);
            _prototype.Add(uniqueNamePrototype);


            MethodCombobox.ItemsSource = _prototype;
            operationListBox.ItemsSource = _actions;
            FileNameListView.ItemsSource = _listfilenames;
            FolderNameListView.ItemsSource = _listfoldernames;
            PresetCombobox.ItemsSource = _listpreset;

            
        }

        private void getPreset()
        {
            string[] presetFiles = Directory.GetFiles(pathPreset);
            foreach (var filename in presetFiles)
            {
                string temp = filename.Replace(pathPreset, "");
                temp = temp.Replace("\\", "");
                _listpreset.Add(temp);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshButton_Click_1(object sender, RoutedEventArgs e)
        {
            _listapplyactions.Clear();
            _actions.Clear();
            _listfilenames.Clear();
            _listfoldernames.Clear();
            _listpreset.Clear();
            getPreset();
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Batch rename written by Pham Hoang Phuoc Duy - MSSV: 1712018","Help");
        }

        private void BtnStartBatch_Click(object sender, RoutedEventArgs e)
        {
            //Check if there is no file and folder added
            if (_listfilenames.Count() == 0 && _listfoldernames.Count() == 0 || _listapplyactions.Count() == 0)
            {

                if (_listapplyactions.Count() == 0)
                {
                    MessageBox.Show("You haven't added method","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("No file and folder added", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                //Show Warning box when user haven't previewed yet
                foreach (var filename in _listfilenames)
                {
                    if (filename.PreviewFilename == null)
                    {
                        flagPreviewed = false;
                        break;
                    }
                    flagPreviewed = true;
                }
                foreach(var foldername in _listfoldernames)
                {
                    if(foldername.PreviewFoldername == null)
                    {
                        flagPreviewed = false;
                        break;
                    }
                    flagPreviewed = true;
                }

                //Haven't previewed
                if (!flagPreviewed)
                {
                    string message = "You haven't previewed yet. Do you want to preview before starting batch ?";
                    string caption = "Warning";
                    MessageBoxButton buttons = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icon = MessageBoxImage.Question;
                    MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon);

                    if (result == MessageBoxResult.Yes)
                    {
                        btnPreviewFileName(this, new RoutedEventArgs());
                    }
                    if (result == MessageBoxResult.No)
                    {
                        //Start batch here
                        //Operate string first and rename
                        if (_listfilenames.Count() != 0)
                        {
                            
                            btnPreviewFileName(this, new RoutedEventArgs());
                            
                            foreach(var filename in _listfilenames)
                            {
                                if (filename.Error == "")
                                {
                                    string newName = filename.Path + filename.PreviewFilename;
                                    string oldName = filename.Path + filename.Value;
                                    try
                                    {
                                        if (File.Exists(newName))
                                        {
                                            int count = 1;
                                            string cap = "Name Colision ";
                                            string mes = "Click OK to automatically change the name, click cancel to ignore the file";
                                            MessageBoxResult messageBoxResult = MessageBox.Show(mes, cap, MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                                            if (messageBoxResult == MessageBoxResult.Cancel)
                                            {
                                                continue;
                                            }
                                            if (messageBoxResult == MessageBoxResult.OK)
                                            {
                                                newName += count.ToString();
                                            }

                                            File.Move(oldName, newName);
                                        }
                                        else
                                        {
                                            File.Move(oldName, newName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                    }
                                }
                            }
                            
                        }
                        if(_listfoldernames.Count() != 0)
                        {
                            btnPreviewFolderName(this, new RoutedEventArgs());

                            foreach (var foldername in _listfoldernames)
                            {
                                if (foldername.Error == "")
                                {
                                    string newName = foldername.Path + foldername.PreviewFoldername;
                                    string oldName = foldername.Path + foldername.Value;
                                    string tempName = foldername.Path + "temp";
                                    try
                                    {
                                        if (Directory.Exists(newName))
                                        {
                                            int count = 1;
                                            string cap = "Name Colision ";
                                            string mes = "Click OK to automatically change the name, click cancel to ignore the file";
                                            MessageBoxResult messageBoxResult = MessageBox.Show(mes, cap, MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                                            if (messageBoxResult == MessageBoxResult.Cancel)
                                            {
                                                continue;
                                            }
                                            if (messageBoxResult == MessageBoxResult.OK)
                                            {
                                                newName += count.ToString();
                                            }

                                            Directory.Move(oldName, tempName);
                                            Directory.Move(tempName, newName);
                                        }
                                        else
                                        {
                                            Directory.Move(oldName, tempName);
                                            Directory.Move(tempName, newName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                    }
                                }
                            }
                           
                        }
                        MessageBox.Show("RENAME DONE !");
                    }
                    else
                    {
                        //do nothing
                    }
                }
                else
                {
                   //Have previewed so just directly rename, don't need to Operate string again
                   if(_listfilenames.Count() != 0)
                    {
                        foreach (var filename in _listfilenames)
                        {
                            if(filename.Error=="")
                            { 
                                string newName = filename.Path + filename.PreviewFilename;
                                string oldName = filename.Path + filename.Value;
                                try
                                {
                                    if (File.Exists(newName))
                                    {
                                        int count = 1;
                                        string cap = "Name Colision ";
                                        string mes = "Click OK to automatically change the name, click cancel to ignore the file";
                                        MessageBoxResult messageBoxResult = MessageBox.Show(mes, cap, MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                                        if (messageBoxResult == MessageBoxResult.Cancel)
                                        {
                                            continue;
                                        }
                                        if (messageBoxResult == MessageBoxResult.OK)
                                        {
                                            newName += count.ToString();
                                        }

                                        File.Move(oldName, newName);
                                    }
                                    else
                                    {
                                        File.Move(oldName, newName);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.GetType().Name);
                                }
                            }
                        }

                    }

                   if(_listfoldernames.Count() != 0)
                    {

                        foreach (var foldername in _listfoldernames)
                        {
                            if (foldername.Error == "")
                            {
                                string newName = foldername.Path + foldername.PreviewFoldername;
                                string oldName = foldername.Path + foldername.Value;
                                string tempName = foldername.Path + "temp";
                                try
                                {
                                    if (Directory.Exists(newName))
                                    {
                                        int count = 1;
                                        string cap = "Name Colision ";
                                        string mes = "Click OK to automatically change the name, click cancel to ignore the file";
                                        MessageBoxResult messageBoxResult = MessageBox.Show(mes, cap, MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                                        if (messageBoxResult == MessageBoxResult.Cancel)
                                        {
                                            continue;
                                        }
                                        if (messageBoxResult == MessageBoxResult.OK)
                                        {
                                            newName += count.ToString();
                                        }

                                        Directory.Move(oldName, tempName);
                                        Directory.Move(tempName, newName);
                                    }
                                    else
                                    {
                                        Directory.Move(oldName, tempName);
                                        Directory.Move(tempName, newName);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.GetType().Name);
                                }
                            }
                        }
                   }
                }
            }
        }

        private void MethodCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var action = MethodCombobox.SelectedItem as StringMethod;
            _actions.Add(action.Clone());
            flagPreviewed = false;
        }

        private void BtnAddMethodArgs_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton button = (ToggleButton)sender;
            //Thay đổi icon của button dựa trên trạng thái đóng mở của listitem
            Image image = new Image();
            image = button.Content as Image;
            if (image.Source == FindResource("Plus") as ImageSource)
            {
                
                image.Source = FindResource("Minus") as ImageSource;
                button.Content = image;

                //show dialog
                var item = operationListBox.SelectedItem as StringMethod;
                item.Config();
            
                //Change the icon when the dialog is closed
                image.Source = FindResource("Plus") as ImageSource;
                button.Content = image;
                button.IsChecked = false;
            }
            


         
        }

        private void BtnDelMethod_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if(cmd.DataContext is StringMethod)
            {
                StringMethod delete = (StringMethod)cmd.DataContext;
                _actions.Remove(delete);
                _listapplyactions.Remove(delete);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpMostMethodPosition(object sender, RoutedEventArgs e)
        {
            int index = (operationListBox.Items.IndexOf(operationListBox.SelectedItem));
            if(index > 0)
            {
                _actions.Insert(0, _actions[index]);
                _actions.RemoveAt(index+1);
                operationListBox.SelectedItems.Add(_actions[0]);
            }
        }

        private void btnDownMostMethodPosition(object sender, RoutedEventArgs e)
        {
            int index = (operationListBox.Items.IndexOf(operationListBox.SelectedItem));
            int numElement = _actions.Count;
            if (index < numElement -1 && index >= 0 )
            {
                _actions.Insert(numElement, _actions[index]);
                _actions.RemoveAt(index);
                //operationListBox.SelectedItems.Add(_actions[numElement - 1]);
                operationListBox.SelectedItems.Add(_actions[numElement - 1]);
            }

        }

        private void btnDownMethodPosition(object sender, RoutedEventArgs e)
        {
            
            int index = (operationListBox.Items.IndexOf(operationListBox.SelectedItem));
            if (index != (_actions.Count - 1) && index >=0)
            {
                var temp = _actions[index+1];
                _actions.Insert(index, temp);
                _actions.RemoveAt(index + 2);
            }
        }

        private void btnUpMethodPosition(object sender, RoutedEventArgs e)
        {
            int index = (operationListBox.Items.IndexOf(operationListBox.SelectedItem));
            if (index > 0)
            {
                var temp = _actions[index];
                _actions.Insert(index - 1, temp);
                _actions.RemoveAt(index + 1);
                operationListBox.SelectedItems.Add(_actions[index - 1]);
            }
            
            


        }

        private void BtnClearMethod_Click(object sender, RoutedEventArgs e)
        {
            
            _actions.Clear();
        }

        private void btnAddFiles(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog FBD = new System.Windows.Forms.FolderBrowserDialog();

            // show dialog
            System.Windows.Forms.DialogResult result = FBD.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // get all filenames in path
                string path = FBD.SelectedPath + "\\";
                string[] filenames = Directory.GetFiles(path);

                // add all to filenameList
                foreach (var filename in filenames)
                {
                    string newFilename = filename.Remove(0, path.Length);
                    _listfilenames.Add(new FileName() { Value = newFilename, Path = path });
                }
            }

        }

        private void btnAddFolder(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog folderDlg = new System.Windows.Forms.FolderBrowserDialog();

            // show dialog
            System.Windows.Forms.DialogResult result = folderDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // get all foldernames
                string path = folderDlg.SelectedPath + "\\";
                string[] foldernames = Directory.GetDirectories(path);

                // add all to foldername list
                foreach (var foldername in foldernames)
                {
                    string newFoldername = foldername.Remove(0, path.Length);
                    _listfoldernames.Add(new Foldername() { Value = newFoldername, Path = path });
                }
            }
        }

        private void btnPreviewFileName(object sender, RoutedEventArgs e)
        {
            foreach(var filename in _listfilenames)
            {
                string newFileName = filename.Value;
                string error = "";
                foreach(var action in _listapplyactions)
                {
                    try
                    {
                        newFileName = action.Operate(newFileName, true);
                    }catch(Exception er)
                    {
                        error = er.GetType().Name;
                    }
                }

                filename.PreviewFilename = newFileName;
                filename.Error = error;
               
                
            }
        }

        private void btnPreviewFolderName(object sender, RoutedEventArgs e)
        {
            foreach (var foldername in _listfoldernames)
            {
                string newFolderName = foldername.Value;
                string error = "";
                foreach (var action in _listapplyactions)
                {
                    try
                    {
                        newFolderName = action.Operate(newFolderName, true);
                    }catch(Exception er)
                    {
                        error = er.GetType().Name;
                    }
                }

                foldername.PreviewFoldername = newFolderName;
                foldername.Error = error;

            }
        }

        private void btnUpFoldernamePosition(object sender, RoutedEventArgs e)
        {
            int index = (FolderNameListView.Items.IndexOf(FolderNameListView.SelectedItem));
            if (index > 0)
            {
                var temp = _listfoldernames[index];
                _listfoldernames.Insert(index - 1, temp);
                _listfoldernames.RemoveAt(index + 1);
                FolderNameListView.SelectedItems.Add(_listfoldernames[index - 1]);
            }
        }

        private void btnUpMostFoldernamePosition(object sender, RoutedEventArgs e)
        {
            int index = (FolderNameListView.Items.IndexOf(FolderNameListView.SelectedItem));
            if (index > 0)
            {
                _listfoldernames.Insert(0, _listfoldernames[index]);
                _listfoldernames.RemoveAt(index + 1);
                FolderNameListView.SelectedItems.Add(_listfoldernames[0]);
            }
        }

        private void btnDownFoldernamePosition(object sender, RoutedEventArgs e)
        {

            int index = (FolderNameListView.Items.IndexOf(FolderNameListView.SelectedItem));
            if (index != (_listfoldernames.Count - 1) && index >= 0)
            {
                var temp = _listfoldernames[index + 1];
                _listfoldernames.Insert(index, temp);
                _listfoldernames.RemoveAt(index + 2);
            }
        }

        private void btnDownMostFoldernamePosition(object sender, RoutedEventArgs e)
        {
            int index = (FolderNameListView.Items.IndexOf(FolderNameListView.SelectedItem));
            int numElement = _listfoldernames.Count;
            if (index < numElement - 1 && index >= 0)
            {
                _listfoldernames.Insert(numElement, _listfoldernames[index]);
                _listfoldernames.RemoveAt(index);
                //operationListBox.SelectedItems.Add(_actions[numElement - 1]);
                FolderNameListView.SelectedItems.Add(_listfoldernames[numElement - 1]);
            }
        }

        private void btnUpFilenamePosition(object sender, RoutedEventArgs e)
        {
            int index = (FileNameListView.Items.IndexOf(FileNameListView.SelectedItem));
            if (index > 0)
            {
                var temp = _listfilenames[index];
                _listfilenames.Insert(index - 1, temp);
                _listfilenames.RemoveAt(index + 1);
                FileNameListView.SelectedItems.Add(_listfilenames[index - 1]);
            }
        }

        private void btnUpMostFilenamePosition(object sender, RoutedEventArgs e)
        {
            int index = (FileNameListView.Items.IndexOf(FileNameListView.SelectedItem));
            if (index > 0)
            {
                _listfilenames.Insert(0, _listfilenames[index]);
                _listfilenames.RemoveAt(index + 1);
                FileNameListView.SelectedItems.Add(_listfilenames[0]);
            }
        }

        private void btnDownFilenamePosition(object sender, RoutedEventArgs e)
        {
            int index = (FileNameListView.Items.IndexOf(FileNameListView.SelectedItem));
            if (index != (_listfilenames.Count - 1) && index >= 0)
            {
                var temp = _listfilenames[index + 1];
                _listfilenames.Insert(index, temp);
                _listfilenames.RemoveAt(index + 2);
            }
        }

        private void btnDownMostFilenamePosition(object sender, RoutedEventArgs e)
        {
            int index = (FileNameListView.Items.IndexOf(FileNameListView.SelectedItem));
            int numElement = _listfilenames.Count;
            if (index < numElement - 1 && index >= 0)
            {
                _listfilenames.Insert(numElement, _listfilenames[index]);
                _listfilenames.RemoveAt(index);
                //operationListBox.SelectedItems.Add(_actions[numElement - 1]);
                FileNameListView.SelectedItems.Add(_listfilenames[numElement - 1]);
            }
        }

        private void CheckboxApplyMethod_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if(checkBox.IsChecked == true)
            {
                int index = operationListBox.Items.IndexOf(operationListBox.SelectedItem);
                _listapplyactions.Add(_actions[index]);
            }
            if(checkBox.IsChecked == false)
            {
                int index = operationListBox.Items.IndexOf(operationListBox.SelectedItem);
                _listapplyactions.Remove(_actions[index]);
            }
        }

        private void btnSavePreset(object sender, RoutedEventArgs e)
        {
            if (_listapplyactions.Count() != 0)
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory;
                string name = "Preset";
                string folderPresetName = path + name;
                string presetName = "";
                

                var screen = new PresetControl();
                if (screen.ShowDialog() == true)
                {
                    presetName = "preset_" + PresetControl.presetName;


                    //Create file
                    string filename = folderPresetName + "//" + presetName;
                    var presetfile = File.Create(filename);
                    presetfile.Close();


                    List<string> data = new List<string>();
                    int i = 0;
                    foreach (var action in _listapplyactions)
                    {
                        data.Add(action.Name);
                       
                    }
                    File.WriteAllLines(filename, data);
                }

            }


        }
    }
}
