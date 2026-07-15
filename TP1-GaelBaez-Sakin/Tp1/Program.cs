using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tp1
{
    //Realizar un programa que represente una simulación de copos de nieve cayendo en la consola, utilizando el símbolo "*" para cada copo.
    //El programa debe cumplir con las siguientes condiciones:
    //Definir una clase Configuracion que almacene parámetros de la simulación, como la cantidad de filas, columnas y la velocidad de caída de los copos.
    //Definir una clase Copo que modele el comportamiento de un copo de nieve. Cada copo debe tener una posición en la consola y un método para mostrarse y desplazarse hacia abajo.
    //Usar una lista para administrar todos los copos activos durante la simulación.
    //Implementar una lógica que controle la caída de los copos de nieve, evitando que se superpongan en la misma posición.
    //Al completarse una fila con copos en todas las columnas, esta debe eliminarse para permitir que continúe la simulación.
    //El programa debe ejecutarse en un ciclo continuo, simulando de manera animada la caída de los copos

    class Configuracion
    {
        public int Filas = 15;
        public int Columnas = 10;
        public int Velocidad = 250;
    }

    class Copo
    {
        public int X;
        public int Y;

        public Copo(int x)
        {
            X = x;
            Y = 0;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;

            Configuracion config = new Configuracion();

            List<Copo> copos = new List<Copo>();
            bool[,] suelo = new bool[config.Filas, config.Columnas];

            Random random = new Random();

            while (true)
            {
                // Crear un copo en una columna aleatoria
                int columna = random.Next(config.Columnas);
                if (!suelo[0, columna])
                {   //si la primer fila de esa columna esa libre, añade un copo
                    copos.Add(new Copo(columna));
                }

                // Mover copos
                for (int i = copos.Count - 1; i >= 0; i--)
                {
                    Copo c = copos[i];

                    bool llegoAlFondo = c.Y == config.Filas - 1;
                    bool chocaConOtro = !llegoAlFondo && suelo[c.Y + 1, c.X];

                    if (llegoAlFondo || chocaConOtro)
                    {
                        suelo[c.Y, c.X] = true;
                        copos.RemoveAt(i);
                    }
                    else
                    {
                        c.Y++;
                    }
                }

                // Eliminar filas completas
                for (int fila = config.Filas - 1; fila >= 0; fila--)
                {
                    bool completa = true;

                    for (int x = 0; x < config.Columnas; x++)
                    {
                        if (!suelo[fila, x])
                        {
                            completa = false;
                            break;
                        }
                    }

                    if (completa)
                    {
                        for (int y = fila; y > 0; y--)
                        {
                            for (int x = 0; x < config.Columnas; x++)
                            {
                                suelo[y, x] = suelo[y - 1, x];
                            }
                        }

                        for (int x = 0; x < config.Columnas; x++)
                            suelo[0, x] = false;

                        fila++; // volver a revisar la misma fila luego del desplazamiento
                    }
                }

                Console.Clear();

                // Dibujar copos acumulados
                for (int y = 0; y < config.Filas; y++)
                {
                    for (int x = 0; x < config.Columnas; x++)
                    {
                        if (suelo[y, x])
                        {
                            Console.SetCursorPosition(x, y);
                            Console.Write("*");
                        }
                    }
                }

                // Dibujar copos cayendo
                foreach (Copo c in copos)
                {
                    Console.SetCursorPosition(c.X, c.Y);
                    Console.Write("*");
                }

                Thread.Sleep(config.Velocidad);
            }
        }
    }
}
