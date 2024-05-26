using System;
using System.Collections.Generic;
using System.Linq;
//using System.Numerics;
using System.Text;
using OpenTK.Mathematics;

using System.Threading.Tasks;
using System.Data;

namespace StardomEngine.Resonance.Controls
{
    public class IDateSelector : IControl
    {
        public ITextBox DaySelector
        {
            get;
            set;
        }

        public ITextBox MonthSelector
        {
            get;
            set;
        }

        public ITextBox YearSelector
        {
            get;
            set;
        }

        public IList DayList
        {
            get;
            set;
        }

        public IList MonthList
        {
            get;
            set;
        }

        public IList YearList
        {
            get;
            set;
        }

        public int Day
        {
            get;
            set;
        }

        public int Month
        {
            get;
            set;
        }

        public int Year
        {
            get;
            set;
        }

        public IDateSelector(string reason)
        {

            var lab = new ILabel(reason).Set(new OpenTK.Mathematics.Vector2(0, 10), Vector2.Zero, reason) as ILabel;
            DaySelector = new ITextBox().Set(new OpenTK.Mathematics.Vector2(GameUI.This.TextWidth(reason)+5, 0), new OpenTK.Mathematics.Vector2(72, 30), "1") as ITextBox;
            MonthSelector = new ITextBox().Set(new Vector2(DaySelector.Position.X + DaySelector.Size.X + 5,0), new Vector2(82, 30), "Jan") as ITextBox;
            YearSelector = new ITextBox().Set(new Vector2(MonthSelector.Position.X + MonthSelector.Size.X + 5, 0), new Vector2(82, 30), "2024") as ITextBox;
            AddControls(lab, DaySelector, MonthSelector, YearSelector);
            DaySelector.Selector = true;
            MonthSelector.Selector = true;
            YearSelector.Selector = true;

            DateTime dateTime = DateTime.Now;

            Day = dateTime.Day;
            Month = dateTime.Month;
            Year = dateTime.Year;

            SetTime(Day, Month, Year);

            DaySelector.OnBoxSelected = (c) =>
            {
                if (DayList == null)
                {
                    DayList = new IList().Set(new Vector2(DaySelector.Position.X, DaySelector.Position.Y + 30), new Vector2(DaySelector.Size.X, 150), "") as IList;
                    AddControl(DayList);
                    for (int i = 1; i < 32;i++)
                    {
                        var it = DayList.AddItem(i.ToString());
                        it.Action += It_Action;
                    }
                   
                }
            };

            MonthSelector.OnBoxSelected = (c) =>
            {
                if (MonthList == null)
                {
                    MonthList = new IList().Set(new Vector2(MonthSelector.Position.X, MonthSelector.Position.Y + 30), new Vector2(MonthSelector.Size.X, 150), "") as IList;
                    AddControl(MonthList);
                    for (int i = 1; i < 13; i++)
                    {
                        var month = MonthList.AddItem(i.ToString());
                        month.Action += Month_Action;
                    }

                }
            };

            YearSelector.OnBoxSelected = (c) =>
            {
                if (YearList == null)
                {
                    YearList = new IList().Set(new Vector2(YearSelector.Position.X, YearSelector.Position.Y + 30), new Vector2(YearSelector.Size.X, 150), "") as IList;
                    AddControl(YearList);
                    for (int i = 1900; i < 2200; i++)
                    {
                        var year = YearList.AddItem(i.ToString());
                        year.Action += Year_Action;
                    }

                }
            };

        }

        private void Year_Action(ListItem item, int index, object data)
        {
            //throw new NotImplementedException();
            YearSelector.Text = item.Name.ToString();
            RemoveControl(YearList);
            YearList = null;
        }

        private void Month_Action(ListItem item, int index, object data)
        {
            MonthSelector.Text = item.Name.ToString();
            RemoveControl(MonthList);
            MonthList = null;
            //throw new NotImplementedException();
        }

        private void It_Action(ListItem item, int index, object data)
        {
            DaySelector.Text = item.Name.ToString();
            RemoveControl(DayList);
            DayList = null;
            //throw new NotImplementedException();
        }

        public void SetTime(int day,int month,int year)
        {
            Day = day;
            Month = month;
            Year = year;
            DaySelector.Text = day.ToString();
            MonthSelector.Text = month.ToString();
            YearSelector.Text = year.ToString();
            
        }

    }
}
