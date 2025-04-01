// Типы данных для AI-рекомендаций
export interface AiSuggestion {
  id: string;
  type: 'skills' | 'experience' | 'education' | 'description';
  title: string;
  description: string;
  confidence: number;
  before: any;
  after: any;
  reasoning: string;
  targetExperienceId?: string;
}

export interface AiSuggestionStats {
  totalSuggestions: number;
  estimatedImprovementScore: number;
  targetPositionMatch: {
    before: number;
    after: number;
  };
}

export interface AiSuggestionsResponse {
  success: boolean;
  resumeId: number;
  suggestions: AiSuggestion[];
  stats: AiSuggestionStats;
}

/**
 * Генерирует мок-данные AI-рекомендаций для резюме
 * @param resumeId - ID резюме
 * @param targetPosition - Целевая должность (опционально)
 * @param targetCompany - Целевая компания (опционально)
 * @returns Объект с AI-рекомендациями
 */
export function getMockAiSuggestions(
  resumeId: number = 123,
  targetPosition: string = 'Frontend Developer',
  targetCompany: string = ''
): AiSuggestionsResponse {
  // Базовые рекомендации, которые всегда присутствуют
  const baseSuggestions: AiSuggestion[] = [
    {
      id: 'sug-001',
      type: 'skills',
      title: 'Добавьте ключевые навыки',
      description: `Добавление навыков 'Vue.js' и 'TypeScript' увеличит ваши шансы на 35% для позиции ${targetPosition}`,
      confidence: 0.87,
      before: ['React', 'JavaScript', 'HTML', 'CSS'],
      after: ['React', 'JavaScript', 'HTML', 'CSS', 'Vue.js', 'TypeScript'],
      reasoning: 'Анализ 200+ вакансий показал, что эти навыки востребованы в 78% случаев'
    },
    {
      id: 'sug-002',
      type: 'experience',
      title: 'Улучшите описание опыта работы',
      description: 'Добавьте количественные результаты в описание вашей работы в компании ABC',
      confidence: 0.92,
      targetExperienceId: 'exp-123',
      before: 'Разрабатывал веб-приложения с использованием React',
      after: 'Разработал 5 высоконагруженных веб-приложений с использованием React, что привело к увеличению конверсии на 23%',
      reasoning: 'Количественные показатели делают ваши достижения более убедительными'
    },
    {
      id: 'sug-003',
      type: 'description',
      title: 'Оптимизируйте самоописание',
      description: 'Ваше самоописание слишком общее. Добавьте конкретные профессиональные цели',
      confidence: 0.78,
      before: 'Я опытный разработчик с хорошими навыками программирования',
      after: `${targetPosition} с 5-летним опытом создания высоконагруженных SPA. Специализируюсь на оптимизации производительности и создании доступных интерфейсов`,
      reasoning: 'Конкретное описание вашей специализации и опыта привлекает больше внимания рекрутеров'
    }
  ];

  // Дополнительные рекомендации в зависимости от целевой должности
  let positionSpecificSuggestions: AiSuggestion[] = [];

  if (targetPosition.toLowerCase().includes('frontend')) {
    positionSpecificSuggestions.push({
      id: 'sug-004',
      type: 'skills',
      title: 'Добавьте навыки по фронтенд-фреймворкам',
      description: 'Современные фронтенд-разработчики должны владеть несколькими фреймворками',
      confidence: 0.85,
      before: ['React'],
      after: ['React', 'Vue.js', 'Angular'],
      reasoning: '76% вакансий для фронтенд-разработчиков требуют знания нескольких фреймворков'
    });
  } else if (targetPosition.toLowerCase().includes('backend')) {
    positionSpecificSuggestions.push({
      id: 'sug-004',
      type: 'skills',
      title: 'Добавьте навыки по базам данных',
      description: 'Укажите опыт работы с различными базами данных',
      confidence: 0.89,
      before: ['MySQL'],
      after: ['MySQL', 'PostgreSQL', 'MongoDB', 'Redis'],
      reasoning: '82% вакансий для бэкенд-разработчиков требуют знания нескольких типов баз данных'
    });
  }

  // Дополнительные рекомендации в зависимости от целевой компании
  if (targetCompany) {
    positionSpecificSuggestions.push({
      id: 'sug-005',
      type: 'description',
      title: `Адаптируйте резюме под ${targetCompany}`,
      description: `Добавьте упоминание ценностей и технологий компании ${targetCompany}`,
      confidence: 0.76,
      before: 'Ищу работу в современной IT-компании с интересными проектами',
      after: `Стремлюсь присоединиться к команде ${targetCompany}, чтобы применить свой опыт в разработке инновационных решений, соответствующих ценностям компании`,
      reasoning: 'Персонализированные резюме получают на 65% больше откликов от рекрутеров'
    });
  }

  // Объединяем все рекомендации
  const allSuggestions = [...baseSuggestions, ...positionSpecificSuggestions];

  // Рассчитываем статистику
  const stats: AiSuggestionStats = {
    totalSuggestions: allSuggestions.length,
    estimatedImprovementScore: Math.floor(Math.random() * 30) + 20, // Случайное число от 20 до 50
    targetPositionMatch: {
      before: Math.floor(Math.random() * 30) + 50, // Случайное число от 50 до 80
      after: Math.floor(Math.random() * 15) + 80 // Случайное число от 80 до 95
    }
  };

  return {
    success: true,
    resumeId,
    suggestions: allSuggestions,
    stats
  };
}

/**
 * Генерирует расширенный набор мок-данных AI-рекомендаций
 * @returns Массив объектов с AI-рекомендациями для разных сценариев
 */
export function getExtendedMockAiSuggestions(): AiSuggestionsResponse[] {
  return [
    getMockAiSuggestions(123, 'Frontend Developer', 'Yandex'),
    getMockAiSuggestions(124, 'Backend Developer', 'VK'),
    getMockAiSuggestions(125, 'Full Stack Developer', 'Ozon'),
    getMockAiSuggestions(126, 'UI/UX Designer', 'Avito'),
    getMockAiSuggestions(127, 'Project Manager', 'Sber')
  ];
}

