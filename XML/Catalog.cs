using System.Collections.Generic;

namespace XML
{
    /// <summary>
    /// Каталог телефонных номеров.
    /// </summary>
    public class Catalog
    {
        /// <summary>
        /// Список телефонных номеров.
        /// </summary>
        public List<Phone> Phones { get; set; } = new List<Phone>();
    }
}
