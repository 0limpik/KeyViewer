using System;

namespace EventHook
{
    /// <summary>
    /// Атрибут для описания клавиш
    /// </summary>
    public class KeyAttribute : Attribute
    {
        /// <summary>
        /// Название по умолчанию
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Описание по умолчанию
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Отображать ли клавишу для редактирования
        /// </summary>
        public bool Diplay { get; }

        /// <summary>
        /// Стандартный конструстор
        /// </summary>
        /// <param name="Name">Название</param>
        /// <param name="Description">Описание</param>
        public KeyAttribute(
            string Name,
            string Description,
            bool Diplay = true)
        {
            this.Name = Name;
            this.Description = Description;
            this.Diplay = Diplay;
        }
    }
}
