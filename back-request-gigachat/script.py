import sys
import io
import requests
import uuid
import json
import urllib3
import re

# === 🔧 Консольный вывод с поддержкой UTF-8 (для Windows PowerShell)
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

# === 🔒 Подавляем предупреждения об отключённой проверке сертификатов
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

# === 0. Настройки подключения ===
AUTH_URL = "https://ngw.devices.sberbank.ru:9443/api/v2/oauth"
GIGACHAT_API_URL = "https://gigachat.devices.sberbank.ru/api/v1/chat/completions"
AUTHORIZATION_KEY = "ZTgyZWNmOGEtZDJiNy00N2Q1LWE3MWItZmMzMTgzZjVhNmEwOjE4OWM4NmI1LTJkYjAtNDFjYS04ZTg3LWI2YmViYTA3ZmZlOQ=="  # ← ЗАМЕНИ на свой ключ
SCOPE = "GIGACHAT_API_PERS"

# === 1. Получение Access Token ===
def get_access_token():
    headers = {
        'Content-Type': 'application/x-www-form-urlencoded',
        'Accept': 'application/json',
        'RqUID': str(uuid.uuid4()),
        'Authorization': f'Basic {AUTHORIZATION_KEY}'
    }
    data = {'scope': SCOPE}
    response = requests.post(AUTH_URL, headers=headers, data=data, verify=False)
    response.raise_for_status()
    token = response.json().get("access_token")
    if not token:
        raise RuntimeError("Access token not found in response")
    return token

# === 2. Формирование payload для GigaChat ===
def build_payload(resume_data: dict) -> dict:
    system_prompt = """
Ты — профессиональный AI-движок по улучшению резюме.

Ты получаешь резюме в формате JSON. Ты должен вернуть массив объектов-предложений строго в следующем формате:

[
  {
    "id": "строка (sug-{уникальныйId})",
    "type": "experience | skills | description | etc",
    "title": "строка",
    "description": "строка",
    "confidence": число (от 0.0 до 1.0),
    "before": "строка или массив строк",
    "after": "строка или массив строк",
    "reasoning": "строка"
  }
]

🛑 ВАЖНЫЕ ПРАВИЛА (СТРОГО):
- НЕ предлагай улучшения для образования — полностью пропускай.
- Если type === "skills":
  - "before" и "after" должны быть массивами строк.
  - Добавляй только реалистичные новые навыки.
- Если type === "experience":
  - "before" — обычная строка (текущее описание работы),
  - "after" должно быть: "`{целеваяДолжность}` + твоя улучшенная версия".
  - Текст должен быть одной строкой, не списком.
- Если type === "description":
  - Те же правила, что и для "experience": строка → строка с префиксом `{целеваяДолжность}`.
- Никогда не используй шаблоны типа "{company}" или "{description}" — только реальные данные.
- Сохраняй форматирование: если "before" был списком, "after" тоже должен быть списком.
- Не используй Markdown или комментарии — только чистый JSON.

Результат ДОЛЖЕН быть строго валидным JSON.
""".strip()

    return {
        "model": "GigaChat",
        "stream": False,
        "update_interval": 0,
        "messages": [
            {"role": "system", "content": system_prompt},
            {"role": "user", "content": json.dumps(resume_data, ensure_ascii=False)}
        ]
    }

# === 3. Отправка запроса и обработка ответа ===
def analyze_resume(resume_data: dict) -> list:
    try:
        token = get_access_token()
        headers = {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': f'Bearer {token}'
        }
        payload = build_payload(resume_data)

        response = requests.post(GIGACHAT_API_URL, headers=headers, json=payload, verify=False)
        response.encoding = 'utf-8'
        response.raise_for_status()
        response_json = response.json()

        print("📦 RAW response:")
        print(json.dumps(response_json, indent=2, ensure_ascii=False))

        # Получаем message.content
        message_content = response_json.get('choices', [{}])[0].get('message', {}).get('content', '')

        print("\n🧾 Raw message_content:")
        print(message_content)

        # Удаляем Markdown-обёртку ```json ... ```
        cleaned_content = re.sub(r"^```json\s*|\s*```$", "", message_content.strip(), flags=re.DOTALL)

        try:
            suggestions = json.loads(cleaned_content)
        except Exception as parse_err:
            print("❌ JSON parsing error:", parse_err)
            print("🔎 Cleaned content:")
            print(cleaned_content)
            return []

        # Валидация структуры
        assert isinstance(suggestions, list), "Result is not a list"
        for item in suggestions:
            assert all(key in item for key in [
                "id", "type", "title", "description", "confidence", "before", "after", "reasoning"
            ]), f"Invalid format in: {item}"

        return suggestions

    except Exception as e:
        print("❌ Error during resume analysis:", e)
        return []

# === 4. Пример резюме на русском ===
if __name__ == "__main__":
    resume_data = {
        "id": 1,
        "title": "Резюме старшего фронтенд-разработчика",
        "city": "Сан-Франциско",
        "job": "Фронтенд-разработчик",
        "experience": [
            {
                "company": "Google",
                "position": "Фронтенд-инженер",
                "startDate": "2022-01",
                "endDate": "2024-12",
                "description": "Разработал 5 высоконагруженных приложений на React, увеличив конверсию пользователей на 23%."
            },
            {
                "company": "CodeCorp",
                "position": "Младший разработчик",
                "startDate": "2020-01",
                "endDate": "2021-12",
                "description": "Разрабатывал внутренние инструменты на Vue.js и TypeScript."
            }
        ],
        "education": [],
        "skills": ["React", "TypeScript", "Redux"],
        "template": "modern",
        "date": "2025-03-30T00:00:00Z",
        "userId": 123,
        "description": "Опытный фронтенд-разработчик, стремящийся присоединиться к команде с высоким влиянием на продукт."
    }

    suggestions = analyze_resume(resume_data)
    print("\n✅ Предложения:")
    print(json.dumps(suggestions, ensure_ascii=False, indent=2))
