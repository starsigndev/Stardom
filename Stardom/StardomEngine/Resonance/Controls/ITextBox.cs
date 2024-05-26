using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
namespace StardomEngine.Resonance.Controls
{
    public delegate void TextChanged(ITextBox control, string text);
    public class ITextBox : IControl
    {

        public TextChanged OnTextChanged
        {
            get;
            set;
        }

        bool ClaretOn = true;
        int StartX = 0;
        int NextClaret = 0;
        int DisLen = 0;
        public bool Numeric = false;
        bool Password = false;
        string GuideText = "";
        int ClaretX = 0;

        public ITextBox()
        {
            //_
        }

        string MaxString(string text)
        {

            string res = "";
            for(int i = StartX; i < text.Length; i++)
            {
                string nc = text.Substring(i, 1);
                int aw = (int)GameUI.This.TextWidth(res + nc);
                if(aw<Size.X)
                {
                    res = res + nc;
                }
                else
                {
                    return res;
                }
            }

            return res;

        }

        int GetClaretX()
        {

            int nx = ClaretX - StartX;
            var str = MaxString(_Text);
            string mstr = "";
            try
            {
                mstr = str.Substring(0, nx);
            }
            catch
            {
                try
                {
                    mstr = str.Substring(0, nx - 1);
                }
                catch
                {
                    mstr = str;
                }
            }
            return (int)GameUI.This.TextWidth(mstr);

        }

        float GetNumber()
        {
            return float.Parse(Text);
        }

        void SetNumber(float val)
        {
            Text = val.ToString();
        }

        string GetString(string text,int s,int e)
        {
            var str = text.Substring(s);
            str = MaxString(str);
            return str;
        }

        public override void OnActivate()
        {
            //base.OnActivate();
            ClaretOn = true;
            NextClaret = Environment.TickCount;
        }

        public override void OnDeactivate()
        {
            ClaretOn = false;
            
        }

        public override void Update()
        {
            //base.Update();
            int time = Environment.TickCount;
            if (time > NextClaret + 500)
            {
                NextClaret = time;
                ClaretOn = ClaretOn ? false : true;
            }
            int b = 5;
        }

        public override void Render()
        {
            //base.Render();
            GameUI.This.DrawRect(GameUI.Theme.TextBoxSlice,10,10, RenderPosition+new Vector2(-3,0), Size+new Vector2(6,0), new OpenTK.Mathematics.Vector4(1, 1, 1, 1));

            string display_str = MaxString(Text);
            DisLen = display_str.Length;

            int dy = (int)Size.Y / 2;
            dy = dy - ((int)GameUI.This.TextHeight(display_str) / 2);
            dy = dy +1;
            
            if (Text == "")
            {
                GameUI.This.DrawText(GuideText, RenderPosition + new OpenTK.Mathematics.Vector2(3, dy), new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
            }
            else
            {
                GameUI.This.DrawText(display_str, RenderPosition + new OpenTK.Mathematics.Vector2(3, dy), new OpenTK.Mathematics.Vector4(1, 1, 1, 1));

            }

            if (Active)
            {
                int cx = (int)RenderPosition.X + 3;

                if (ClaretOn)
                {
                    cx = GetClaretX();
                    GameUI.This.DrawRect(GameUI.Theme.Frame, new Vector2(RenderPosition.X + cx, RenderPosition.Y + 4), new Vector2(2, Size.Y - 8), new Vector4(1, 1, 1, 1));
                }

            }


        }

        public override void TextChanged()
        {
            //base.TextChanged();
            ClaretX = Text.Length;
        }

        public override void OnKey(Keys key)
        {
            //base.OnKey(key);
            Console.WriteLine("Key:" + key.ToString());
            switch (key)
            {
                case Keys.Backspace:
                    if (ClaretX > 0)
                    {

                        if (ClaretX == 1)
                        {
                            _Text = _Text.Substring(1);
                        }
                        else
                        {
                            _Text = _Text.Substring(0, ClaretX - 1) + _Text.Substring(ClaretX);
                        }
                        ClaretX--;
                        if (ClaretX <= StartX)
                        {
                            StartX = ClaretX - 5;
                            if (StartX < 0) StartX = 0;
                            if (ClaretX < 0) ClaretX = 0;
                        }
                    }
                    OnTextChanged?.Invoke(this, _Text);
                    //if (OnChanged)
                    {
                        //OnChanged(_Text);

                    }
                    return;
                    break;
                case Keys.Delete:

                    if (ClaretX < _Text.Length)
                    {

                        _Text = _Text.Substring(0, ClaretX) + _Text.Substring(ClaretX + 1);
                        //_Tex6t = _Text + 
                        OnTextChanged?.Invoke(this, _Text);

                        //if (OnChanged)
                        {
                          //  OnChanged(_Text);
                        }

                    }
                    return;
                    break;
                case Keys.Right:

                    int tc = ClaretX;
                    if (tc < _Text.Length)
                    {
                        ClaretX++;
                        int mf = ClaretX - StartX;
                        if (mf > MaxString(_Text).Length - 1)
                        {
                            StartX++;
                        }
                    }
                    return;
                    break;
                case Keys.Left:

                    if (ClaretX > 0)
                    {
                        ClaretX--;
                        if (ClaretX < StartX)
                        {
                            if (StartX > 0)
                            {
                                StartX--;
                            }
                        }
                    }
                    return;
                    break;

            }

            string k = GetChar(key);

            if (k.Length==0)
            {
                return;
            }

            if (ClaretX == _Text.Length)
            {
                _Text = _Text + k;
                ClaretX++;
            }else if (ClaretX == 0)
            {
                if (_Text.Length == 0)
                {
                    _Text = k;
                    ClaretX++;
                }
                else
                {
                    _Text = k + _Text;
                    ClaretX++;
                }
            }
            else
            {
                _Text = _Text.Substring(0, ClaretX) + k + _Text.Substring(ClaretX);
            }


            if (ClaretX > _Text.Length)
            {
                ClaretX = _Text.Length;
            }

            int ax = ClaretX - StartX;
            int tw = 0;
            for(int i = StartX; i < _Text.Length; i++)
            {
                tw += (int)GameUI.This.TextWidth(_Text.Substring(i, 1));
                if (tw > Size.X)
                {
                    StartX++;
                }
            }

            OnTextChanged?.Invoke(this, _Text);


        }

        public string GetChar(Keys key)
        {

            string ks = key.ToString();
            Console.WriteLine(ks);
            switch (key)
            {
                case Keys.Minus:
                    ks = "-";
                    break;
                case Keys.Equal:
                    ks = "=";
                    break;
                case Keys.LeftBracket:
                    ks = "[";
                    break;
                case Keys.RightBracket:
                    ks = "]";
                    break;
                case Keys.Apostrophe:
                    ks = "'";
                    break;
                case Keys.Comma:
                    ks = ",";
                    break;
                case Keys.Period:
                    ks = ".";
                    break;
                case Keys.Space:
                    ks = " ";
                    break;
                case Keys.D0:
                    ks = "0";
                    break;
                case Keys.D1:
                    ks = "1";
                    break;
                case Keys.D2:
                    ks = "2";
                    break;
                case Keys.D3:
                    ks = "3";
                    break;
                case Keys.D4:
                    ks = "4";
                    break;
                case Keys.D5:
                    ks = "5";
                    break;
                case Keys.D6:
                    ks = "6";
                    break;
                case Keys.D7:
                    ks = "7";
                    break;
                case Keys.D8:
                    ks = "8";
                    break;
                case Keys.D9:
                    ks = "9";
                    break;
                case Keys.Tab:
                    ks = "  ";
                    break;
                case Keys.Semicolon:
                    ks = ";";
                    break;
                case Keys.Backslash:
                    ks = "\\";
                    break;
                case Keys.Slash:
                    ks = "/";
                    break;

                default:
                    ks = key.ToString();
                    break;
            }

            if (ShiftOn)
            {
                switch (key)
                {
                    case Keys.Semicolon:
                        ks = ":";
                        break;
                    case Keys.Slash:
                        ks = "?";
                        break;
                    case Keys.Minus:
                        ks = "_";
                        break;
                    case Keys.Equal:
                        ks = "+";
                        break;
                    case Keys.LeftBracket:
                        ks = "{";
                        break;
                    case Keys.RightBracket:
                        ks = "}";
                        break;
                    case Keys.Backslash:
                        ks = "|";
                        break;
                    case Keys.Comma:
                        ks = "<";
                        break;
                    case Keys.Period:
                        ks = ">";
                        break;
                    case Keys.Apostrophe:
                        ks = "\"";
                        break;
                    case Keys.D0:
                        ks = ")";
                        break;
                    case Keys.D1:
                        ks = "!";
                        break;
                    case Keys.D2:
                        ks = "@";
                        break;
                    case Keys.D3:
                        ks = "#";
                        break;
                    case Keys.D4:
                        ks = "$";
                        break;
                    case Keys.D5:
                        ks = "%";
                        break;
                    case Keys.D6:
                        ks = "^";
                        break;
                    case Keys.D7:
                        ks = "&";
                        break;
                    case Keys.D8:
                        ks = "*";
                        break;
                    case Keys.D9:
                        ks = "(";
                        break;
                }
            }
            else
            {
                ks = ks.ToLower();
            }

            if (Numeric)
            {
                if ("0123456789.-".Contains(ks))
                {
                    return ks;
                }
                else
                {
                    return "";
                }
            }

            return ks;

        }

    }
}
