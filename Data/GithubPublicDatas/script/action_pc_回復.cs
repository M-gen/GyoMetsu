
TimeEvents.Add( new TimeEvent(0,  ()=>{ SetPosition(0,-0); }) );
TimeEvents.Add( new TimeEvent(1,  ()=>{ SetPosition(0,-1); }) );
TimeEvents.Add( new TimeEvent(2,  ()=>{ SetPosition(0,-2); }) );
TimeEvents.Add( new TimeEvent(3,  ()=>{ SetPosition(0,-3); }) );
TimeEvents.Add( new TimeEvent(4,  ()=>{ SetPosition(0,-10); }) );
TimeEvents.Add( new TimeEvent(5,  ()=>{ SetPosition(0,-25); }) );
TimeEvents.Add( new TimeEvent(6,  ()=>{ SetPosition(0,-35); }) );
TimeEvents.Add( new TimeEvent(7,  ()=>{ SetPosition(0,-43); }) );
TimeEvents.Add( new TimeEvent(8,  ()=>{ SetPosition(0,-48); }) );
TimeEvents.Add( new TimeEvent(9,  ()=>{ SetPosition(0,-49); }) );
TimeEvents.Add( new TimeEvent(10, ()=>{ SetPosition(0,-50); }) );

TimeEvents.Add( new TimeEvent(15, ()=>{ PlaySoundEffect("data/se/魔法 詠唱10(1秒)ドライs.mp3",0.5f); }) );

var a = 30;
TimeEvents.Add( new TimeEvent(30+a, ()=>{ PlaySoundEffect("data/se/魔法 回復14(1秒)s.mp3",0.5f); }) );
TimeEvents.Add( new TimeEvent(30+a, ()=>{ MainAction(); }) );

a += 15;

TimeEvents.Add( new TimeEvent(30+a, ()=>{ SetPosition(0,-40); }) );
TimeEvents.Add( new TimeEvent(30+a, ()=>{ SetPosition(0,-28); }) );
TimeEvents.Add( new TimeEvent(31+a, ()=>{ SetPosition(0,-14); }) );
TimeEvents.Add( new TimeEvent(32+a, ()=>{ SetPosition(0,-6); }) );
TimeEvents.Add( new TimeEvent(33+a, ()=>{ SetPosition(0,-0); }) );