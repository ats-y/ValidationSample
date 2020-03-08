using System;
namespace ValidationSample.Models
{
    /// <summary>
    /// 材料クラス。
    /// </summary>
    public class Material
    {
        /// <summary>
        /// 材料名。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 依頼量。
        /// </summary>
        public decimal OrderQuantity { get; set; }

        public Material()
        {
        }
    }
}
