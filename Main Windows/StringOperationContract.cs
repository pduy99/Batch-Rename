using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using PropertyChanged;
using System.Windows;

namespace Main_Windows
{
    public class StringArgs
    {

    }

    public class ReplaceArgs : StringArgs, INotifyPropertyChanged
    {
        public string From { get; set; }
        public string To { get; set; }
        //1: Name Area, 2: Extension Area
        public int Area { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class MoveArgs : StringArgs
    {
        public bool FrontToEnd { get; set; }
    }

    public class NewCaseArgs : StringArgs
    {
        public int style { get; set; } //1: All Uppercase, 2: All lowercase, 3: UpperCase the first character each word

    }

    public class RemovePatternArgs : StringArgs
    {
        public string pattern { get; set; }
    }

    public class TrimArgs : StringArgs
    {

    }

    public class FullnameNormalizeArgs : StringArgs
    {

    }

    public class UniqueNameArgs : StringArgs
    {
    }



    public abstract class StringMethod:INotifyPropertyChanged
    {
        public StringArgs Args { get; set; }
        public virtual string Name { get; set; }
        public virtual string Error { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract string Operate(string origin, bool isFileName);
        public abstract StringMethod Clone();
        public abstract void Config();
        public virtual string Description { get; set; }

    }

    public class ReplaceMethod : StringMethod, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public override string Name => "Replace";

        public override string Description
        {
            get
            {
                var args = Args as ReplaceArgs;
                string area = (args.Area == 1)? "Name" : "Extension";
                return $"Replace  '{args.From}' to '{args.To}' in {area} ";
            }
        }

        public override StringMethod Clone()
        {
            var oldArgs = Args as ReplaceArgs;
            return new ReplaceMethod()
            {
                Args = new ReplaceArgs()
                {
                    From = oldArgs.From,
                    To = oldArgs.To,
                    Area = oldArgs.Area,
                }
            };
        }

        public override string Operate(string origin, bool isFileName)
        {
            var args = Args as ReplaceArgs;
            var from = args.From;
            var to = args.To;
            var area = args.Area;

            
            if (isFileName)
            {
                string name = Path.GetFileNameWithoutExtension(origin);
                string extension = Path.GetExtension(origin);
                if (area == 1)
                {
                    name = name.Replace(from, to);
                }
                else
                {
                    extension = extension.Replace(from, to);
                }
                return (name + extension);
            }
            return origin.Replace(from, to);
            
        }

        public override void Config()
        {
            var screen = new ReplaceMethodControl(Args);
            if(screen.ShowDialog() == true)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description"));
            }
        }
    }

    public class MoveMethod : StringMethod, INotifyPropertyChanged
    {
        public override string Name => "Move";

        public event PropertyChangedEventHandler PropertyChanged;

        public override StringMethod Clone()
        {
            var oldArgs = Args as MoveArgs;
            return new MoveMethod()
            {
                Args = new MoveArgs()
                {
                    FrontToEnd = oldArgs.FrontToEnd,
                }
            };
        }

        public override void Config()
        {
            var screen = new MoveMethodControl(Args);
            if(screen.ShowDialog() == true)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description"));
            }
        }

        public override string Operate(string origin, bool isFileName)
        {
            var args = Args as MoveArgs;
            if (!isFileName)
            {
                if (args.FrontToEnd == true)
                {
                    return origin.Substring(13) + origin.Substring(0, 13);
                }
                else
                {
                    if (origin.Length >= 13)
                    {
                        return origin.Substring(origin.Length - 13) + origin.Substring(0, origin.Length - 13);
                    }
                    else
                    {
                        throw new IndexOutOfRangeException("The file name lenght is less than 13");
                    }
                }
            }
            else
            {
                string name = Path.GetFileNameWithoutExtension(origin);
                string extension = Path.GetExtension(origin);
                if (args.FrontToEnd == true)
                {
                    return (name.Substring(13) + name.Substring(0, 13) + extension);
                    
                }
                else
                {
                    if (name.Length >= 13)
                    {
                        return (name.Substring(name.Length - 13) + name.Substring(0, name.Length - 13) + extension);
                    }
                    else
                    {
                        throw new IndexOutOfRangeException("The file name lenght is less than 13"); 
                    }
                }
            }
        }

        public override string Description
        {
            get
            {
                var args = Args as MoveArgs;
                if(args.FrontToEnd)
                {
                    return "Move 13 letters at front to end";
                }
                else
                {
                    return "Move 13 letter at end to front";
                }
            }
        }
    }

    public class NewCaseMethod:StringMethod, INotifyPropertyChanged
    {
        public override string Name => "New Case";

        public event PropertyChangedEventHandler PropertyChanged;

        public override StringMethod Clone()
        {
            var oldArgs = Args as NewCaseArgs;
            return new NewCaseMethod()
            {
                Args = new NewCaseArgs()
                {
                    style = oldArgs.style
                }
            };
        }

        public override void Config()
        {
            var screen = new NewCaseMethodControl(Args);
            if (screen.ShowDialog() == true)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description"));
            }
        }

        public override string Operate(string origin, bool isFileName)
        {
            var args = Args as NewCaseArgs;
            if (isFileName == true)
            {
                string name = Path.GetFileNameWithoutExtension(origin);
                string extension = Path.GetExtension(origin);
                if (args.style == 1)
                {
                    name = name.ToUpper();
                }
                else if (args.style == 2)
                {
                    name = name.ToLower();
                }
                else
                {
                    name = name.ToLower();
                    TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
                    name = cultInfo.ToTitleCase(name);

                }
                return (name + extension);
            }
            else
            {
                if (args.style == 1)
                {
                    return origin.ToUpper();
                }
                else if (args.style == 2)
                {
                    return origin.ToLower();
                }
                else
                {
                    origin = origin.ToLower();
                    TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
                    string res = cultInfo.ToTitleCase(origin);
                    return res;
                }
            }
           
        }

        public override string Description
        {
            get
            {
                var args = Args as NewCaseArgs;
                
                if(args.style == 1)
                {
                    return "Uppercase all letters";
                }
                if(args.style == 2)
                {
                    return "Lowercase all letters";
                }
                if(args.style == 3)
                {
                    return "Only Uppercase the first letter of each word";
                }
                return "error";                
            }
        }
    }

    public class RemovePatternMethod : StringMethod, INotifyPropertyChanged
    {
        public override string Name => "Remove Pattern";

        public event PropertyChangedEventHandler PropertyChanged;

        public override StringMethod Clone()
        {
            var oldArgs = Args as RemovePatternArgs;
            return new RemovePatternMethod()
            {
                Args = new RemovePatternArgs()
                {
                    pattern = oldArgs.pattern
                }
            };
        }

        public override void Config()
        {
            throw new NotImplementedException();
        }

        public override string Operate(string origin, bool isFileName)
        {
            var args = Args as RemovePatternArgs;
            return origin.Replace(args.pattern, "");
        }
    }

    public class TrimMethod : StringMethod,INotifyPropertyChanged
    {
        public override string Name => "Trim";

        public event PropertyChangedEventHandler PropertyChanged;

        public override StringMethod Clone()
        {
            var oldArgs = Args as TrimArgs;
            return new TrimMethod()
            {
                Args = new TrimArgs()
                {

                }
            };
        }

        public override void Config()
        {
            MessageBox.Show("Dont't need to config this method");
        }

        public override string Operate(string origin, bool isFileName)
        {
            return origin.Trim();
        }

        public override string Description
        {
            get
            {
                return "Removes all leading and trailing white-space characters";
            }
        }
    }

    public class FullnameNormalizeMethod : StringMethod, INotifyPropertyChanged
    {
        public override string Name => "Fullname Normalize";

        public event PropertyChangedEventHandler PropertyChanged;

        public override StringMethod Clone()
        {
            var oldArgs = Args as FullnameNormalizeArgs;
            return new FullnameNormalizeMethod()
            {
                Args = new FullnameNormalizeArgs()
                {

                }
            };
        }

        public override void Config()
        {
            MessageBox.Show("Don't need to config this method");
        }

        public override string Operate(string origin, bool isFileName)
        {
            if (isFileName)
            {
                string name = Path.GetFileNameWithoutExtension(origin);
                string extension = Path.GetExtension(origin);
                //Viet hoa cac chu cai dau cua moi tu
                name = name.ToLower();
                TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
                name = cultInfo.ToTitleCase(name);

                //Xoa bo cac khoang trang dau va cuoi
                name = name.Trim();

                //Xoa bo cac khoang trang thua
                name = System.Text.RegularExpressions.Regex.Replace(name, @"\s+", " ");

                return (name+extension);
            }
            else
            {
                origin = origin.ToLower();
                TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
                origin = cultInfo.ToTitleCase(origin);

                //Xoa bo cac khoang trang dau va cuoi
                origin = origin.Trim();

                //Xoa bo cac khoang trang thua
                origin = System.Text.RegularExpressions.Regex.Replace(origin, @"\s+", " ");

                return origin;
            }
        }

        public override string Description
        {
            get
            {
                return "Remove all extra white spaces and Uppercase the first letter of each word";
            }
        }
    }

    public class UniqueNameMethod : StringMethod, INotifyPropertyChanged
    {
        public override string Name => "Unique Name";

        public event PropertyChangedEventHandler PropertyChanged;

        public override StringMethod Clone()
        {
            var oldArgs = Args as UniqueNameArgs;
            return new UniqueNameMethod()
            {
                Args = new UniqueNameArgs()
                {

                }
            };
        }

        public override void Config()
        {
            MessageBox.Show("Don't need to config this method");
        }

        public override string Operate(string origin, bool isFileName)
        {
            if (!isFileName)
            {
                string res = System.Guid.NewGuid().ToString();
                return res;
            }
            else
            {
                string extension = Path.GetExtension(origin);
                string res = System.Guid.NewGuid().ToString();
                return res + extension;
            }

        }

        public override string Description
        {
            get
            {
                return "Change the name to GUID";
            }
        }
    }
}
