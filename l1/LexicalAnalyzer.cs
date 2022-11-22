using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace l1
{
    public enum SymbolTypes
    {
        Digit,
        Letter,
        Reserved,
        WhiteSpace,
        Other,
        GoToStartLine,
        NewLine,
        EndOfText
    }


    class LexicalAnalyzer
    {
        public string Data { get;private set; }

        public Symbol CurrentSymbol { get;private set; }
        public int CurrentIndex { get; private set; }

        public int CurrentRow { get; private set; }
        public int CurrentColumn { get; private set; }


        public LexicalAnalyzer()
        {
            CurrentSymbol = new Symbol();
            CurrentIndex = 0;
            CurrentRow = 1;
            CurrentColumn = 0;
        }

        /// <summary>
        /// Анализ введенной строки
        /// </summary>
        /// <param name="Data"></param>
        public void Analyze(string Data)
        {
            CurrentIndex = 0;
            CurrentRow = 1;
            CurrentColumn = 0;
            this.Data = Data;
          
            while (CurrentSymbol.Type!= SymbolTypes.EndOfText || CurrentIndex<Data.Length)          //Пока не закончился текст
            {
                ReadNext();                             //Считать след.символ
                switch (CurrentSymbol.Type)
                {
                    case SymbolTypes.Digit:
                        DKAFirstWord();
                        if (CurrentSymbol.Type != SymbolTypes.EndOfText) CurrentIndex -= 1;         
                        break;
                    case SymbolTypes.Letter:
                        DKASecondWord();
                        if (CurrentSymbol.Type != SymbolTypes.EndOfText) CurrentIndex -= 1;
                        break;
                    case SymbolTypes.Reserved:
                        SkipComment();
                        break;
                    case SymbolTypes.WhiteSpace:
                        break;
                    case SymbolTypes.Other:
                        throw new LexicalException(LexicalException.Types.UndefinedSymbol, CurrentRow, CurrentColumn);
                    case SymbolTypes.NewLine:
                        AddRow();
                        break;
                    default:
                        break;
                }

            }
        }

        private void DKAFirstWord()
        {
            goto S;
            S:
                if (CurrentSymbol.Value == '1')
                {
                    ReadNext();
                    goto A;
                }
            throw new LexicalException(LexicalException.Types.FirstWordError, CurrentRow, CurrentColumn);
            A:
                if (CurrentSymbol.Value =='1')
                {
                    ReadNext();
                    goto B;
                }
            throw new LexicalException(LexicalException.Types.FirstWordError, CurrentRow, CurrentColumn);
            B:
                if(CurrentSymbol.Value =='0')
                {
                    ReadNext();
                    goto S;
                }
                if (CurrentSymbol.Value == '1')
                {
                    ReadNext();
                    goto C;
                }
            throw new LexicalException(LexicalException.Types.FirstWordError, CurrentRow, CurrentColumn);
            C:
                if (CurrentSymbol.Value == '0')
                {
                    ReadNext();
                    goto D;
                }
                goto FIN;
            D:
                if (CurrentSymbol.Value == '0')
                {
                    ReadNext();
                    goto E;
                }
            throw new LexicalException(LexicalException.Types.FirstWordError, CurrentRow, CurrentColumn);
            E:
                if (CurrentSymbol.Value == '0')
                {
                    ReadNext();
                    goto C;
                }
            throw new LexicalException(LexicalException.Types.FirstWordError, CurrentRow, CurrentColumn);
            FIN:
            return;
        }

        private void DKASecondWord()
        {
            goto S;
            S:
            if (CurrentSymbol.Value == 'a' || CurrentSymbol.Value == 'b' || CurrentSymbol.Value == 'c')
            {
                ReadNext();
                goto A;
            }
            else if (CurrentSymbol.Value == 'd')
            {
                ReadNext();
                goto C;
            }
            throw new LexicalException(LexicalException.Types.SecondWordError, CurrentRow, CurrentColumn);
            A:
            if (CurrentSymbol.Value == 'a' || CurrentSymbol.Value == 'b' || CurrentSymbol.Value == 'c')
            {
                ReadNext();
                goto A;
            }
            if (CurrentSymbol.Value == 'd')
            {
                ReadNext();
                goto C;
            }
            goto FIN;

            C:
            if (CurrentSymbol.Value == 'a')
            {
                ReadNext();
                goto S;
            }
            if (CurrentSymbol.Value == 'b' || CurrentSymbol.Value == 'c')
            {
                ReadNext();
                goto A;
            }
            if (CurrentSymbol.Value == 'd')
            {
                ReadNext();
                goto C;
            }
            goto FIN;

            FIN:
            return;


        }


        private void ReadNext()
        {
            if (CurrentIndex == Data.Length)
            {
                CurrentSymbol = '\0';
                return;
            }
            CurrentSymbol = Data[CurrentIndex];
            CurrentColumn++;
            CurrentIndex++;
        }

        private void AddRow()
        {
            CurrentColumn = 0;
            CurrentRow++;
        }

        private void SkipComment()
        {
            while (CurrentSymbol.Type != SymbolTypes.NewLine && CurrentSymbol.Type!=SymbolTypes.EndOfText)
            {
                ReadNext();
            }
            AddRow();
        }


    }
    public class LexicalException : Exception
    {
        public enum Types
        {
            FirstWordError,
            SecondWordError,
            UndefinedSymbol
        }

        public string Message { get; private set; }

        public LexicalException(Types ErrorType, int row, int column)
        {
            switch (ErrorType)
            {
                case Types.FirstWordError:
                    Message = $"Ошибка в первом слове, строка {row} , столбец {column}";
                    break;
                case Types.SecondWordError:
                    Message = $"Ошибка во втором слове , строка {row} , столбец {column}";
                    break;
                case Types.UndefinedSymbol:
                    Message = $"Неизвестный символ, строка {row} , столбец {column}";
                    break;
                default:
                    break;
            }
        }
    }
}
