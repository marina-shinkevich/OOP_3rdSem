using System;

namespace FurnitureStore
{
    public abstract partial class Furniture: Product, IAssemble
    {
        public virtual void Assemble()
        {
            Console.WriteLine($"Сборка {Name} из {Material} цвета {Color}, размер: {Size}.");
        }

        

        public override void ShowInfo()
        {
            Console.WriteLine($"Общая информация - Мебель: {Name}, Материал: {Material}, Цвет: {Color}, Размер: {Size}, Цена: {Price} руб., Вес: {Weight} кг");
        }

        
    }
}
