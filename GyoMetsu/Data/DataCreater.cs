using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyoMetsu.Data
{

    public class DataCreater
    {
        static public DataCreater Instance;
        
        public List<Character> playerCharacters = new List<Character>();
        public List<Character> enemyCharacters = new List<Character>();  // Todo : エネミーの選択をできるようにしたい

        public DataCreater()
        {
            Instance = this;
        }

        public void Update()
        {
            foreach( var character in playerCharacters )
            {
                character.Update();
            }
            foreach (var character in enemyCharacters)
            {
                character.Update();
            }
        }

        public bool IsPlayerCharacterAllDead()
        {
            var j = 0;
            foreach ( var i in playerCharacters)
            {
                if (i.HP.Now == 0) j++;
            }
            if (j == playerCharacters.Count) return true;
            return false;
        }

    }
}
