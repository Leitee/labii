using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatallaNaval
{
    class Barco
    {
        private Coordenada[] ubicacion;

        public Coordenada[] Ubicacion
        {
            get { return ubicacion; }
            set { ubicacion = value; }
        }
        private int salud;

        public int Salud
        {
            get { return salud; }
        }

        public Barco(int salud)
        {
            this.salud = salud;
        }

        public bool hit()
        {
            salud--;
            return salud > 0;
        }

        public bool isHundido()
        {
            return Salud == 0;
        }
    }
}
