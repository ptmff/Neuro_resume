// mocks/mockAiSuggestions.ts

import type { AiSuggestion } from '@/stores/resumesStore'

export interface AiSuggestionsResponse {
  success: boolean;
  resumeId: number;
  suggestions: AiSuggestion[];
  stats: {
    totalSuggestions: number;
    estimatedImprovementScore: number;
    targetPositionMatch: {
      before: number;
      after: number;
    };
  };
}

export const getMockAiSuggestions = (
  resumeId: number = 123,
  targetPosition: string = 'Frontend Developer',
  targetCompany: string = '',
  experienceIds?: string[]
): AiSuggestionsResponse => {
  const targetExperienceId = experienceIds?.[0] || null;

  const suggestions: AiSuggestion[] = [
    {
      "id": "sug-dobavki-s-fronta",
      "type": "experience",
      "title": "Frontend-разработчик",
      "description": "Разработал 5 высоконагруженных веб-приложений с использованием React, что привело к увеличению конверсии на 23%.",
      "confidence": 0.9,
      "before": "Разработал 5 высоконагруженных веб-приложений с использованием React, что привело к увеличению конверсии на 23%.",
      "after": "Разработал 5 высоконагруженных веб-приложений с использованием React, что привело к увеличению конверсии на 23%, используя современные подходы и лучшие практики.",
      "reasoning": "Улучшение описания опыта работы путем добавления фразы 'используя современные подходы и лучшие практики'."
    },
    {
      "id": "sug-depresscorp",
      "type": "experience",
      "title": "Frontend-разработчик",
      "description": "Жёстко депрессировал из-за неработающих запросов.",
      "confidence": 0.8,
      "before": "Жёстко депрессировал из-за неработающих запросов.",
      "after": "Пробовал оптимизировать работу сервера и улучшить UX/UI дизайна, но столкнулся с некоторыми трудностями.",
      "reasoning": "Замена негативного описания на более нейтральное и конструктивное."
    }
  ]
  

  const stats = {
    totalSuggestions: suggestions.length,
    estimatedImprovementScore: 38,
    targetPositionMatch: {
      before: 64,
      after: 88
    }
  };

  return {
    success: true,
    resumeId,
    suggestions,
    stats
  };
};
