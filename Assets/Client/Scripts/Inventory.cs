using System;
using System.Collections.Generic;

namespace LD52
{
    [Serializable]
    public class Inventory //todo replicate content on host migration?
    {
        public List<ItemState> Items = new();

        public int Height
        {
            get => Taken.GetLength(1);
            set
            {
                if (value > 0)
                {
                    var newTaken = new int[value, Height];
                    for (int i = 0; i < newTaken.GetLength(0); i++)
                    {
                        for (int j = 0; j < newTaken.GetLength(1); j++)
                        {
                            if (i < Taken.GetLength(0) && j < Taken.GetLength(1))
                            {
                                newTaken[i, j] = Taken[i, j];
                            }
                            else
                            {
                                newTaken[i, j] = -1;
                            }
                        }
                    }

                    Taken = newTaken;
                }
            }
        }

        public int Width
        {
            get => Taken.GetLength(0);
            set
            {
                if (value > 0)
                {
                    var newTaken = new int[Width, value];
                    for (int i = 0; i < newTaken.GetLength(0); i++)
                    {
                        for (int j = 0; j < newTaken.GetLength(1); j++)
                        {
                            if (i < Taken.GetLength(0) && j < Taken.GetLength(1))
                            {
                                newTaken[i, j] = Taken[i, j];
                            }
                            else
                            {
                                newTaken[i, j] = -1;
                            }
                        }
                    }

                    Taken = newTaken;
                }
            }
        }
        public int[,] Taken = new int[1,1] { {-1}};
    }
}