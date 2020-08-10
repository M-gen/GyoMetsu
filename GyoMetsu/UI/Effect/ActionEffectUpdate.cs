using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emugen.Image.Primitive;
using Emugen.OpenTK;
using Emugen.OpenTK.Sprite;
using Emugen.Resource;

namespace GyoMetsu.UI.Effect
{

    public class ActionEffectUpdate : UpdateAndAutoDispose, IDisposable
    {
        public Emugen.Script.Script<ScriptAPI> script;
        AutoDisposer autoDisposer;
        int timer = 0;
        int timerMax = 0;

        public ActionEffectUpdate(string scriptPath, Action action, AutoDisposer autoDisposer)
        {
            this.autoDisposer = autoDisposer;

            script = new Emugen.Script.Script<ScriptAPI>(scriptPath, new ScriptAPI());
            script.api.MainAction = action;
            SetupUpdateAndAutoDispose(autoDisposer);
            script.Run();

            foreach (var i in script.api.TimeEvents)
            {
                if (timerMax < i.Time)
                {
                    timerMax = i.Time;
                }
            }

        }

        public override void Update()
        {
            var nowTimer = timer;
            timer++;
            foreach (var i in script.api.TimeEvents)
            {
                if (i.Time == nowTimer)
                {
                    // Todo : i.Actionで捕捉できないエラー？で処理が停止していることがあるっぽい？　対象を自身を選択した行動をとったときに発生している
                     i.Action();
                }
            }

            if (timer > timerMax)
            {
                autoDisposer.__WaitDispose(this);
            }
        }

        public void Dispose()
        {
        }

        public class ScriptAPI
        {
            public class TimeEvent
            {
                public int Time;
                public Action Action;
                public TimeEvent(int time, Action action)
                {
                    this.Time = time;
                    this.Action = action;
                }
            }
            public List<TimeEvent> TimeEvents = new List<TimeEvent>();

            public Vector2D Position = new Vector2D(0, 0);
            public Action MainAction;
            
            public void SetPosition(double x, double y)
            {
                Position.X = x;
                Position.Y = y;
            }

            public void PlaySoundEffect( string path, float value )
            {
                new Emugen.Sound.SoundPlayer(path, value, false, Emugen.Sound.SoundPlayer.SoundType.SE);
            }


        }
    }
}
