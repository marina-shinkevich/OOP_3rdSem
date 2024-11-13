using System;

namespace FurnitureStore
{
    // Базовый класс Furniture, наследуемый от Product
    public abstract partial class Furniture : Product, IAssemble
    {
        public MaterialType Material;
        public string Color ;
        public Dimensions Size; // Размеры мебели

        public Furniture(string name, decimal price, double weight, MaterialType material, string color, Dimensions size)
            : base(name, price, weight)
        {
            Material = material;
            Color = color;
            Size = size;
        }


    }
}
