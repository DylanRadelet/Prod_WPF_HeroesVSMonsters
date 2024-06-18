using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Heroes_VS_Monsters.Class.Monstre
{
    public class Dragonnet : Monstre
    {
        

        #region Properties

        public override double BonusEnd
        {
            get => 1;
        }

        public override int Cuir
        {
            get => 3;
        }

        public override int Gold
        {
            get => 8;
        }

        #endregion



        #region constructor
        public Dragonnet(string race, int @for, int end, double pdv, double position, int gold = 8, int cuir = 3)
            : base(@for, end, pdv, position, gold, cuir, race)
        {
        }

        #endregion

    }
}
