using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using NAudio.Wave;
using NAudio.CoreAudioApi;

using Emugen.Thread;

namespace Emugen.Sound
{
    // ひとまずはなげっぱなしで、インスタンスを利用して制御や削除できるわけではない
    // Todo : BGMと効果音、音声などの種類分けと、BGMは1つの未再生にしたいが
    // Todo : BGMのクロスフェード終了と再生やりたい？
    // Todo : おなじ効果音(種類分けでも…最初は多くは望めないか)が、近い時間帯に多発している場合に抑制できるかどうか…音量の調整、倍々で音量が大きくなるので
    public class SoundPlayer : IDisposable
    {
        static volatile object soundPlayersLock = new object();
        static List<SoundPlayer> soundPlayers = new List<SoundPlayer>();
        static EasyLoopThread coreThread; // 管理用スレッド
        static public bool systemClose = false;


        AudioFileReader audioFileReader;

        string filePath;
//        MemoryStream stream;
        bool isLoop;
        float volume;

        //private string aliasName;

        //NAudio.Wave.WaveChannel32 waveChannel;
        WaveOut waveOut;
        
        enum Status
        {
            Play,
            End,
        }

        enum SourceType
        {
            File,
            MemoryStream,
        }

        public enum SoundType
        {
            BGM,
            SE,
            Voice,
        }

        private Status status = Status.Play;
        private SoundType soundType;
        private SourceType sourceType;

        public SoundPlayer(string filePath, float volume, bool isLoop, SoundType soudType)
        {
            if ( !File.Exists(filePath) )
            {
                this.filePath = filePath;
                this.volume = volume;
                this.isLoop = isLoop;
                this.soundType = soudType;
                sourceType = SourceType.File;
                return;
            }

            if (soundType == SoundType.BGM)
            {
                lock (soundPlayersLock)
                {
                    foreach (var sp in soundPlayers)
                    {
                        if ( (sp.soundType == SoundType.BGM) && (sp.filePath == filePath) && (sp.status == Status.Play))
                        {
                            // 同じファイルであれば、と切らずそのまま継続する
                            return;
                        }
                    }
                }
            }

            if (coreThread == null)
            {
                //rundamIDManager = new Random.RundamIDManager("Sound_", 16);
                coreThread = new EasyLoopThread(Core, null);
            }

            this.filePath = filePath;
            this.volume = volume;
            this.isLoop = isLoop;
            this.soundType = soudType;
            sourceType = SourceType.File;

            _PlayCore();

            if (soundType == SoundType.BGM)
            {
                lock (soundPlayersLock)
                {
                    foreach (var sp in soundPlayers)
                    {
                        if (sp.soundType == SoundType.BGM)
                        {
                            // Todo : 停止しているだけで、Disposeしてない～
                            sp.Stop();
                        }
                    }
                }
            }

            lock (soundPlayersLock)
            {
                soundPlayers.Add(this);
            }
        }



        private void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if( isLoop && (status == Status.Play))
            {
                //_PlayCore();
                audioFileReader.Position = 0;
                waveOut.Play();
            }
            else
            {
                try
                {
                    // todo : 消去するスレッドが違うとエラーになる 事前に対処できないか？　生成時のスレッドが問題なら、そっちで解決するべきかも？
                    waveOut.Dispose();
                }
                catch
                {
                }

                lock (soundPlayersLock)
                {
                    soundPlayers.Remove(this);
                }
            }
        }

        private void _PlayCore()
        {
            switch (sourceType)
            {
                case SourceType.File:
                    {
                        audioFileReader = new AudioFileReader(filePath);
                        audioFileReader.Volume = volume;
                        
                        waveOut = new WaveOut();
                        waveOut.Init(audioFileReader);
                        waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
                        waveOut.Play();
                    }
                    break;
                //case SourceType.MemoryStream:
                //    {
                //        var pcm = new NAudio.Wave.StreamMediaFoundationReader(stream);
                        
                //        waveOut = new WaveOut();
                //        waveOut.Init(pcm);
                //        waveOut.Volume = volume;
                //        waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
                //        waveOut.Play();

                //    }
                //    break;
            }
        }


        public void Stop()
        {
            status = Status.End;
            if (waveOut != null)
            {
                waveOut.Dispose();
                waveOut = null;
            }
        }

        public void Dispose()
        {
        }

        static public void Clear()
        {
            foreach(var soundPlayer in soundPlayers)
            {
                try
                {
                    soundPlayer.Dispose();
                }
                catch
                {
                    Console.WriteLine("exit");
                }
            }
            soundPlayers.Clear();

            Console.WriteLine("Sound Clear");
        }

        static public void Core( object[] args )
        {
            if(systemClose)
            {
                coreThread.isBreak = true;
                return;
            }

            lock (soundPlayersLock)
            {
                // Todo : サウンド終了時に破棄する処理 リストsoundPlayersの更新が必要、そのさい、rundamIDManagerからIDの削除

                foreach (var soundPlayer in soundPlayers)
                {
                    soundPlayer.Update();
                }
            }

        }

        void Update()
        {
            switch (status)
            {

                case Status.Play:
                    break;
                case Status.End:
                    break;
            }
        }

    }
}
