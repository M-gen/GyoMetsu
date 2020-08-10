using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;
using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;

namespace GyoMetsu.UI
{
    public class CharacterCard : ISprite
    {
        protected Vector2D position;
        protected Vector2D size;

        protected LayerSprite layer = new LayerSprite();
        public bool isMouseOn = false;         // マウスカーソルが乗っているかどうか
        public bool isMouseOnUITarget = false; // マウスカーソルが乗っていて、マウスクリックなどの操作の対象として扱われているか

        public double damageTimer = -1;
        protected double damageTimerMax = 30;

        public Data.Character character;

        public CharacterCard()
        {
        }

        virtual public void RefreshViewHP() { }

        virtual public void ShowDamage( int value ) { }

        virtual public void ShowMiss() { }

        virtual public void ShowHealing( int value ) { }

        virtual public void AddElement(Data.Element element) { }
        virtual public void RemoveElement(int index) { }
        virtual public void RemoveElementAll() { }

        virtual public void ActionEffect( Action action, Data.ActionSkill actionSkill ) { }
    }
}
