## ASP.NET WEB API: готовое ядро/шаблон для будущего проекта
Разработка ядра/шаблона для будущих проектов с использованием ASP.NET Core API и принципов Чистой архитектуры. Этот проект даст базовую гибкую и масштабируемую основу для разработки, обеспечивая четкое разделение слоев и ответственности, что упростит поддержку и расширение проекта в дальнейшем.

## Структура проекта

Модульный подход помогает разделить функциональные компоненты, облегчая поддержку и улучшая масштабируемость, а также упрощает работу с отдельными модулями, их тестирование и устранение неполадок.

```
├── src
│   ├── Core                    # Доменный слой
│   │   ├── Common              # Общие утилиты и вспомогательные функции
│   │   ├── Entities            # Классы, представляющие основные сущности
│   │   ├── Exceptions          # Пользовательские исключения, используемые для обработки ошибок
│   │   ├── Interfaces          # Интерфейсы, определяющие контракты для сервисов и репозиториев
│   │   └── Services            # Сервисы приложения, содержащие основную бизнес-логику
│   │ 
│   ├── Infrastructure          # Инфраструктурный слой
│   │   ├── Data                # Классы контекста базы данных и конфигурации доступа к данным
│   │   ├── Migrations          # Миграции для базы данных, определяющие изменения в структуре данных
│   │   └── Repositories        # Репозитории для доступа к данным и внешним системам
│   │ 
│   └── API                     # Слой API
│   │   ├── Controllers         # Контроллеры, обрабатывающие HTTP-запросы и вызывающие бизнес-логику
│   │   ├── Extensions          # Методы расширения для настройки сервисов
│   │   ├── Helpers             # Вспомогательные классы и функции
│   │   ├── Middlewares         # Middleware для обработки запросов
│   │   └── Program.cs          # Основной файл для запуска приложения
│   │ 
└── README.md                   # Документация проекта, описывающая установку, настройку и использование            
```

## Использование

Для использования данного проекта выполните следующие шаги:

1. Убедитесь, что на вашем компьютере установлен SDK .NET 7.
2. Скопируйте или загрузите данный репозиторий на локальный компьютер.
3. Откройте решение в среде разработки (IDE).
4. Выполните сборку решения для восстановления пакетов NuGet и компиляции кода.
5. Настройте параметры подключения к базе данных в файле `appsettings.json` проекта `Infrastructure`.
6. Откройте консоль диспетчера пакетов, выберите проект `Infrastructure` и выполните команду `Update-Database` для создания базы данных.
7. Запустите приложение, открыв проект `API`.

## Особенности проекта

Этот проект включает в себя следующие функции:

- **Чистая архитектура (Clean Architecture)**: Структура проекта построена на принципах Чистой архитектуры, что способствует разделению ответственности.
- **Принципы SOLID**: Код соответствует принципам SOLID, что упрощает его поддержку и расширение.
- **Шаблон репозитория (Repository Pattern)**: Шаблон репозитория абстрагирует слой доступа к данным и предоставляет единый интерфейс для работы с ними.
- **Entity Framework Core**: В проекте используется Entity Framework Core в качестве инструмента ORM (Object-Relational Mapping) для доступа к данным.
- **ASP.NET Core API**: В проекте имеется проект ASP.NET Core API, который служит слоем API, обрабатывающим HTTP-запросы и ответы.
- **JWT для аутентификации с использованием токенов**: Легкость в управлении сессиями пользователей, аутентификацией и авторизацией с использованием современного подхода на основе JWT-токенов.
- **Операции CRUD**: Проект предоставляет основу для реализации полного набора операций CRUD (создание, чтение, обновление, удаление) с использованием Entity Framework Core.
- **Внедрение зависимостей (Dependency Injection)**: В проекте используется встроенный контейнер внедрения зависимостей в ASP.NET Core, что упрощает управление и внедрение зависимостей в различных частях приложения.

## Технологии

1. **Языки программирования и фреймворки:**
   - **C#** – Язык программирования.
   - **ASP.NET Core (7.0)** – Фреймворк для создания веб-приложений.

2. **Технологии для работы с базами данных:**
   - **Entity Framework Core** – ORM для работы с базами данных в .NET.
   - **PostgreSQL** – Система управления базами данных.

3. **Безопасность и аутентификация:**
   - **JwtBearer** – Механизм аутентификации с использованием JWT (JSON Web Tokens).

4. **Документирование и тестирование API:**
   - **Swagger** – Инструмент для генерации документации API и тестирования.
