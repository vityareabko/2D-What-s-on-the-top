using UnityEngine;

namespace Extensions
{
    public static class LayerMaskExtensions
    {
        /// <summary>
        /// Ключевое слово this в аргументах метода расширения (extension method) в C# указывает, что метод является расширением для типа
        /// теперь мы можем использовать этот метод на любом стринг вот так :
        /// string layerName = "Enemy";
        /// LayerMask layerMask = layerName.ToLayerMask();
        ///
        /// если без this в аргументах
        /// то вот так :
        /// LayerMask layerMask = LayerMaskExtensions.ToLayerMask(layerName); 
        /// </summary>
       
        public static LayerMask ToLayerMask(this string layerName)
        {
            return 1 << LayerMask.NameToLayer(layerName);
        }
    }
}