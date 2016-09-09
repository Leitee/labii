using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatallaNaval
{
    public class Juego
    {
        private const int PLAYERS = 2;
        private const int CANT_BARCOS = 2;
        private const int TAMAÑO_BARCO = 2;
        private const int TABLERO = 10;
        private const int DAMAGE = 1;
        private const int SALUD = 2;

        User[] users = new User[PLAYERS];
        Tablero tablero = new Tablero(TABLERO);

        static void Main(string[] args)
        {
            bool op = true;
            while (op)
            {
                Juego game = new Juego();
                game.imprimirMatriz(new Tablero(TABLERO).Tamaño);
                game.start();
                game.play();

                Console.WriteLine("\nDesea salir? Si/No");
                op = Console.ReadLine().ToString().ToUpper() != "S";
            }
        }

        private void play()
        {
            bool anyDead = false;
            do
            {
                //ronda de disparos
                for (int i = 0; i < PLAYERS; i++)
                {
                    Console.Write(users[i].Nombre + " es tu turno, ingresa una coordenada valida para disparar: ");
                    var input = (Console.ReadLine()).ToString().ToCharArray();
                    var cord = new Coordenada { Letra = input[0].ToString().ToUpper(), Numero = Convert.ToInt32(input[1].ToString()) };
                    if (users[i].Enemy.shoot(cord))
                    {
                        validarCoordenadas(tablero.Tamaño, false, cord);
                        Console.WriteLine("\nBULLSEYE!!!\n");
                    }
                    else
                    {
                        Console.WriteLine("\nYOU MISS!!!\n");
                    }
                }

                //comprobar daños y/o muerte
                for (int i = 0; i < PLAYERS; i++)
                {
                    for (int j = 0; j < CANT_BARCOS; j++)
                    {
                        if (users[i].Barcos[j].isHundido())
                        {
                            if (anyDead)
                            {
                                Console.WriteLine("\nEs un empate!\n");
                                return;
                            }
                            Console.WriteLine(users[i].Nombre + " No tiene mas barcos!");
                            anyDead = true;
                            break;
                        }                        
                    }
                    Console.WriteLine("{0} tiene {1} de salud!", users[i].Nombre, users[i].Salud);
                }

            } while (!anyDead);
        }

        private void start()
        {
            for (int i = 0; i < PLAYERS; i++)
            {
                Tablero tabla = new Tablero(TABLERO);
                Console.Write("\nIngrese el nombre del {0}° jugador: ", i + 1);
                users[i] = new User(Console.ReadLine(), CANT_BARCOS);

                for (int j = 0; j < CANT_BARCOS; j++)
                {
                    users[i].Barcos[j] = new Barco(CANT_BARCOS * TAMAÑO_BARCO);
                    Coordenada[] cords = new Coordenada[TAMAÑO_BARCO];
                    bool invalido = false;

                    do
                    {
                        for (int h = 0; h < TAMAÑO_BARCO; h++)
                        {
                            Console.Write("{0} ingresa la {1}° coordenada del {2}° barco (Ej A1... B2...): ", users[i].Nombre, h + 1, j + 1);
                            var input = (Console.ReadLine()).ToString().ToCharArray();
                            cords[h] = new Coordenada { Letra = input[0].ToString().ToUpper(), Numero = Convert.ToInt32(input[1].ToString()) };
                        }

                        if (validarCoordenadas(tabla.Tamaño, true, cords))
                        {
                            invalido = false;
                        }
                        else
                        {
                            Console.WriteLine("\nCoordenada invalida, vuelva a ingresarla:\n");
                            invalido = true;
                        }
                    }
                    while (invalido);

                    users[i].Barcos[j].Ubicacion = cords;

                    imprimirMatriz(tabla.Tamaño);
                }
            }
            users[0].Enemy = users[1];
            users[1].Enemy = users[0];
        }

        private void imprimirMatriz(bool[,] t)
        {
            Console.WriteLine("\n\t TABLERO \n");
            for (int i = 0; i < TABLERO; i++)
            {
                for (int j = 0; j < TABLERO; j++)
                {
                    Console.Write((t[i, j]) ? "X" : "O");
                    Console.Write(" ");
                    if (j == 9)
                    {
                        Console.Write("\n");
                    }
                }
            }
            Console.WriteLine("");
        }

        private bool validarCoordenadas(bool[,] tab, bool value, params Coordenada[] cors)
        {
            int cont = 0;

            if (cors.Count() > 1)
            {
                for (int n = 0; n < TABLERO; n++)
                {
                    for (int l = 0; l < TABLERO; l++)
                    {
                        if (!tab[l, n])
                        {
                            if (((int)cors[0].Letra == (int)cors[1].Letra && Math.Abs(cors[0].Numero - cors[1].Numero) == 1)
                                || (cors[0].Numero == cors[1].Numero && Math.Abs((int)cors[0].Letra - (int)cors[1].Letra) == 1))
                            {
                                if ((n == cors[0].Numero && l == (int)cors[0].Letra) || (n == cors[1].Numero && l == (int)cors[1].Letra))
                                {
                                    tab[l, n] = value;
                                    cont++;
                                }
                            }
                        }
                    }
                }
                return cont == 2;
            }
            else
            {
                tab[(int)cors[0].Letra, cors[0].Numero] = value;
                return true;
            }
        }
    }



    class Coordenada
    {
        private int letra;
        public object Letra
        {
            get { return letra; }
            set { letra = Encoding.ASCII.GetBytes(value.ToString())[0] - 65; }
        }

        private int numero;
        public int Numero
        {
            get { return numero; }
            set { numero = value - 1; }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj != null && GetType() == obj.GetType())
            {
                Coordenada cor = (Coordenada)obj;
                if ((int)cor.Letra == (int)this.Letra && cor.Numero == this.Numero)
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Tablero
    {
        private bool[,] tamaño;
        public bool[,] Tamaño
        {
            get { return tamaño; }
            set { tamaño = value; }
        }

        public Tablero(int size)
        {
            tamaño = new bool[size, size];
        }
    }
}
