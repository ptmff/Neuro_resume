# Neuro Resume

[![Последний коммит](https://img.shields.io/github/last-commit/ptmff/neuro_resume?style=flat-square)](https://github.com/ptmff/neuro_resume/commits/main)
[![Размер репозитория](https://img.shields.io/github/repo-size/ptmff/neuro_resume?style=flat-square)](https://github.com/ptmff/neuro_resume)
[![Лицензия](https://img.shields.io/github/license/ptmff/neuro_resume?style=flat-square)](LICENSE)

Neuro Resume — инновационный проект, использующий нейросетевые алгоритмы для создания, анализа и оптимизации резюме. Проект автоматически распознаёт ключевую информацию, оценивает данные и генерирует персональные рекомендации по улучшению документа. Современный интерактивный интерфейс делает работу с резюме простой и удобной.

---

## Особенности проекта

- **Анализ резюме:** Автоматическое распознавание ключевых данных и оценка профессионального опыта.
- **Персональные рекомендации:** Генерация советов по улучшению структуры и содержания резюме.
- **Интерактивный интерфейс:** Удобное и современное оформление для пользователей.

---

## Наша команда:

<table>
  <tr>
    <td align="center">
      <a href="https://github.com/ptmff">
        <img src="https://github.com/ptmff.png" width="100px;" alt="ptmff"/><br />
        <sub><b>ptmff</b></sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/denis-mist">
        <img src="https://github.com/denis-mist.png" width="100px;" alt="username2"/><br />
        <sub><b>denis-mist</b></sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/fuglyy">
        <img src="https://github.com/fuglyy.png" width="100px;" alt="fuglyy"/><br />
        <sub><b>fuglyy</b></sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/n1ck0n">
        <img src="https://github.com/n1ck0n.png" width="100px;" alt="n1ck0n"/><br />
        <sub><b>n1ck0n</b></sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/xle6yc3k">
        <img src="https://github.com/xle6yc3k.png" width="100px;" alt="xle6yc3k"/><br />
        <sub><b>xle6yc3k</b></sub>
      </a>
    </td>
  </tr>
</table>


---


## Запуск проекта

### 1. Клонирование репозитория

Откройте терминал и выполните команду:
```bash
git clone https://github.com/ptmff/neuro_resume.git
```

### 2. Настройка конфигурации бэкенда

В каталоге `back/back.API` создайте (или отредактируйте) файл `appsettings.json` с необходимыми параметрами подключения к базе данных, JWT и API ИИ. Пример (без конфиденциальных данных):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=Resume;Username=postgres;Password=ВАШ_ПАРОЛЬ"
  },
  "Jwt": {
    "Key": "ВАШ_СЕКРЕТНЫЙ_КЛЮЧ",
    "Issuer": "your_issuer",
    "Audience": "your_audience",
    "ExpireDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AI": {
    "AuthUrl": "https://ngw.devices.sberbank.ru:9443/api/v2/oauth",
    "GigaChatApiUrl": "https://gigachat.devices.sberbank.ru/api/v1/chat/completions",
    "AuthorizationKey": "ВАШ_КЛЮЧ",
    "Scope": "GIGACHAT_API_PERS"
  }
}
```

### 3. Запуск бэкенда

Перейдите в каталог проекта бэкенда и запустите приложение:
```bash
cd back/back.API
dotnet run
```
После запуска бэкенд будет доступен по адресу:
```
http://localhost:5184/swagger
```
Swagger UI позволит вам протестировать API.

### 4. Запуск фронтенда

Перейдите в каталог проекта фронтенда:
```bash
cd frontend
npm install
npm run dev
```
Фронтенд будет доступен по адресу:
```
http://localhost:5173/
```

---


## Вклад и обратная связь

Мы всегда открыты для новых идей и улучшений!  
Если у вас есть предложения или вопросы, пожалуйста, [создайте issue](https://github.com/ptmff/neuro_resume/issues).

---

© 2025 Neuro Resume. Все права защищены.


