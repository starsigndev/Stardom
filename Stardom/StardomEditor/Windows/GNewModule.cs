using StardomEditor.States;
using StardomEngine.Resonance;
using StardomEngine.Resonance.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StardomEditor.Windows
{
    public class GNewModule : IWindow
    {
        private IHBox name_h, author_h, starsys_h;
        private IHBox accept_h;
        private IVBox vb;
        public GNewModule() : base(false,false)
        {


            
            /*
            vb = new IVBox();
            name_h = new IHBox();
            author_h = new IHBox();
            starsys_h = new IHBox();
            accept_h = new IHBox();

            Contents.AddControl(vb);
            vb.AddControls(name_h,author_h,starsys_h,accept_h);


            ILabel mod_name = new ILabel("Name");
            ITextBox modname_box = new ITextBox();

            name_h.AddControls(mod_name, modname_box);
            vb.Alignment = FillAlignment.Top;
            vb.AlignSize = 30;
            vb.AlignSpace = 10;

            ILabel mod_author = new ILabel("Author");
            ITextBox author_box = new ITextBox();
           
            author_h.AddControls(mod_author, author_box);
            

            mod_name.AlignSize = 150;
            mod_author.AlignSize = 150;

            ILabel sys_name = new ILabel("Star System");
            ITextBox sys_namebox = new ITextBox();

            sys_name.AlignSize = 150;

            starsys_h.AddControls(sys_name, sys_namebox);

            modname_box.TabNext = author_box;
            author_box.TabNext = sys_namebox;
            sys_namebox.TabNext = modname_box;

            IButton create = new IButton("Create");
            IButton cancel = new IButton("Cancel");

            accept_h.AddControls(create, cancel);

            create.OnClick = (c,b) =>
            {
                ProjectState.EditModule = new StardomEngine.GameMod.GameModule();
               ProjectState.EditModule.ModName = modname_box.Text;
                ProjectState.EditModule.ModAuthor = mod_author.Text;
                ProjectState.EditModule.ModStarSystem = starsys_h.Text;
                GameUI.This.Windows.Remove(this);
                MainEditorState.ModInfo.Text = "Mod:" + modname_box.Text;
                ProjectState.Save();

            };

            cancel.OnClick = (c, b) =>
            {
                GameUI.This.Windows.Remove(this);
            };
            */
        }

        public override void Build()
        {
            //base.Build();
            Builder.BeginVertical(FillAlignment.Top,10,35);
            Builder.BeginHorizontal(FillAlignment.Fill);
           var i1= Builder.TextBox("Mod",-1,105);
            Builder.End();
            Builder.BeginHorizontal(FillAlignment.Fill);
            var i2=Builder.TextBox("Author",-1,105);
            Builder.End();
            Builder.BeginHorizontal(FillAlignment.Fill);
            var i3=Builder.TextBox("System",-1,105);
            Builder.End();
            Builder.BeginHorizontal(FillAlignment.Fill,25);
            var create = Builder.Button("Create");
            var cancel = Builder.Button("Cancel");
            Builder.End();
            Builder.End();
            i1.TabNext = i2;
            i2.TabNext = i3;
            i3.TabNext = i1;

            create.OnClick = (b, m) =>
            {
                ProjectState.EditModule = new StardomEngine.GameMod.GameModule();
                ProjectState.EditModule.ModName = i1.Text;
                ProjectState.EditModule.ModAuthor = i2.Text;
                ProjectState.EditModule.ModStarSystem = i3.Text;
                GameUI.This.Windows.Remove(this);
                MainEditorState.ModInfo.Text = "Mod:" + i1.Text;
                ProjectState.Save();

            };

            cancel.OnClick = (b, m) =>
            {
                GameUI.This.Windows.Remove(this);
            };

            return;
           
        }
        public override void AfterSet()
        {
            base.AfterSet();
            //base.AfterSet();
            /*
           
            vb.Set(new OpenTK.Mathematics.Vector2(0, 0),Contents.Size, Text);
            vb.Calculate();
            name_h.Calculate();
            author_h.Calculate();
            starsys_h.Calculate();
            accept_h.Calculate();
            Contents.Offset = new OpenTK.Mathematics.Vector2(6, 0);
        */
        }


    }
}
