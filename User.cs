using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatallaNaval
{
    class User
    {
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
        }

        private Barco[] barcos;
        public Barco[] Barcos
        {
            get { return barcos; }
            set { barcos = value; }
        }

        private User enemy;

        public User Enemy
        {
            get { return enemy; }
            set { enemy = value; }
        }
        

        private int salud;
        public int Salud
        {
            get
            {
                salud = barcos.AsEnumerable().Sum(b => b.Salud);
                return salud;
            }
        }

        public User(string nombre, int cant)
        {
            this.nombre = nombre;
            this.Barcos = new Barco[cant];
        }

        public bool shoot(Coordenada cor)
        {
            foreach (Barco b in barcos)
            {
                for (int i = 0; i < b.Ubicacion.Length; i++)
                {
                    if (b.Ubicacion[i].Equals(cor))
                    {
                        b.Ubicacion[i] = null;
                        b.hit();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isHeOver()
        {
            return barcos.Count(b => b != null) == 0;
        }
    }
}
