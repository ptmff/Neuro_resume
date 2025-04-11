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

## Вклад и обратная связь

Мы всегда открыты для новых идей и улучшений!  
Если у вас есть предложения или вопросы, пожалуйста, [создайте issue](https://github.com/ptmff/neuro_resume/issues).

---

# Инструкция по запуску проекта Neuro_resume

Ниже приведены подробные шаги, разделённые по функциональным частям, для корректного запуска проекта.

---

## 1. Клонирование репозитория

Сначала клонируйте репозиторий с GitHub:

```bash
git clone https://github.com/ptmff/Neuro_resume

Перейдите в каталог с проектом:

cd Neuro_resume



⸻

2. Настройка серверной части (back.API)

2.1 Создание конфигурационного файла

Перейдите в папку серверной части:

cd back.API

Создайте файл appsettings.json в каталоге back.API (если его ещё нет) со следующим примерным содержимым:

{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=Resume;Username=postgres;Password=пароль"
  },
  "Jwt": {
    "Key": "апи ключ",
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
    "AuthorizationKey": "апи ключ",
    "Scope": "GIGACHAT_API_PERS"
  }
}



2.2 Запуск серверной части

Перейдите в нужную директорию и запустите сервер:

cd ../back.API
dotnet run



⸻

3. Запуск клиентской части (frontend)

Перейдите в директорию с фронтендом, установите зависимости и запустите сервер разработки:

cd ../frontend
npm install
npm run dev



⸻



© 2025 Neuro Resume. Все права защищены.


