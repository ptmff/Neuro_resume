import requests
from bs4 import BeautifulSoup

def parse_page(url):
    headers = {
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) ' 
                      'AppleWebKit/537.36 (KHTML, like Gecko) ' 
                      'Chrome/90.0.4430.93 Safari/537.36'
    }
    try:
        response = requests.get(url, headers=headers)
        response.raise_for_status()  # Проверка на ошибки HTTP
        soup = BeautifulSoup(response.text, 'html.parser')
        return soup.get_text().strip()
    except requests.exceptions.RequestException as e:
        return f"Ошибка: {e}"

if __name__ == "__main__":
    url = input("Введите URL страницы: ")
    page_text = parse_page(url)
    print("\nПолученный текст:\n")
    print(page_text)
