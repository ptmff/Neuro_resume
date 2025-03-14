namespace back.Models;

public class Resume
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Внешний ключ
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Job { get; set; } // Изменил на Job с заглавной буквы для соответствия стилю
        public List<string> Skills { get; set; } // Изменил на Skills с заглавной буквы для соответствия стилю

        // Навигационное свойство
        public User User { get; set; } // Связь с пользователем
    }