import requests
from bs4 import BeautifulSoup
from urllib.parse import urlparse

def normalize_url(url):
    # Если схема отсутствует, добавляем https://
    if not url.startswith(("http://", "https://")):
        url = "https://" + url

    parsed = urlparse(url)
    # Удаляем "www." из домена, если оно есть
    netloc = parsed.netloc.replace("www.", "")
    
    # Ожидаем, что путь имеет вид "/vacancy/<id>"
    path_parts = parsed.path.split('/')
    if len(path_parts) >= 3 and path_parts[1] == 'vacancy':
        vacancy_id = path_parts[2]
        return f"https://hh.ru/vacancy/{vacancy_id}"
    else:
        return None

def parse_page(url):
    headers = {
        'User-Agent': ('Mozilla/5.0 (Windows NT 10.0; Win64; x64) '
                       'AppleWebKit/537.36 (KHTML, like Gecko) '
                       'Chrome/90.0.4430.93 Safari/537.36')
    }
    try:
        response = requests.get(url, headers=headers)
        response.raise_for_status()  # Проверка на ошибки HTTP
        soup = BeautifulSoup(response.text, 'html.parser')
        return soup.get_text().strip()
    except requests.exceptions.RequestException as e:
        return f"Ошибка: {e}"

if __name__ == "__main__":
    raw_url = input("Введите URL вакансии: ")
    normalized_url = normalize_url(raw_url)
    
    if normalized_url:
        print(f"Нормализованный URL: {normalized_url}\n")
        text = parse_page(normalized_url)
        print("Полученный текст:\n")
        print(text)
    else:
        print("Ошибка: не удалось нормализовать URL")
