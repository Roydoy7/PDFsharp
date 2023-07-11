using PdfSharp.Pdf.Internal;
using PdfSharp.Pdf.IO;
using System.Text;

namespace PdfSharp.Pdf.StreamContent
{
    public class PdfStreamParser
    {
        byte[] _data;
        int _idxChar;
        int _dataLen;
        char _curChar;
        char _nextChar;

        readonly Dictionary<string, string> _textState = new();
        readonly Dictionary<string, string> _generalState = new();

        readonly Queue<string> _tokens = new();

        readonly StringBuilder _token = new();
        readonly StringBuilder _contents = new();
        readonly List<PdfStreamItem> _results = new();
        readonly Stack<PdfStreamContainer> _containers = new();

        /// <summary>
        /// Parse stream marked content into objects.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<PdfStreamItem> Parse(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            _data = data;
            _idxChar = 0;
            _dataLen = data.Length;
            _results.Clear();
            _textState.Clear();
            ParseToObjects();
            return _results;
        }

        /// <summary>
        /// Parse stream marked content into objects.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<PdfStreamItem> Parse(string data)
        {
            return Parse(PdfEncoders.RawEncoding.GetBytes(data));
        }

        void ParseToObjects()
        {
            while (_idxChar < _dataLen)
            {
                MoveUntilWhiteSpace();
                var str = _token.ToString();

                switch (str)
                {
                    case "":
                        break;
                    //General graphics state
                    case "w":
                    case "J":
                    case "j":
                    case "M":
                    case "d":
                    case "ri":
                    case "i":
                    case "gs":
                        {
                            FlushToContents(str);
                            var contents = _contents.ToString();
                            if (_generalState.ContainsKey(str))
                            {
                                var val = _generalState[str];
                                if (val != contents)
                                {
                                    CreateAndAdd(CreateGraphicState);
                                    _generalState[str] = contents;
                                }
                                else
                                    _contents.Clear();
                            }
                            else
                            {
                                CreateAndAdd(CreateGraphicState);
                                _generalState.Add(str, contents);
                            }
                            break;
                        }
                    //Color
                    case "CS":
                    case "cs":
                    case "SC":
                    case "SCN":
                    case "Sc":
                    case "Scn":
                    case "G":
                    case "g":
                    case "RG":
                    case "rg":
                    case "K":
                    case "k":
                        {
                            FlushToContents(str);
                            CreateAndAdd(CreateGraphicState);
                            break;
                        }
                    //Special graphics state
                    case "q"://Save current graphic state
                        {
                            _generalState.Clear();
                            _textState.Clear();

                            FlushToContents(str);
                            CreateAndAdd(CreateGraphicState);
                            break;
                        }
                    case "Q"://Restore graphic state
                    case "cm"://Modify current transformation matrix                    
                    //Type 3 fonts
                    case "d0":
                    case "d1":
                        {
                            FlushToContents(str);
                            CreateAndAdd(CreateGraphicState);
                            break;
                        }
                    //Text state
                    case "Tc":
                    case "Tw":
                    case "Tz":
                    case "TL":
                    case "Tf":
                    case "Tr":
                    case "Ts":
                    //Text position
                    case "Td":
                    case "TD":
                    case "Tm":
                    case "T*":
                    //Test showing
                    case "Tj":
                    case "TJ":
                    case "'":
                    case "\"":
                        {
                            ModifyTextStateDict(str);
                            break;
                        }
                    //Path painting
                    case "m"://Move
                    case "l"://Line to
                    case "c":
                    case "v":
                    case "y":
                    case "h":
                    case "re":
                    //Clip path
                    case "W"://Clip path
                    case "W*":
                        {
                            FlushToContents(str);
                            break;
                        }
                    //Path painting
                    case "S"://Stroke path
                    case "s":
                    case "f":
                    case "F":
                    case "f*":
                    case "B":
                    case "B*":
                    case "b":
                    case "b*":
                    case "n":
                        {
                            FlushToContents(str);
                            CreateAndAdd(CreatePath);
                            break;
                        }
                    case "BDC":
                        {
                            if (_tokens.Count > 0)
                            {
                                var name = _tokens.Dequeue();
                                if (_tokens.Count > 0)
                                {
                                    var propertyList = _tokens.Dequeue();
                                    var container = new PdfStreamContainer
                                    {
                                        Name = name,
                                        PropertyList = propertyList,
                                    };
                                    _containers.Push(container);
                                }
                            }
                            break;
                        }
                    case "BMC":
                        {
                            if (_tokens.Count > 0)
                            {
                                var name = _tokens.Dequeue();
                                var container = new PdfStreamContainer
                                {
                                    Name = name,
                                };
                                _containers.Push(container);
                            }
                            break;
                        }
                    case "EMC":
                        {
                            if (_containers.Count > 0)
                            {
                                var container = _containers.Pop();
                                _results.Add(container);
                            }
                            break;
                        }
                    //Text start
                    case "BT":
                        {
                            break;
                        }
                    //Text end
                    case "ET":
                        {
                            CreateAndAdd(CreateText);
                            break;
                        }
                    //XObject
                    case "Do":
                        {
                            FlushToContents(str);
                            CreateAndAdd(CreateXObject);
                            break;
                        }

                    default:
                        _tokens.Enqueue(str);
                        break;
                }
                ClearToken();
            }
        }

        /// <summary>
        /// Convert stream objects to bytes.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public byte[] GetBytes(IEnumerable<PdfStreamItem> items)
        {
            return GetString(items).ToArray().Select(x => (byte)x).ToArray();
        }

        /// <summary>
        /// Convert stream objects to string.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GetString(IEnumerable<PdfStreamItem> items)
        {
            if (!items.Any())
                throw new ArgumentException("Input collection is empty.");

            var sb = new StringBuilder();
            foreach (var item in items)
            {
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }

        void ModifyTextStateDict(string key)
        {
            var val = string.Empty;
            while (_tokens.Count > 0)
            {
                val += _tokens.Dequeue() + " ";
            }
            if (_textState.ContainsKey(key))
                _textState[key] = val;
            else
                _textState.Add(key, val);
        }

        void FlushToContents(string str)
        {
            while (_tokens.Count > 0)
            {
                var token = _tokens.Dequeue();
                _contents.Append(token + " ");
            }
            if (!string.IsNullOrEmpty(str))
                _contents.Append(str + '\n');
        }

        void CreateAndAdd(Func<PdfStreamItem> action)
        {
            if (_containers.Count > 0)
            {
                var container = _containers.Peek();
                container.Items.Add(action());
            }
            else
                _results.Add(action());
        }

        PdfStreamText CreateText()
        {
            var text = new PdfStreamText
            {
                TextState = new Dictionary<string, string>(_textState)
            };
            var ts = new string[] { "Tj", "TJ", "'", "\"" };
            foreach (var t in ts)
            {
                if (_textState.ContainsKey(t))
                    _textState.Remove(t);
            }
            _contents.Clear();
            return text;
        }

        PdfStreamPath CreatePath()
        {
            var path = new PdfStreamPath { Content = _contents.ToString() };
            _contents.Clear();
            return path;
        }

        PdfStreamGraphicState CreateGraphicState()
        {
            var gs = new PdfStreamGraphicState { Content = _contents.ToString() };
            _contents.Clear();
            return gs;
        }

        PdfStreamXObject CreateXObject()
        {
            var obj = new PdfStreamXObject { Content = _contents.ToString() };
            _contents.Clear();
            return obj;
        }

        int _forward;
        void MoveUntilWhiteSpace()
        {
            ScanNextChar(true);
            while (_curChar != Chars.EOF)
            {
                switch (_curChar)
                {
                    case Chars.NUL:
                    case Chars.HT:
                    case Chars.LF:
                    case Chars.FF:
                    case Chars.CR:
                        return;
                    case Chars.SP:
                        {
                            if (_forward > 0)
                            {
                                _token.Append(_curChar);
                                ScanNextChar(true);
                                break;
                            }
                            else
                                return;
                        }
                    case '<':
                    case '[':
                        {
                            _forward++;
                            _token.Append(_curChar);
                            ScanNextChar(true);
                            break;
                        }
                    case '>':
                    case ']':
                        {
                            _forward--;
                            _token.Append(_curChar);
                            ScanNextChar(true);
                            break;
                        }


                    case (char)11:
                    case (char)173:
                        ScanNextChar(true);
                        break;

                    default:
                        _token.Append(_curChar);
                        ScanNextChar(true);
                        break;
                }
            }
        }

        internal char ScanNextChar(bool handleCRLF)
        {
            if (_dataLen <= _idxChar)
            {
                _curChar = Chars.EOF;
                _nextChar = Chars.EOF;
            }
            else
            {
                _curChar = _nextChar;
                _nextChar = (char)_data[_idxChar];
                _idxChar++;
                if (handleCRLF && _curChar == Chars.CR)
                {
                    if (_nextChar == Chars.LF)
                    {
                        // Treat CR LF as LF.
                        _curChar = _nextChar;
                        _nextChar = (char)_data[_idxChar];
                        _idxChar++;
                    }
                    else
                    {
                        // Treat single CR as LF.
                        _curChar = Chars.LF;
                    }
                }
            }
            return _curChar;
        }

        /// <summary>
        /// Resets the current token to the empty string.
        /// </summary>
        void ClearToken() => _token.Clear();
    }
}
