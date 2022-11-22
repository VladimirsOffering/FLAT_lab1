using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace l1
{
    public class ViewModelMain : ViewModelBase
    {
        LexicalAnalyzer analyzer;

        string data;
        public string Data
        {
            get => data;
            set
            {
                data = value;
                OnPropertyChanged("Data");
                try
                {
                    analyzer.Analyze(value);
                    Result = "Ошибок нет";
                }
                catch (LexicalException e)
                {
                    Result = e.Message;
                }
            }
        }

        string result;

        public string Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged("Result");
            }
        }

        public ViewModelMain()
        {
            analyzer = new LexicalAnalyzer();
        }
    }
}
