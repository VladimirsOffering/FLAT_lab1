using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace l1
{
    public class Symbol
    {
        char value;
        public char Value
        {
            get => value;
            set
            {
                this.value = value;
                DefineType();
            }
        }
        public SymbolTypes Type { get; private set; }


        public Symbol(char c)
        {
            Value = c;
        }

        public Symbol()
        {
            Value = ' ';
        }

        private void DefineType()
        {
            if (Char.IsDigit(Value)) Type = SymbolTypes.Digit;
            else if (Char.IsLetter(Value)) Type = SymbolTypes.Letter;
            else if (Value == ';') Type = SymbolTypes.Reserved;
            else if (Value == '\r') Type = SymbolTypes.GoToStartLine;
            else if (Value == '\n') Type = SymbolTypes.NewLine;
            else if (Char.IsWhiteSpace(Value)) Type = SymbolTypes.WhiteSpace;
            else if (Value == '\0') Type = SymbolTypes.EndOfText;
            else Type = SymbolTypes.Other;
        }

        public static implicit operator Symbol(char value)
        {
            return new Symbol(value);
        }

    }
}
