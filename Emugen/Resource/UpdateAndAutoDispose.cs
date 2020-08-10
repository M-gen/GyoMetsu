using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emugen.Resource
{
    // 自動でDisposeを定期的に呼び出す

    public class UpdateAndAutoDispose
    {
        bool isEnd = false;
        //public Action updateAction;

        AutoDisposer autoDisposer;

        protected void SetupUpdateAndAutoDispose(  AutoDisposer autoDisposer)
        {
            this.autoDisposer = autoDisposer;
            this.autoDisposer.Add(this);
        }

        public virtual void Update() { }

        public void WaitDispose()
        {
            isEnd = true;
            autoDisposer.__WaitDispose(this);
        }

    }

    // Todo : 別ファイルに分離
    public class AutoDisposer
    {
        object itemsLock = new object();
        List<UpdateAndAutoDispose> items = new List<UpdateAndAutoDispose>();
        List<UpdateAndAutoDispose> disposeWaitItems = new List<UpdateAndAutoDispose>(); // 削除待ち
        object disposeWaitItemsLock = new object();


        public AutoDisposer()
        {
        }

        public void Add(UpdateAndAutoDispose item)
        {
            lock (itemsLock)
            {
                items.Add(item);
            }
        }

        public void Update()
        {
            lock (itemsLock)
            {

                try
                {
                    foreach (var t in items)
                    {
                        t.Update();
                    }
                }
                catch(System.InvalidOperationException exp)
                {
                    // todo : err : System.InvalidOperationException
                    //              HResult = 0x80131509
                    //Message = コレクションが変更されました。列挙操作は実行されない可能性があります。
                    //Source = mscorlib
                }
            }

            lock (disposeWaitItemsLock)
            {
                foreach( var t in disposeWaitItems)
                {
                    var u = t as IDisposable;
                    u?.Dispose();

                    lock (itemsLock)
                    {
                        items.Remove(t);
                    }
                }
                disposeWaitItems.Clear();
            }
        }

        
        public void __WaitDispose(UpdateAndAutoDispose t)
        {
            lock (disposeWaitItemsLock)
            {
                disposeWaitItems.Add(t);
            }
        }
    }

}
