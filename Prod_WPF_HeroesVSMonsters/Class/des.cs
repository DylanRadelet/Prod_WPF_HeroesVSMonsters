using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heroes_VS_Monsters.Class
{
    public class Des
    {
        #region field
        //================================================================================================
        // ====> FIELD
        //================================================================================================

        private Random random;
        #endregion

        #region Fonction
        //================================================================================================
        // ====> FONCTION
        //================================================================================================
        public int Lancer()
        {
            List<int> resultats = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                resultats.Add(random.Next(1, 7));
            }

            var troisMeilleurs = resultats.OrderByDescending(n => n).Take(3);

            return troisMeilleurs.Sum();

            //int a = rolls.Sum() - rolls.Min();
        }

        #endregion
    }
}

