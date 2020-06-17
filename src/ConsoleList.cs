using System;

namespace Topdev.Sublee.Cli
{
    public class ConsoleList
    {
        private int _index = 0;
        private int _cursor = 0;
        private bool _isFixed = false;
        private int _rowIndex = 0;
        private readonly bool _overflows = false;
        private readonly string _message;
        private readonly string[] _data;
        private readonly int _fixPosition;
        private const ConsoleColor _selectionColor = ConsoleColor.DarkCyan;
        private const string _selectionMark = ">";
        private const int _maxRows = 9;

        public ConsoleList(string message, string[] data)
        {
            _fixPosition = (_maxRows / 2);
            _message = message;
            _data = data;
            _overflows = (_data.Length > _maxRows);

            Write(true);
        }

        public int ReadResult()
        {
            while (true)
            {
                var key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        MoveDown();
                        Write();
                        break;
                    case ConsoleKey.UpArrow:
                        MoveUp();
                        Write();
                        break;
                    case ConsoleKey.Enter:
                        Console.CursorVisible = true;
                        return _index;
                }
            }
        }

        private void MoveUp()
        {
            if (_index == 0)
            {
                _index = _data.Length - 1;
            }
            else
            {
                _index--;
            }

            if (_cursor == 0 && !_isFixed)
                _rowIndex = _index;

            if (_cursor == 0)
            {
                _cursor = 0;
            }
            else if (_cursor == _fixPosition)
            {
                _isFixed = true;
            }
            else
            {
                _cursor--;
            }
        }

        private void MoveDown()
        {
            if (_index == (_data.Length - 1))
            {
                _index = 0;
            }
            else
            {
                _index++;
            }

            if (_cursor == _fixPosition)
            {
                _isFixed = true;
            }
            else
            {
                _cursor++;
            }
        }

        private void WriteList()
        {
            for (var i = 0; i < _data.Length; i++)
            {
                WriteRow(_data[i], _index == i);
            }
        }

        private void WriteOverflowingList()
        {

            int index;
            if (_isFixed)
            {
                index = _index - _fixPosition;
                if (index < 0)
                {
                    index = _data.Length + index;
                }
            }
            else
            {
                index = _rowIndex;
            }

            for (var i = 0; i < _maxRows; i++)
            {
                WriteRow(_data[index], _cursor == i);

                if (index == _data.Length - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
            }
        }

        private void WriteRow(string value, bool isSelected)
        {
            value = value.Ellipsis(Console.WindowWidth - 3);

            if (isSelected)
            {
                Console.ForegroundColor = _selectionColor;
                Console.Write(_selectionMark);
                Console.Write(" ");
                Console.Write(value);
                Console.ResetColor();
            }
            else
            {
                Console.Write("  ");
                Console.Write(value);
            }

            int spaces = Console.WindowWidth - value.Length - 3;

            if (spaces > 0)
                Console.Write(new string(' ', spaces));
            
            Console.Write(Environment.NewLine);
        }

        private void Write(bool init = false)
        {
            Console.CursorVisible = false;

            if (_overflows)
            {
                if (!init)
                    Console.SetCursorPosition(0, Console.CursorTop - (_maxRows + 1));

                WriteMessage();
                WriteOverflowingList();
                WriteFooter();
            }
            else
            {
                if (!init)
                    Console.SetCursorPosition(0, Console.CursorTop - (_data.Length + 1));

                WriteMessage();
                WriteList();
            }


        }

        private void WriteMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("? ");
            Console.ResetColor();
            Console.WriteLine(_message + ":");
        }

        private void WriteFooter()
        {
            Console.Write("(Move up and down to reveal more choices)");
        }
    }
}