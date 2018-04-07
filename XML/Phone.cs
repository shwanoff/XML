namespace XML
{
    /// <summary>
    /// Телефонный контакт.
    /// </summary>
    public class Phone
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Телефонный номер.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Заметка.
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Важный контакт.
        /// </summary>
        public bool Important { get; set; }

        /// <summary>
        /// Приведение объекта к строке.
        /// </summary>
        /// <returns> Имя. </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
